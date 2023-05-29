Shader "Custom/ON_Liquid"
{
    Properties
    {
		_WaveCenter("WaveCenter", Vector)=(.0, .0, .0, .0)
		_WaveParams("WaveParam", Vector)=(.1, 10.0, 10.0, .0)
		_LiquidColorForward("LiquidColorForward", Color)=(.5, .6, .9, 1.0)
		_LiquidColorBack("LiquidColorBack", Color)=(.6, .7, 1.0, 1.0)
		_BrokeTex("_BrokeTex", 2D) = "white"{}
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
			CBUFFER_END

			// マクロ定義
			#define WaveSize (_WaveParams.x) // 波振幅
			#define WaveCycleCoef (_WaveParams.y) // 波周期
			#define WaveOffsetCycleCoef (_WaveParams.z) // 波の移動速度

            struct Attributes
            {
			    float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float2 uv : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

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

			half4 frag(Varyings input) : SV_TARGET
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
