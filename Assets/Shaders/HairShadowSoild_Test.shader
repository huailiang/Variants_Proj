Shader "Custom/HairShadowSoild_Test"
{
	SubShader
	{
		Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

		HLSLINCLUDE
		#include "Assets/Packages/universal/ShaderLibrary/Core.hlsl"
		ENDHLSL

		Pass
		{
			Name "FaceDepthOnly"
			Tags { "LightMode" = "UniversalForward" }

			ColorMask 0
			ZTest LEqual
			ZWrite On

			HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			struct a2v
			{
				float4 positionOS: POSITION;
			};

			struct v2f
			{
				float4 positionCS: SV_POSITION;
			};

			v2f vert(a2v v)
			{
				v2f o;

				//VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
				//v.positionCS = positionInputs.positionCS;
				// Or this :
				o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				return half4(0, 0, 0, 1);
			}
			ENDHLSL
		}

		Pass
		{
			Name "HairSimpleColor"
			Tags { "LightMode" = "UniversalForward" }

			Cull Off
			ZTest LEqual
			ZWrite Off

			HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			struct a2v
			{
				float4 positionOS: POSITION;
			};

			struct v2f
			{
				float4 positionCS: SV_POSITION;
			};


			v2f vert(a2v v)
			{
				v2f o;

				VertexPositionInputs positionInputs = GetVertexPositionInputs(v.positionOS.xyz);
				o.positionCS = positionInputs.positionCS;
				// Or this :
				//o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				return float4(1, 1, 1, 1);
			}
			ENDHLSL

		}

	}
}