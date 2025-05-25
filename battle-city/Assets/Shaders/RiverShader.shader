Shader "Custom/RiverShader"
{
Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}
        _Speed ("Wave Speed", Float) = 1.0
        _Frequency ("Wave Frequency", Float) = 2.0
        _Amplitude ("Wave Amplitude", Float) = 0.01
        _Tint ("Water Tint", Color) = (0.3, 0.5, 0.7, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;
            float _Frequency;
            float _Amplitude;
            float4 _Tint;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                float time = _Time.y;
                float2 uv = i.uv;

                // Apply sine wave distortion
                uv.x += sin((uv.y + time * _Speed) * _Frequency) * _Amplitude;
                uv.y += sin((uv.x + time * _Speed * 0.7) * _Frequency) * _Amplitude;

                float4 col = tex2D(_MainTex, uv);
                col.rgb *= _Tint.rgb;

                return col;
            }
            ENDHLSL
        }
    }    
}