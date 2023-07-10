Shader "Custom/ON_TestGeometry"
{
	Properties
	{
		_MainColor("Main Color", Color) = (1, 1, 1, 1)

		_ScaleFactor("Scale Factor", float) = 0.5
		_Rate("Effect Rate", Range(0, 1)) = 0
	}
		SubShader
	{
		Tags {
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
			"RenderPipeline" = "UniversalPipeline"
		}
		Blend SrcAlpha OneMinusSrcAlpha
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

		struct VertexOutPut
		{
			float3 positionWS : TEXCOORD0;
		};

		struct g2f
		{
			float4 positionCS : SV_POSITION;
			float3 positionWS : TEXCOORD0;
		};


		CBUFFER_START(UnityPerMaterial)
		half4 _MainColor;
		float _ScaleFactor;
		float _Rate;
		CBUFFER_END

		// 2次元ベクトルをシードとして0～1のランダム値を返す
		float rand(float2 co)
		{
			return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43756.5453);
		}

		VertexOutPut vert(appdata v)
		{
			VertexOutPut o = (VertexOutPut)0;

			VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
			o.positionWS = vertexInput.positionWS;

			return o;
		}

		[maxvertexcount(3)]
		void geom(triangle VertexOutPut input[3], inout TriangleStream<g2f> stream)
		{
			float3 center = (input[0].positionWS + input[1].positionWS + input[2].positionWS) / 3; // ポリゴンのセンター

			// 法線計算
			float3 vec1 = input[1].positionWS - input[0].positionWS;
			float3 vec2 = input[2].positionWS - input[0].positionWS;
			float3 normal = normalize(cross(vec1, vec2));

			float random = rand(center.xy);
			float3 random3 = random.xxx;
			[unroll]
			for (int i = 0; i < 3; i++)
			{
				VertexOutPut v = input[i];
				g2f o;
				
				// 法線ベクトルに沿って頂点移動
				v.positionWS.xyz += normal * _Rate * _ScaleFactor * random3;

				o.positionWS = v.positionWS;
				o.positionCS = TransformObjectToHClip(v.positionWS);

				stream.Append(o);
			}
			stream.RestartStrip();
		}

		float4 frag(g2f i) : SV_Target
		{
			float4 col = _MainColor;
			col.a = lerp(_MainColor.a, 0, _Rate);

			return col;
		}
		ENDHLSL
		}
	}
}