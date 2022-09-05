//////////////////////
////   GLOBALS
//////////////////////
cbuffer ShaderInput
{
	float4 Color;
};

//////////////////////
////   TYPES
//////////////////////
struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
};

//////////////////////
////   Pixel Shader
/////////////////////
float4 Entry(PixelInputType input) : SV_TARGET
{
	return Color;
}
