//-----------------------------------------------------------------------------
// stoplight.fx
//-----------------------------------------------------------------------------


//Textures
sampler TextureSampler : register(s0);

//Pixel Shader Variables
float timer;
float flash;


float4 PixelShaderFunction(float2 texCoord : TEXCOORD0, 
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
	return (1, 0, 0, 1);
	return tex;
}

technique Stoplight
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}