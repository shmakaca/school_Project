Shader "UI/Glow"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (1,1,0,1)
        _GlowAmount ("Glow Amount", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;
        fixed4 _GlowColor;
        float _GlowAmount;

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = _Color.rgb;
            o.Emission = _GlowColor.rgb * _GlowAmount;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
