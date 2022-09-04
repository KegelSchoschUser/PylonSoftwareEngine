///////////////////////
////   GLOBALS
///////////////////////
cbuffer ScreenSizeBuffer : register(b0)
{
	float screenWidth;
	float screenHeight;
	float Multiplier;
	float padding;
};

cbuffer MatrixBuffer : register(b1)
{
	matrix viewMatrix;
	matrix projectionMatrix;
};

cbuffer CameraPosition : register(b2)
{
	float3 cameraPosition;
};

cbuffer ObjectBuffer : register(b3)
{
	matrix objectmatrix;
};

//////////////////////
////   TYPES
//////////////////////
struct VertexInputType
{
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
};

struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
	float2 texCoord1 : TEXCOORD1;
	float2 texCoord2 : TEXCOORD2;
	float2 texCoord3 : TEXCOORD3;
	float2 texCoord4 : TEXCOORD4;
	float2 texCoord5 : TEXCOORD5;
	float2 texCoord6 : TEXCOORD6;
	float2 texCoord7 : TEXCOORD7;
	float2 texCoord8 : TEXCOORD8;
	float2 texCoord9 : TEXCOORD9;
};

/////////////////////////////////////
/////   Vertex Shader
/////////////////////////////////////
PixelInputType Entry(VertexInputType input)
{
	PixelInputType output;
	float4 worldPosition;
	float texelSize;

	// Change the position vector to be 4 units for proper matrix calculations.
	input.position.w = 1.0f;


	//output.position = mul(input.position, worldMatrix);
	output.position = mul(input.position, objectmatrix);
	output.position = mul(output.position, viewMatrix);
	output.position = mul(output.position, projectionMatrix);

	output.position.y *= -1;
	output.position.x = output.position.x - 1;
	output.position.y = output.position.y + 1;

	// Store the texture coordinates for the pixel shader.
	output.tex = input.tex;

	// Determine the floating point size of a texel for a screen with this specific width.
	texelSize = 1.0f / screenWidth * Multiplier;

	// Create UV coordinates for the pixel and its four horizontal neighbors on either side.
	output.texCoord1 = input.tex + float2(texelSize * -4.0f, 0.0f);
	output.texCoord2 = input.tex + float2(texelSize * -3.0f, 0.0f);
	output.texCoord3 = input.tex + float2(texelSize * -2.0f, 0.0f);
	output.texCoord4 = input.tex + float2(texelSize * -1.0f, 0.0f);
	output.texCoord5 = input.tex + float2(texelSize * 0.0f, 0.0f);
	output.texCoord6 = input.tex + float2(texelSize * 1.0f, 0.0f);
	output.texCoord7 = input.tex + float2(texelSize * 2.0f, 0.0f);
	output.texCoord8 = input.tex + float2(texelSize * 3.0f, 0.0f);
	output.texCoord9 = input.tex + float2(texelSize * 4.0f, 0.0f);

	return output;
}