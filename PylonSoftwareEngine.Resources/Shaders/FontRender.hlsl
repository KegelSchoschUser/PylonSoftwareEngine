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

cbuffer CharacterColorBuffer : register(b0)
{
	float4 CharacterColor;
};
//////////////////////
////   TYPEDEFS
//////////////////////
struct Pixel2DInputType
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
};

//////////////////////
////   Pixel Shader
/////////////////////
float4 FontRenderer(Pixel2DInputType input) : SV_TARGET
{
	float4 OutputColor;

	// Sample the pixel color from the texture using the sampler at this texture coordinate location.
	OutputColor = shaderTexture.Sample(SampleType, input.tex);
	OutputColor = OutputColor * CharacterColor;
	clip(OutputColor.a < 0.01f ? -1 : 1);
	//textureColor.a = 1;
	return OutputColor;
}
