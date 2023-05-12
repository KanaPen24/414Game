Shader "Hidden/ON_BraunTube"
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

			// 2�����x�N�g�����V�[�h�Ƃ���0�`1�̃����_���l��Ԃ�
			float rand(float2 co)
			{
				return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43756.5453);
			}

			// ��ʂ��o�������Ă�悤�ɘc�܂���
			float2 distort(float2 uv, float rate)
			{
				uv -= 0.5;
				uv /= 1 - length(uv) * rate;
				uv += 0.5;
				return uv;
			}

			// 3x3�̃K�E�V�A���t�B���^��������
			half4 sample_gaussian(float2 uv, float2 dx, float2 dy)
			{
				half4 col = 0;
				col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - dx - dy) * 1 / 16;
				col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - dx) * 2 / 16;
				col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - dx + dy) * 1 / 16;
				col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - dy) * 2 / 16;
				col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv) * 4 / 16;
				col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + dy) * 2 / 16;
				col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + dx - dy) * 1 / 16;
				col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + dx) * 2 / 16;
				col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + dx + dy) * 1 / 16;
				return col;
			}

			// �C�[�W���O
			float ease_in_out_cubic(const float x)
			{
				return x < .5 ? 4 * x * x * x : 1 - pow(-2 * x + 2, 3) / 2;
			}

			// 1��f�̏㉺�[���Â��Ȃ錻�ۂ��Č�
			float crt_ease(const float x, const float base, const float offset)
			{
				float tmp = fmod(x + offset, 1);
				float xx = 1 - abs(tmp * 2 - 1);
				float ease = ease_in_out_cubic(xx);
				return ease * base + base * 0.8;
			}

			half4 frag(Varyings IN) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				float2 uv = IN.uv;
				
				// uv����ʂ��o�������Ă���悤�ɘc�܂���
				uv = distort(uv, .2);

				// uv���͈͓��łȂ���΍����h��Ԃ�
				if (uv.x < 0 || 1 < uv.x || uv.y < 0 || 1 < uv.y)
				{
					return half4(0, 0, 0, 1);
				}

				// �c��ɂɂɓ����F�̉�f������
				const float floor_x = fmod(IN.uv.x * _ScreenParams.x / 3, 1);
				const float isR = floor_x <= 0.3;
				const float isG = 0.3 < floor_x && floor_x <= 0.6;
				const float isB = 0.6 < floor_x;

				// �ׂ̃s�N�Z���܂ł�uv���W�ł̍�
				const float2 dx = float2(1 / _ScreenParams.x, 0);
				const float2 dy = float2(0, 1 / _ScreenParams.y);

				// RGB���Ƃ�UV�����炷
				uv += isR * -1 * dy;
				uv += isG * 0 * dy;
				uv += isB * 1 * dy;

				// �K�E�V�A���t�B���^�ɂ���āA���E���ڂ���
				half4 col = sample_gaussian(uv, dx, dy);

				// �c������N�s�N�Z�����Ƃɕ������Ē[���Â����鏈��
				const float floor_y = fmod(uv.y * _ScreenParams.y / 6, 1);
				const float ease_r = crt_ease(floor_y, col.r, rand(uv)* 0.1);
				const float ease_g = crt_ease(floor_y, col.g, rand(uv)* 0.1);
				const float ease_b = crt_ease(floor_y, col.b, rand(uv)* 0.1);

				// ���݂̃s�N�Z���ɂ����RGB�̂�����̐F������\������
				col.r = isR * ease_r;
				col.g = isG * ease_g;
				col.b = isB * ease_b;

				return col;
			}
			ENDHLSL
		}
	}
}
