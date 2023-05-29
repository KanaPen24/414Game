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
			float4 _WaveCenter;			// �g�̒��S�ʒu xz:���f���̒��S, y:�g�̊����
			float4 _WaveParams;			// �g�̃p�����[�^ x:�g�U��, y:�g����, z:�g�̈ړ����x
			float4 _LiquidColorForward;	// ���̃��f���\�J���[(���̑��ʃJ���[
			float4 _LiquidColorBack;	// ���̃��f�����J���[(���̕\�ʃJ���[
			float4 _BrokeTex_ST;		// �q�r�e�N�X�`��
			CBUFFER_END

			// �}�N����`
			#define WaveSize (_WaveParams.x) // �g�U��
			#define WaveCycleCoef (_WaveParams.y) // �g����
			#define WaveOffsetCycleCoef (_WaveParams.z) // �g�̈ړ����x

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
				float3 normalWS : TEXCOORD1;	// �@�����
				float3 viewDir : TEXCOORD2;		// ��������
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

				// ���_���擾
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

				// �ݒ肳�ꂽ�p�����[�^��xz���W����g�̍������Z�o
				float waveBaseY = _WaveCenter.y;
				float2 localXZ = _WaveCenter.xz - input.posWS.xz;
				float2 waveInput = localXZ * WaveCycleCoef;
				float waveInputOffset = _Time.y * WaveOffsetCycleCoef;
				waveInput += waveInputOffset;
				float clipPosY = waveBaseY + (sin(waveInput.x) + sin(waveInput.y)) * WaveSize;

				// �w�荂������̃s�N�Z����j��
				clip(clipPosY - input.posWS.y);
				
				// �c�����s�N�Z���ɁA�@�������m�F���\���ŐF��������
				// ���ʂŕ\������镔���𐅖ʂ̂悤��
				half NdotV = dot(input.normalWS, input.viewDir);

				half4 color = lerp(_LiquidColorBack, _LiquidColorForward, step(0.0h, NdotV));
				
				// �e�N�X�`������T���v�����O
				float4 tex = SAMPLE_TEXTURE2D(_BrokeTex, sampler_BrokeTex, input.uv);
				color.rgb *= tex.rbg;

				color.rgb = MixFog(color.rgb, input.fogCoord);
				return color;
			}
            ENDHLSL
        }
    }
}
