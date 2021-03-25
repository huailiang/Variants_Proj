Shader "DC/MultiCompile"
{
	Properties
	{
		[KeywordEnum(R,G,B)] _CL("ColorSelect", Float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#pragma multi_compile _CL_R _CL_G _CL_B
				//#pragma multi_compile _CL_T
				//使用 __ 减少一个编译选项，编译选项最多256个
				#pragma multi_compile __ DB_ON

				
				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				float4 _MainTex_ST;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{

					#if DB_ON
						return fixed4(1,1,0,1);
					#elif _CL_R
						return fixed4(1,0,0,1);
					#elif _CL_G
						return fixed4(0,1,0,1);
					#elif _CL_B
						return fixed4(0,0,1,1);
					#else
						return fixed4(1,1,1,1);
					#endif
				}
				ENDCG
			}
		}
}

