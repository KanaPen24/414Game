// テセレーション
Shader "Custom/ON_Tessellesion"
{
	Properties
	{
		_MainColor("Main Color", Color) = (1, 1, 1, 1)

		_Gravity("Gravity", Vector) = (.0, -1, .0, .0)
		_GravityScale("Gravity Scale", float) = 1
		_ScaleFactor("Scale Factor", float) = 0.5
		_Rate("Effect Rate", Range(0, 1)) = 0

		_TessFactor("Tess Factor",Vector) = (2,2,2,2)
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
			#pragma hull hs
			#pragma domain ds
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

		struct h2d_main
		{
			float3 pos:POS;
		};
		
		struct h2d_const
		{
			float tess_factor[3] : SV_TessFactor;
			float InsideTessFactor : SV_InsideTessFactor;
		};

		struct d2g
		{
			float3 positionWS : TEXCOORD0;
		};

		struct g2f
		{
			float4 positionCS : SV_POSITION;
		};


		CBUFFER_START(UnityPerMaterial)
		half4 _MainColor;
		float4 _Gravity;
		float _GravityScale;
		float _ScaleFactor;
		float _Rate;
		float4 _TessFactor;
		CBUFFER_END

		// 2次元ベクトルをシードとして0〜1のランダム値を返す
		float rand(float2 co)
		{
			return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43756.5453);
		}

		// 軸を基準に座標を回転させる
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

		h2d_const HSConst(InputPatch<VertexOutPut, 3> i) {
			h2d_const o = (h2d_const)0;
			o.tess_factor[0] = _TessFactor.x;
			o.tess_factor[1] = _TessFactor.y;
			o.tess_factor[2] = _TessFactor.z;
			o.InsideTessFactor = _TessFactor.w;
			return o;
		}

		[domain("tri")]
		[partitioning("integer")]
		[outputtopology("triangle_cw")]
		[outputcontrolpoints(3)]
		[patchconstantfunc("HSConst")]
		h2d_main hs(InputPatch<VertexOutPut, 3> i, uint id:SV_OutputControlPointID) {
			h2d_main o = (h2d_main)0;
			o.pos = i[id].positionWS;
			return o;
		}

		[domain("tri")]
		d2g ds(h2d_const hs_const_data, const OutputPatch<h2d_main, 3> i, float3 bary:SV_DomainLocation) {
			d2g o = (d2g)0;
			float3 pos = i[0].pos * bary.x + i[1].pos * bary.y + i[2].pos * bary.z;
			o.positionWS = pos;
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
