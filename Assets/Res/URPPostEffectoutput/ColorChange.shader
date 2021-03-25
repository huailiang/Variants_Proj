Shader "Unlit/ColorChange"
{
    Properties
    {
        _MainTex("ScreenTexture", 2D) = "white" {}
        _Contrast("Contrast", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
             HLSLPROGRAM
            #include "Assets/Packages/universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv:TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv:TEXCOORD0;
            };

            half _Contrast;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            Varyings vert (Attributes v)
            {
                Varyings o = (Varyings)0;

                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = vertexInput.positionCS;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                half3 avgColor = half3(0.5, 0.5, 0.5);
                //对比度
                half4 finalCol = 1;
                finalCol.rgb = lerp(avgColor, col, _Contrast);
                return finalCol;
            }
            ENDHLSL
        }
    }
}
