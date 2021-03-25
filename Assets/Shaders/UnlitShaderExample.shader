// Example Shader for Universal RP
// Written by @Cyanilux
// https://cyangamedev.wordpress.com/urp-shader-code/
Shader "Custom/UnlitShaderExample" {
	Properties{
		_BaseMap("Example Texture", 2D) = "white" {}
		_BaseColor("Example Colour", Color) = (0, 0.66, 0.73, 1)
	}
	SubShader{
		Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

		HLSLINCLUDE
		#include "Assets/Packages/universal/ShaderLibrary/Core.hlsl"

		CBUFFER_START(UnityPerMaterial)
		float4 _BaseMap_ST;
		float4 _BaseColor;
		CBUFFER_END

		ENDHLSL

		Pass {
			Name "Example"
			Tags { "LightMode" = "UniversalForward" }

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct a2v {
				float4 positionOS   : POSITION;
				float2 uv           : TEXCOORD0;
				float4 color        : COLOR;
			};

			struct v2f {
				float4 positionCS  : SV_POSITION;
				float2 uv           : TEXCOORD0;
				float4 color        : COLOR;
			};

			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_BaseMap);

			v2f vert(a2v v) {
				v2f o;

				//VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
				//o.positionCS = positionInputs.positionCS;
				// Or this :
				o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
				o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
				o.color = v.color;
				return o;
			}

			half4 frag(v2f i) : SV_Target {
				half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);

				return baseMap * _BaseColor * i.color;
			}
			ENDHLSL
		}
	}
}