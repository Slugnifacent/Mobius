//-----------------------------------------------------------------------------
// rain.fx
//-----------------------------------------------------------------------------


//Textures
sampler TextureSampler : register(s0);
sampler RainSampler : register(s1);

//Pixel Shader Variables
float distance;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	float4 tex = tex2D(TextureSampler, texCoord);
	float2 movedTexCoord;
	movedTexCoord = float2(texCoord.x + distance/10, (texCoord.y - distance));
	float4 rain1 = tex2D(RainSampler, movedTexCoord);
	movedTexCoord = float2(texCoord.x + distance/10, (texCoord.y - distance + 1));
	float4 rain2 = tex2D(RainSampler, movedTexCoord);

	if(rain1.x > 0.1 || rain2.x > 0.1)
	{
		return float4(0.4, 0.4, 0.6, 1);
	}
	return tex;
}

technique Rain
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}