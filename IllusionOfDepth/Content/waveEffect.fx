sampler TextureSampler : register(s0);

Texture2D myTexture;
float time;

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{
	texCoord.x = texCoord.x + (sin(texCoord.y*80+time*6)*0.03);

	float4 color = myTexture.Sample(TextureSampler, texCoord.xy);

	return color;
}

technique
{
	pass P0
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}