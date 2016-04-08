//-----------------------------------------------------------------------------
// stoplight.fx
//-----------------------------------------------------------------------------


//Textures
sampler TextureSampler : register(s0);

//Pixel Shader Variables
float timer;
float flash;
float distance;
float2 positions[5];
float radius;
float offset;


float4 PixelShaderFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	float4 tex = tex2D(TextureSampler, texCoord);
	float4 newColor = float4(1, 1, 1, 1);
	
	float isFlash = flash % 100;
	float blink = (60 - isFlash) / 10;
	if(blink < 0) blink = 0;

	float circle = radius*radius;

	for(int i = 0; i < 3; i++)
	{
		float a = texCoord.x - positions[i].x + distance;
		float b = texCoord.y - positions[i].y;
		a = a*a*2.5;
		b = b*b;

		if((a + b) < circle)
		{
			float fade = ((circle - (a+b)) / (circle) / 2);

			newColor = float4(1 - fade, 1, 1 - fade, 1);

			if(timer > 300)
				if(isFlash < 50)
					newColor = float4(1, 1, 1 - fade, 1);
				else
				{
					newColor = float4(1, 1, 1 - (fade * blink), 1);
				}

			if(timer > 600)
					newColor = float4(1, 1 - fade, 1 - fade, 1);
		}
	}
	return tex * newColor;
}

technique Stoplight
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}