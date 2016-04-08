//-----------------------------------------------------------------------------
// bloom.fx
//-----------------------------------------------------------------------------


//Textures
sampler TextureSampler : register(s0);

//Pixel Shader Variables
float2 position;
float radius;
float offset;
float maxRadius;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0, 
		float4 color : COLOR0) : COLOR0
{
	float4 tex = tex2D(TextureSampler, texCoord);

	float a = texCoord.x - position.x;
	float b = texCoord.y - position.y;
	
	a = a*a*offset*offset;
	b = b*b;
	float softedge = radius*radius - (a+b);
	softedge = maxRadius - radius - (softedge + softedge);
	if(softedge < 0) softedge = 0;

	if((a + b) < radius*radius)
		{
			return tex * float4(1, 1 - softedge, 1 - softedge/2, 1);
		}
	return tex;
}

technique Bloom
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}