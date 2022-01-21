///////////////////////
////   GLOBALS
///////////////////////
cbuffer MatrixBuffer : register(b0)
{
	matrix viewMatrix;
	matrix projectionMatrix;
};

cbuffer CameraPosition : register(b1)
{
	float3 cameraPosition;
};

cbuffer ObjectBuffer : register(b2)
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
	float3 normal : NORMAL;
	float3 viewDirection : TEXCOORD1;
};

/////////////////////////////////////
/////   Vertex Shader
/////////////////////////////////////
PixelInputType EntryPoint3D(VertexInputType input)
{
	PixelInputType output;
	float4 worldPosition;

	// Change the position vector to be 4 units for proper matrix calculations.
	input.position.w = 1.0f;


	output.position = mul(input.position, objectmatrix);

	worldPosition = output.position;
	output.position = mul(output.position, viewMatrix);
	output.position = mul(output.position, projectionMatrix);
	// Store the texture coordinates for the pixel shader.
	output.tex = input.tex;
	// Calculate the normal vector against the object matrix only.
	output.normal = mul(input.normal, (float3x3)objectmatrix);
	// Normalize the normal vector.
	output.normal = normalize(input.normal);



	// Determine the viewing direction based on the position of the camera and the position of the vertex in the world.
	output.viewDirection = cameraPosition.xyz - worldPosition.xyz;

	// Normalize the viewing direction vector.
	output.viewDirection = normalize(output.viewDirection);

	return output;
}