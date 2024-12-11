Shader "Unlit/Hologram"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0.0, 1.0, 1.0, 1.0)
        _ScrollSpeed ("Scroll Speed", Vector) = (0.1, 0.1, 0.0, 0.0)
        _Transparency ("Transparency", Range(0,1)) = 0.5
        _DistortionStrength ("Distortion Strength", Range(0, 1)) = 0.1
        _ScanLineSpeed ("Scan Line Speed", Range(0, 10)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 noiseUV : TEXCOORD1;
                UNITY_FOG_COORDS(2)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _NoiseTex_ST;
            float4 _Color;
            float4 _ScrollSpeed;
            float _Transparency;
            float _DistortionStrength;
            float _ScanLineSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.noiseUV = TRANSFORM_TEX(v.uv, _NoiseTex);
                o.uv += _ScrollSpeed.xy * _Time.y;

                // Apply sine wave distortion
                o.uv.y += sin(o.uv.x * 10.0 + _Time.y * 5.0) * _DistortionStrength;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                col.a *= _Transparency;

                // Apply noise-based distortion
                float noise = tex2D(_NoiseTex, i.noiseUV + _Time.y * _ScrollSpeed.xy).r;
                col.rgb += noise * _DistortionStrength;

                // Add scan lines
                float scanLine = sin((i.uv.y + _Time.y * _ScanLineSpeed) * 100.0) * 0.1;
                col.rgb += scanLine;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}