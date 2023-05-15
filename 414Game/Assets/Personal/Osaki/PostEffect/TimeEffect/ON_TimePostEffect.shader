Shader "Hidden/ON_TimePostEffect"
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
			float _Rate;

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

			half4 frag(Varyings IN) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
				half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

				// âÊñ íÜêSÇ©ÇÁê≥â~Çï`Ç≠
				float radius = lerp(.0f, 1.1f, _Rate);
				float aspect = _ScreenParams.x / _ScreenParams.y;
				float2 uv = IN.uv;
				uv.x *= aspect;
				float r = distance(uv, float2(.5f * aspect, .5f));
				float range = smoothstep(radius, radius + .0002f,r);

				// â~ÇÃíÜÇÃÇ›îíçïÇ…
				float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
				col.rgb = lerp(gray, col.rgb, range);

				return col;
			}
			ENDHLSL
		}
	}
}
