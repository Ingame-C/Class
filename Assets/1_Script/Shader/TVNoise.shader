Shader "Custom/TVNoise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Alpha ("Alpha", Range(0, 1)) = 1.0  // 알파 값을 조절할 수 있는 속성 추가
        _GridSize ("GridSize", Range(1, 256)) = 1
        _Darkness ("Darkness", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha  // 알파 블렌딩 설정

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
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float _TimeValue;
            float _Alpha;  // 알파 값 속성
            int _GridSize;
            float _Darkness;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 quantizedUV = floor(i.uv * _GridSize) / _GridSize;

                float noise = frac(sin(dot(quantizedUV * _TimeValue, float2(12.9898, 78.233))) * 43758.5453);

                noise *= _Darkness;
                return fixed4(noise, noise, noise, _Alpha);
            }
            ENDCG
        }
    }
}