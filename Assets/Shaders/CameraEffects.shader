Shader "Custom/ReduceColorBits"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NumBits ("Number of color bits", Int) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            int _NumBits;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // Apperantly, colors can exceed 1 for no apperant reason                
                if (col.r >= 1)
                {
                    col.r = 1;
                }
                if (col.g >= 1)
                {
                    col.g = 1;
                }
                if (col.b >= 1)
                {
                    col.b = 1;
                }
                // Apply colors
                col.rgb = floor(floor(col.rgb * 255) / pow(2, 8 - _NumBits)) / (pow(2, _NumBits) - 1);
                return col;
            }
            ENDCG
        }
    }
}
