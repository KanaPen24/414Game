Shader "Hidden/ON_PlayerBlur"
{
	SubShader
	{
		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
			TEXTURE2D(_BlurTex1);
			SAMPLER(sampler_BlurTex1);
			TEXTURE2D(_BlurTex2);
			SAMPLER(sampler_BlurTex2);
			TEXTURE2D(_BlurTex3);
			SAMPLER(sampler_BlurTex3);


			struct Attributes
			{
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionHCS : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			Varyings vert(Attributes IN)
			{
				Varyings OUT;
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
				OUT.uv = IN.uv;
				return OUT;
			}

			half4 sample_blur(float2 uv)
			{
				half4 col = 0;
				col += SAMPLE_TEXTURE2D(_BlurTex1, sampler_BlurTex1, uv) / 3;
				col += SAMPLE_TEXTURE2D(_BlurTex2, sampler_BlurTex2, uv) / 3;
				col += SAMPLE_TEXTURE2D(_BlurTex3, sampler_BlurTex3, uv) / 3;
				return col;
			}

			half4 frag(Varyings IN) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
				
				half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

				// ブラーテクスチャを加え画をぼかす
				col += sample_blur(IN.uv);
				col /= 2;

				return col;
			}
			ENDHLSL
		}
	}
}
