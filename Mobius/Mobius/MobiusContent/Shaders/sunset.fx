//-----------------------------------------------------------------------------
// sunset.fx
//-----------------------------------------------------------------------------


//Textures
sampler TextureSampler : register(s0);

//Pixel Shader Variables
float2 position;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	float4 tex = tex2D(TextureSampler, texCoord);

	float a = texCoord.x - position.x;
	float b = texCoord.y - position.y;
	
	a = a*a*2.5;
	b = b*b;

	float circle = 0.6;

	float4 newColor = float4(1, 1, 1, 1);

	if((a + b) < circle)
	{
		float fade = ((circle - (a+b)) / (circle) / 2);
		if(fade < 0) fade = 0;
		float4 addColor = float4(tex.z * fade, tex.z * fade, -0.2 * fade, 1);
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

technique Sunset
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}