/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

 //////////////////////
////   GLOBALS
//////////////////////
Texture2D shaderTexture;
SamplerState SampleType;

//////////////////////
////   TYPEDEFS
//////////////////////
struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
};

//////////////////////
////   Pixel Shader
/////////////////////
float4 Entry(PixelInputType input) : SV_TARGET
{
	float4 textureColor;

	// Sample the pixel color from the texture using the sampler at this texture coordinate location.
	textureColor = shaderTexture.Sample(SampleType, input.tex);
	clip(textureColor.a < 0.01f ? -1 : 1);
	//textureColor.a = 1;
	return textureColor;
}
