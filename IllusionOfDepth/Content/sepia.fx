sampler TextureSampler : register(s0);

Texture2D myTexture;

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{
	float4 color = myTexture.Sample(TextureSampler, texCoord.xy);

	float red = (color[0] * .393) + (color[1] * .796) + (color[2] * .189);
	if (red > 255)
	{
		red = 255;
	}

	float green = (color[0] * .349) + (color[1] * .686) + (color[2] * .168);
	if (green > 255)
	{
		green = 255;
	}

	float blue = (color[0] * .272) + (color[1] * .534) + (color[2] * .131);
	if (blue > 255)
	{
		blue = 255;
	}

	color[0] = red;
	color[1] = green;
	color[2] = blue;

	return color;
}

technique Technique1
{
    pass Pass1
    {
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
    }
}