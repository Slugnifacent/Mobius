//-----------------------------------------------------------------------------
// rave.fx
//-----------------------------------------------------------------------------


//Textures
sampler TextureSampler : register(s0);

//Pixel Shader Variables
float4 ravePositions[10];
float3 raveColors[12];
int index2;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	float4 tex = tex2D(TextureSampler, texCoord);
	float fade;
	float a;
	float b;
	float c;
	float d;
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

technique Rave
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}