Shader "Hsinpa/HealthBarShader"
{

    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _MainColor ("Display Color", Color) = (1, 1, 1, 1)
        _BackgroundColor ("Background Color", Color) = (0, 0, 0, 1)
        _ResidualColor ("Residual Color", Color) = (1, 0, 0, 1)

        _Residual ("Residual", Range(0,1)) = 1
        _Health ("Health", Range(0,1)) = 0.8
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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


            sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform float4 _MainColor;
            uniform float4 _BackgroundColor;
            uniform float4 _ResidualColor;
            uniform float _Residual;
            uniform float _Health;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv);
                float gradient = i.uv.y;
                //Revert uv
                float uv = 1 - i.uv.x; 
                float uvHealth = uv - _Health;
                float uvResidual= uv - _Residual;

                fixed4 col = _BackgroundColor;

                if (uvResidual < 0) {
                    col = _ResidualColor;
                }

                if (uvHealth < 0) {
                    col = _MainColor;
                }

                col *= gradient;
                return col;
            }
            ENDCG
        }
    }
}
