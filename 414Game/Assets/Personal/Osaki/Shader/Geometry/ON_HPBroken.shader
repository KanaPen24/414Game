Shader "Custom/ON_HPBroken"
{
	Properties
	{
		_MainColor("Main Color", Color) = (1, 1, 1, 1)

		_Gravity("Gravity", Vector) = (.0, -1, .0, .0)
		_GravityScale("Gravity Scale", float) = 1
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
		float4 _Gravity;
		float _GravityScale;
		float _ScaleFactor;
		float _Rate;
		CBUFFER_END

			// 2次元ベクトルをシードとして0～1のランダム値を返す
			float rand(float2 co)
			{
				return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43756.5453);
			}

		// 座標を軸を基準に回転させる
		float3 rotate(float3 pos, float3 axis, float angle)
		{
			float3 a = normalize(axis);
			float s = sin(angle);
			float c = cos(angle);
			float r = 1.0 - c;
			float3x3 m = float3x3(
				a.x * a.x * r + c, a.y * a.x * r + a.z * s, a.z * a.x * r - a.y * s,
				a.x * a.y * r - a.z * s, a.y * a.y * r + c, a.z * a.y * r + a.x * s,
				a.x * a.z * r + a.y * s, a.y * a.z * r - a.x * s, a.z * a.z * r + c
				);

			return  mul(m, pos);
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
			// ポリゴンの中心を取得
			float3 center = (input[0].positionWS + input[1].positionWS + input[2].positionWS) / 3; // ポリゴンのセンター

			// 法線計算
			float3 vec1 = input[1].positionWS - input[0].positionWS;
			float3 vec2 = input[2].positionWS - input[0].positionWS;
			float3 normal = normalize(cross(vec1, vec2));

			float random = rand(center.xy);
			float3 random3 = random.xxx;

			float3 gravity = lerp(float3(.0, .0, .0), _Gravity.xyz, _Rate);

			[unroll]
			for (int i = 0; i < 3; i++)
			{
				VertexOutPut v = input[i];
				g2f o;
				// centerを基準に拡縮
				v.positionWS.xyz = (v.positionWS.xyz - center) * (1 - _Rate) + center;

				// centerを中心に乱数を使って回転
				v.positionWS.xyz = center + rotate(v.positionWS.xyz - center, center, _Rate * _ScaleFactor);

				// 法線ベクトルに沿って頂点移動
				v.positionWS.xyz += normal * _Rate * _ScaleFactor * random3;

				// _Gravityに引っ張られて落ちていく
				v.positionWS.xyz += gravity * _GravityScale;

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
