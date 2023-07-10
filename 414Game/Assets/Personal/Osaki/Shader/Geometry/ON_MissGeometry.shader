Shader "Custom/ON_MissGeometry"
{
	Properties
	{
		_MainColor("Near Color", Color) = (1, 1, 1, 1)

		_ScaleFactor("Scale Factor", float) = 0.5
	}
		SubShader
	{
		Tags {
			"RenderType" = "Opaque"
			"RenderPipeline" = "UniversalPipeline"
		}
		LOD 100
		Cull Off

		Pass
		{
			Name "ForwardLit"
			Tags { "LightMode" = "UniversalForward" }

			HLSLPROGRAM
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag
		// make fog work
		#pragma multi_compile_fog

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		struct appdata
		{
			float4 positionOS : POSITION;
			float3 normal : NORMAL;
		};

		struct g2f
		{
			float4 positionCS : SV_POSITION;
		};


		CBUFFER_START(UnityPerMaterial)
		half4 _MainColor;
		float _ScaleFactor;
		CBUFFER_END

		appdata vert(appdata v)
		{
			return v;
		}

		// ñ@ê¸èÓïÒÇ©ÇÁí∏ì_ÇïœâªÇ≥ÇπÇÈ
		[maxvertexcount(3)]
		void geom(triangle appdata input[3], inout TriangleStream<g2f> stream)
		{
			float3 vec1 = input[1].positionOS.xyz - input[0].positionOS.xyz;
			float3 vec2 = input[2].positionOS.xyz - input[0].positionOS.xyz;
			float3 normal = normalize(cross(vec1, vec2));
			[unroll]
			for (int i = 0; i < 3; i++)
			{
				appdata v = input[i];
				g2f o;
				
				v.positionOS.xyz += normal * (_SinTime.w * 0.5 + 0.5) * _ScaleFactor;

				o.positionCS = TransformObjectToHClip(v.positionOS.xyz);

				stream.Append(o);
			}
			stream.RestartStrip();
		}

		float4 frag(g2f i) : SV_Target
		{
			float4 col = _MainColor;

			return col;
		}
		ENDHLSL
	}
	}
}