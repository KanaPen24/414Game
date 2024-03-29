Shader "Custom/ON_Blur"
{
	Properties
	{
		[HideInInspector] _MainTex("Texture", 2D) = "white" {}
		_BaseColor("BaseColor", Color) = (1.0, 1.0, 1.0, 0)
		_ShadowColor("Shadow Color", Color) = (.5, .5, .5, 0)
		_LightColor("Light Color", Color) = (1.0, 1.0, 1.0, 0)
		_ShadeToony("ShadeToony", Range(.0, 1.0)) = 0.9
		_ShadeShift("ShadeShift", Range(-1.0, 1.0)) = 0

		_NoiseTex("NoiseTex", 2D) = "white"{}
		_Glossiness("Smoothness", Range(.0, 1.0)) = 0.5
		[HideInInspector] _TrailDir("TrailDir", Vector) = (.0, .0, .0, .0)
	}
	SubShader
	{
		Tags {
			"RenderType" = "Opaque"
			"RenderPipeline" = "UniversalPipeline"
		}
		LOD 100

		Pass
		{
			Name "ForwardLit"
			Tags { "LightMode" = "UniversalForward" }

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		// make fog work
		#pragma multi_compile_fog

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float fogFactor : TEXCOORD1;
			float4 vertex : SV_POSITION;
			float3 normal : NORMAL;
		};

		TEXTURE2D(_MainTex);
		SAMPLER(sampler_MainTex);
		TEXTURE2D(_NoiseTex);
		SAMPLER(sampler_NoiseTex);

		CBUFFER_START(UnityPerMaterial)
		float4 _MainTex_ST;
		float4 _BaseColor;
		half4 _ShadowColor;
		half4 _LightColor;
		half _ShadeToony;
		half _ShadeShift;
		half _Glossiness;
		half4 _TrailDir;
		CBUFFER_END


		v2f vert(appdata v)
		{
			v2f o;
			float weight = clamp(dot(v.normal, _TrailDir.xyz), 0, 1);
			float noise = tex2Dlod(sampler_NoiseTex, float4(1 - v.uv.x, v.uv.y, 0, 0)).r;
			float4 trail = _TrailDir * weight * noise;
			v.vertex.xyz = float3(v.vertex.x + trail.x, v.vertex.y + trail.y, v.vertex.z + trail.z);
			o.vertex = TransformObjectToHClip(v.vertex.xyz);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.fogFactor = ComputeFogFactor(o.vertex.z);
			o.normal = TransformObjectToWorldNormal(v.normal);
			return o;
		}

		float4 frag(v2f i) : SV_Target
		{
			// sample the texture
			float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
			col = _BaseColor;
		
			//diff
			Light light = GetMainLight();
			float t = dot(normalize(i.normal), normalize(light.direction));
			half thresholdL = (_ShadeShift - (1 - _ShadeToony)) / 2 + 0.5;
			half thresholdH = (_ShadeShift + (1 - _ShadeToony)) / 2 + 0.5;
			col.rgb *= lerp(_ShadowColor.rgb, _LightColor.rgb, smoothstep(thresholdL, thresholdH, t));
			
			// apply fog
			col.rgb = MixFog(col.rgb, i.fogFactor);
			return col;
		}
		ENDHLSL
		}
	}
}