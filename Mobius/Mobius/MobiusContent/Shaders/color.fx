//-----------------------------------------------------------------------------
// color.fx
//-----------------------------------------------------------------------------


//Textures
sampler TextureSampler : register(s0);

//Pixel Shader Variables
float alpha;
float level;


float4 PixelShaderFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	float4 tex = tex2D(TextureSampler, texCoord);
	float4 red = float4(1, 1, 1, 1);
	float redAlpha = alpha;

	if(redAlpha > 255) redAlpha = 510 - redAlpha;

	if(level == 1)
	{
		red = float4(1, 1- redAlpha/255, 1 - redAlpha/255, 1);
	}

	if(level == 2)
	{
		if((texCoord.x > 0.1 || texCoord.x < 0.9) && texCoord.y < 0.92)
		red = float4(1- redAlpha/1000, 1 - redAlpha/255, 1, 1);
	}

	if(level == 3)
	{
		//if(texCoord.x > 0.1 || texCoord.x < 0.9 && texCoord.y < 0.9)
			red = float4(1 - redAlpha/255, 1, 1 - redAlpha/255, 1);
	}

	if(level == 4)
	{
		red = float4(0, 0, 0, 1);
	}

	return tex * red;
}

technique Color
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}