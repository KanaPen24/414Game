#ifdef COMPUTE_HELPERS_INCLUDED
#define COMPUTE_HELPERS_INCLUDED

float3 GetNormalForomTriangle(float3 a, float3 b, float3 c)
{
	return normalize(cross(b - a, c - a));
}

float3 GetTriangleCenter(float3 a, float3 b, float3 c)
{
	return (a + b + c) / 3;
}

float2 GetTriangleCenter(float2 a, float2 b, float2 c)
{
	return (a + b + c) / 3;
}

#endif