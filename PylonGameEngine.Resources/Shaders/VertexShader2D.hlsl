///////////////////////
////   GLOBALS
///////////////////////
cbuffer MatrixBuffer : register(b0)
{
	matrix viewMatrix;
	matrix projectionMatrix;
};

cbuffer ObjectBuffer : register(b1)
{
	matrix objectmatrix;
};

//////////////////////
////   TYPES
//////////////////////

struct Vertex2DInputType
{
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
};

struct Pixel2DInputType
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
};

/////////////////////////////////////
/////   Vertex Shader
/////////////////////////////////////
Pixel2DInputType EntryPoint2D(Vertex2DInputType input)
{
	Pixel2DInputType output;

	input.position.w = 1.0f;

	output.position = mul(input.position, objectmatrix);
	output.position = mul(output.position, viewMatrix);
	output.position = mul(output.position, projectionMatrix);
	output.position.y *= -1;
	output.position.x = output.position.x - 1;
	output.position.y = output.position.y + 1;

	output.tex = input.tex;
	return output;
}