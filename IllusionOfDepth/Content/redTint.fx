sampler TextureSampler : register(s0);

Texture2D myTexture;

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{
	float4 color = myTexture.Sample(TextureSampler, texCoord.xy);

	float red = (color.r + 0.5);
	if (red > 255)
	{
		red = 255;
	}

	color[0] = red;

	return color;
}

technique Technique1
{
    pass Pass1
    {
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
    }
}