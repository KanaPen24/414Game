Shader "Custom/ON_Lit_Scroll"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
		_MaskTex("Mask", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_Offset("Offset", Vector) = (.0, .0, .0, .0)

			// Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
			[HideInInspector] PixelSnap("Pixel snap", Float) = 0
			[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
			[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
			[HideInInspector] _AlphaTex("External Alpha", 2D) = "white" {}
			[HideInInspector] _EnableExternalAlpha("Enable External Alpha", Float) = 0
	}

	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
	
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off
	
		Pass
		{
			Tags { "LightMode" = "Universal2D" }
	
			HLSLPROGRAM
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
			#if defined(DEBUG_DISPLAY)
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/InputData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"
			#endif
	
			#pragma vertex UnlitVertex
			#pragma fragment UnlitFragment
	
			#pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
			#pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
			#pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
			#pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __
			#pragma multi_compile _ DEBUG_DISPLAY
	
			struct Attributes
			{
				float3 positionOS   : POSITION;
				float4 color        : COLOR;
				float2 uv           : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
	
			struct Varyings
			{
				float4  positionCS  : SV_POSITION;
				half4   color       : COLOR;
				float2  uv          : TEXCOORD0;
				half2   lightingUV  : TEXCOORD1;
				#if defined(DEBUG_DISPLAY)
				float3  positionWS  : TEXCOORD2;
				#endif
				UNITY_VERTEX_OUTPUT_STEREO
			};
	
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
			TEXTURE2D(_MaskTex);
			SAMPLER(sampler_MaskTex);
			half4 _MainTex_ST;
			float4 _Offset; 

			#if USE_SHAPE_LIGHT_TYPE_0
			SHAPE_LIGHT(0)
			#endif
			
			#if USE_SHAPE_LIGHT_TYPE_1
			SHAPE_LIGHT(1)
			#endif
			
			#if USE_SHAPE_LIGHT_TYPE_2
			SHAPE_LIGHT(2)
			#endif
			
			#if USE_SHAPE_LIGHT_TYPE_3
			SHAPE_LIGHT(3)
			#endif
	
			Varyings UnlitVertex(Attributes v)
			{
				Varyings o = (Varyings)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
	
				o.positionCS = TransformObjectToHClip(v.positionOS);
				#if defined(DEBUG_DISPLAY)
				o.positionWS = TransformObjectToWorld(v.positionOS);
				#endif

				o.uv = TRANSFORM_TEX(v.uv + _Offset.xy, _MainTex);
				o.lightingUV = half2(ComputeScreenPos(o.positionCS / o.positionCS.w).xy);
				o.color = v.color;
				return o;
			}
	
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"
			
			half4 UnlitFragment(Varyings i) : SV_Target
			{
				float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
	
				SurfaceData2D surfaceData;
				InputData2D inputData;
				half4 debugColor = 0;
	
				InitializeSurfaceData(mainTex.rgb, mainTex.a, mask, surfaceData);
				InitializeInputData(i.uv, i.lightingUV, inputData);
	
				#if defined(DEBUG_DISPLAY)
				SETUP_DEBUG_DATA_2D(inputData, i.positionWS);
				if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
				{
					return debugColor;
				}
				#endif
	
				return CombinedShapeLightShared(surfaceData, inputData);
			}
			ENDHLSL
		}
	}
}