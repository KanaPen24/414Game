Shader "Custom/ON_Liquid"
{
    Properties
    {
		_WaveCenter("WaveCenter", Vector)=(.0, .0, .0, .0)
		_WaveParams("WaveParam", Vector)=(.1, 10.0, 10.0, .0)
		_LiquidColorForward("LiquidColorForward", Color)=(.5, .6, .9, 1.0)
		_LiquidColorBack("LiquidColorBack", Color)=(.6, .7, 1.0, 1.0)
		_BrokeTex("_BrokeTex", 2D) = "white"{}

		_Gravity("Gravity", Vector) = (.0, -1, .0, .0)
		_GravityScale("Gravity Scale", float) = 2
		_ScaleFactor("Scale Factor", float) = 5
		_Rate("Effect Rate", Range(0, 1)) = 0

		_TessFactor("Tess Factor",Vector) = (1,5,1,1)
    }
    SubShader
    {
        Tags { 
			"RenderType"="Opaque" 
			"RenderPipeline" = "UniversalPipeline"
		}
        LOD 100
		Blend One Zero
		ZWrite On
		ZTest LEqual

		Cull off

        Pass
        {
			Name "ForwardLit"
			Tags {"LightMode" = "UniversalForward"}
			
            HLSLPROGRAM
            #pragma vertex vert
			#pragma hull hs
			#pragma domain ds
			#pragma geometry geom
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			TEXTURE2D(_BrokeTex);
			SAMPLER(sampler_BrokeTex);

			CBUFFER_START(UnityPerMaterial)
			float4 _WaveCenter;			// 波の中心位置 xz:モデルの中心, y:波の基準高さ
			float4 _WaveParams;			// 波のパラメータ x:波振幅, y:波周期, z:波の移動速度
			float4 _LiquidColorForward;	// 流体モデル表カラー(流体側面カラー
			float4 _LiquidColorBack;	// 流体モデル裏カラー(流体表面カラー
			float4 _BrokeTex_ST;		// ヒビテクスチャ
			float4 _Gravity;			// 破裂後の重力
			float _GravityScale;		// 重量の大きさ
			float _ScaleFactor;			// 拡散の大きさ
			float _Rate;				// 拡散の割合
			float4 _TessFactor;			// テッセレーションの分割
			CBUFFER_END

			// マクロ定義
			#define WaveSize (_WaveParams.x) // 波振幅
			#define WaveCycleCoef (_WaveParams.y) // 波周期
			#define WaveOffsetCycleCoef (_WaveParams.z) // 波の移動速度

			// 頂点の構造
            struct Attributes
            {
			    float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float2 uv : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

			// 頂点からハル
            struct Varyings
            {
				float4 vertex : SV_POSITION;
				float3 posWS : TEXCOORD0;
				float3 normalWS : TEXCOORD1;	// 法線情報
				float3 viewDir : TEXCOORD2;		// 視線方向
				float fogCoord : TEXCOORD3;
				float2 uv : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
            };

			// ハルからドメインへ
			struct h2d_main
			{
				float3 pos:POS;
				float3 posWS : TEXCOORD0;
				float3 normalWS : TEXCOORD1;	// 法線情報
				float3 viewDir : TEXCOORD2;		// 視線方向
				float fogCoord : TEXCOORD3;
				float2 uv : TEXCOORD4;

			};

			struct h2d_const
			{
				float tess_factor[3] : SV_TessFactor;
				float InsideTessFactor : SV_InsideTessFactor;
			};

			// ドメインからジオメトリーへ
			struct d2g
			{
				float3 posWS : TEXCOORD0;
				float3 normalWS : TEXCOORD1;	// 法線情報
				float3 viewDir : TEXCOORD2;		// 視線方向
				float fogCoord : TEXCOORD3;
				float2 uv : TEXCOORD4;
			};

			// ジオメトリーからフラグメントへ
			struct g2f
			{
				float3 posWS : TEXCOORD0;
				float3 normalWS : TEXCOORD1;	// 法線情報
				float3 viewDir : TEXCOORD2;		// 視線方向
				float fogCoord : TEXCOORD3;
				float2 uv : TEXCOORD4;
				float4 vertex : SV_POSITION;
			};

			// 2次元ベクトルをシードとして0～1のランダム値を返す
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

			Varyings vert(Attributes input)
            {
				Varyings output;
				UNITY_SETUP_INSTANCE_ID(input);
				UNITY_TRANSFER_INSTANCE_ID(input, output);

				// 頂点情報取得
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
				output.posWS = vertexInput.positionWS;
				VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
				output.normalWS = NormalizeNormalPerPixel(normalInput.normalWS);
				output.viewDir = GetWorldSpaceViewDir(vertexInput.positionWS);

				output.vertex = vertexInput.positionCS;
				output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);

				output.uv = TRANSFORM_TEX(input.uv, _BrokeTex);
				return output;
            }

			h2d_const HSConst(InputPatch<Varyings, 3> i) {
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
			h2d_main hs(InputPatch<Varyings, 3> i, uint id:SV_OutputControlPointID) {
				h2d_main o = (h2d_main)0;
				o.pos = i[id].posWS;
				o.uv = i[id].uv;
				o.normalWS = i[id].normalWS;
				o.viewDir = i[id].viewDir;
				o.fogCoord = i[id].fogCoord;
				return o;
			}

			[domain("tri")]
			d2g ds(h2d_const hs_const_data, const OutputPatch<h2d_main, 3> i, float3 bary:SV_DomainLocation) {
				d2g o = (d2g)0;
				float3 pos = i[0].pos * bary.x + i[1].pos * bary.y + i[2].pos * bary.z;
				o.posWS = pos;
				float2 uv = i[0].uv * bary.x + i[1].uv * bary.y + i[2].uv * bary.z;
				o.uv = uv;
				float3 normal = i[0].normalWS * bary.x + i[1].normalWS * bary.y + i[2].normalWS * bary.z;
				o.normalWS = normal;
				float3 viewDir = i[0].viewDir * bary.x + i[1].viewDir * bary.y + i[2].viewDir * bary.z;
				o.viewDir = viewDir;
				float fog = i[0].fogCoord * bary.x + i[1].fogCoord * bary.y + i[2].fogCoord * bary.z;
				o.fogCoord = fog;
				return o;
			}

			[maxvertexcount(3)]
			void geom(triangle d2g input[3], inout TriangleStream<g2f> stream)
			{
				// ポリゴンの中心を取得
				float3 center = (input[0].posWS + input[1].posWS + input[2].posWS) / 3; // ポリゴンのセンター

				// 法線計算
				float3 vec1 = input[1].posWS - input[0].posWS;
				float3 vec2 = input[2].posWS - input[0].posWS;
				float3 normal = normalize(cross(vec1, vec2));

				float random = rand(center.xy);
				float3 random3 = random.xxx;

				float3 gravity = lerp(float3(.0, .0, .0), _Gravity.xyz, _Rate);

				[unroll]
				for (int i = 0; i < 3; i++)
				{
					d2g v = input[i];
					g2f o;
					// centerを基準に拡縮
					v.posWS.xyz = (v.posWS.xyz - center) * (1 - _Rate) + center;

					// centerを中心に乱数を使って回転
					v.posWS.xyz = center + rotate(v.posWS.xyz - center, center, _Rate * _ScaleFactor);

					// 法線ベクトルに沿って頂点移動
					v.posWS.xyz += normal * _Rate * _ScaleFactor * random3;

					// _Gravityに引っ張られて落ちていく
					v.posWS.xyz += gravity * _GravityScale;

					o.vertex = TransformObjectToHClip(v.posWS);
					o.uv = v.uv;
					o.posWS = v.posWS;
					o.normalWS = v.normalWS;
					o.viewDir = v.viewDir;
					o.fogCoord = v.fogCoord;

					stream.Append(o);
				}
				stream.RestartStrip();
			}

			half4 frag(g2f input) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(input);

				// 設定されたパラメータとxz座標から波の高さを算出
				float waveBaseY = _WaveCenter.y;
				float2 localXZ = _WaveCenter.xz - input.posWS.xz;
				float2 waveInput = localXZ * WaveCycleCoef;
				float waveInputOffset = _Time.y * WaveOffsetCycleCoef;
				waveInput += waveInputOffset;
				float clipPosY = waveBaseY + (sin(waveInput.x) + sin(waveInput.y)) * WaveSize;

				// 指定高さより上のピクセルを破棄
				clip(clipPosY - input.posWS.y);
				
				// 残ったピクセルに、法線情報を確認し表裏で色分けする
				// 裏面で表示される部分を水面のように
				half NdotV = dot(input.normalWS, input.viewDir);

				half4 color = lerp(_LiquidColorBack, _LiquidColorForward, step(0.0h, NdotV));
				
				// テクスチャからサンプリング
				float4 tex = SAMPLE_TEXTURE2D(_BrokeTex, sampler_BrokeTex, input.uv);
				color.rgb *= tex.rbg;

				color.rgb = MixFog(color.rgb, input.fogCoord);
				return color;
			}
            ENDHLSL
        }
    }
}
