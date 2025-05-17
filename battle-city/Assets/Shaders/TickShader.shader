Shader "Unlit/TickShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("MainColor", Color) = (1,1,1,1)
        _NoiseSnap("Noise Snap",Float) = 0.08
        _NoiseScale("Noise Scale",Float) = 0.008

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                //fixed4 color;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _NoiseSnap;
            float _NoiseScale;

            inline float snap(float x, float snap)
            {
                return snap * round(x / snap);
            }

            float rand(float2 co)
            {
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
            }

            float3 random3(float2 co)
            {
                return float3(rand(co), rand(co), rand(co));
            }

            float getNoise(float3 vertex)
            {
                float time = snap(_Time.y, _NoiseSnap);
                return random3(vertex + float3(time, 0.0, 0.0)).xy * _NoiseScale;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //float time = snap(_Time.y, _NoiseSnap);
                //float2 noise = random3(o.vertex.xyz + float3(time, 0.0, 0.0)).xy * _NoiseScale;
                float2 noise = 0;//getNoise(o.vertex.xyz);
                o.vertex.xy += noise;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                if(col.w != 0)
                {
                    col = _Color;
                }
                else
                {
                    col = float4(0,0,0,0);
                }
                
                return col;
            }
            ENDCG
        }
    }
}
