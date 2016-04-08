//-----------------------------------------------------------------------------
// ribbon.fx
//-----------------------------------------------------------------------------


//Textures
sampler TextureSampler : register(s0);

//Pixel Shader Variables
float distance;
float pi;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	float offset = sin(texCoord.x * 2 * pi) / 20; //for other shader, use 40
	// for other shader use texCoord.x -= 0.5;
	float2 newTexCoord = float2(texCoord.x, texCoord.y + offset); //for other shader add distance to x coord
	float4 output = tex2D(TextureSampler, newTexCoord);

	return output;
	
}

technique Ribbon
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}