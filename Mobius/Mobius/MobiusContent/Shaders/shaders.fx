//-----------------------------------------------------------------------------
// stoplight.fx
//-----------------------------------------------------------------------------


//Textures
sampler TextureSampler : register(s0);
sampler RainSampler : register(s1);

//Pixel Shader Variables
float timer;
float flash;
float rainDistance;
float2 sunPosition;
float4 ravePositions[2];
float3 raveColors[12];
int index2;

float4 RaveFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	float4 tex = tex2D(TextureSampler, texCoord);
	float fade;
	float a;
	float b;
	float circle;
	bool inCircle = false;
	int index = 0;

		a = texCoord.x - ravePositions[index2].x;
		b = texCoord.y - ravePositions[index2].y;
		a = a*a*2.5;
		b = b*b;
		circle = 0.04;

		if((a + b) < circle)
		{
			inCircle = true;

			fade = ((circle - (a+b)) / (circle) / 2);

			index = ravePositions[index2].w;
		}

		if(inCircle)
		{
			float4 output = (tex * 3) * float4(1 - (fade * raveColors[index].x), 1 - (fade * raveColors[index].y), 1 - (fade * raveColors[index].z), 1);
			return output;
		}
	return tex;
}

float4 RainFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	float4 tex = tex2D(TextureSampler, texCoord);
	float2 movedTexCoord;
	movedTexCoord = float2(texCoord.x + rainDistance/10, (texCoord.y - rainDistance));
	float4 rain1 = tex2D(RainSampler, movedTexCoord);
	movedTexCoord = float2(texCoord.x + rainDistance/10, (texCoord.y - rainDistance + 1));
	float4 rain2 = tex2D(RainSampler, movedTexCoord);

	if(rain1.x > 0.1 || rain2.x > 0.1)
	{
		return float4(0.4, 0.4, 0.6, 1);
	}
	return tex;
}


float4 SunsetFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	float4 tex = tex2D(TextureSampler, texCoord);

	float a = texCoord.x - sunPosition.x;
	float b = texCoord.y - sunPosition.y;
	
	a = a*a*5.5;
	b = b*b;

	float circle = 0.6;

	float4 newColor = float4(1, 1, 1, 1);

	if((a + b) < circle)
	{
		float fade = ((circle - (a+b)) / (circle) / 2);
		if(fade < 0) fade = 0;
		float4 addColor = float4(tex.z * fade, -0.8 * fade, -0.4 * fade, 1);
		newColor = float4(1, 1 * (1 - fade), 1 * (1 - fade), 1);
		float4 output = (tex + addColor) * newColor;

		if((a + b) < 0.02)
		{
			return output * (output * 1.7);
		}

		return output;
	}

	return tex * newColor;
}

float4 StoplightFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	float4 tex = tex2D(TextureSampler, texCoord);

	float2 testCoord;
	float4 testTex;

	float a = abs(tex.x - tex.y);
	float b = abs(tex.y - tex.z);
	float c = abs(tex.z - tex.x);

	float isFlash = flash % 100;
	float blink = (60 - isFlash) / 10;
	if(blink < 0) blink = 0;

	if(abs(a - b) > 0.1 && abs(b - c) > 0.7) //red
	{
		if(timer > 600) return tex;
		return float4(0, 0, 0, 1);
	}
	if(abs(c - a) > 0.5 && abs(b - c) > 0.5) //green
	{
		if(timer < 300) return tex;
		return float4(0, 0, 0, 1);
	}
	if(abs(a - b) > 0.2 && abs(c - a) > 0.2) //yellow
	{
		if(timer > 300 && timer < 600)
		{
			if(isFlash < 50) return float4(1, 1, 0, 1);
			return float4(1 - blink, 1 - blink, 0, 1);
		}
		return float4(0, 0, 0, 1);
	}
	
	float2 movedTexCoord;
	movedTexCoord = float2(texCoord.x + rainDistance/10, (texCoord.y - rainDistance));
	float4 rain1 = tex2D(RainSampler, movedTexCoord);
	movedTexCoord = float2(texCoord.x + rainDistance/10, (texCoord.y - rainDistance + 1));
	float4 rain2 = tex2D(RainSampler, movedTexCoord);

	if(rain1.x > 0.1 || rain2.x > 0.1)
	{
		return float4(0.4, 0.4, 0.6, 1);
	}
	return tex;

}

float4 RibbonFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{

	float offset = sin(3.14) / 40;
	// for other shader use texCoord.x -= 0.5;
	float2 newTexCoord = float2(texCoord.x, texCoord.y + offset); //for other shader add distance to x coord
	float4 output = tex2D(TextureSampler, newTexCoord);

	return output;
}


float4 NoTechFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	return tex2D(TextureSampler, texCoord);
}


technique Sunset
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 SunsetFunction();
	}
}

technique NoTech
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 NoTechFunction();
	}
}

technique Stoplight
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 StoplightFunction();
    }
}

technique Rave
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 RaveFunction();
	}
}

technique Rain
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 RainFunction();
	}
}

technique Ribbon
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 RibbonFunction();
	}
}