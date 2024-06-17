Shader "Custom/PostProcess"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Gamma ("Gamma", Range(0.5, 2.0)) = 1.0
        _Contrast ("Contrast", Range(0.5, 2.0)) = 1.0
        _Brightness ("Brightness", Range(0.5, 2.0)) = 1.0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Gamma;
            float _Contrast;
            float _Brightness;

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                col.rgb = pow(col.rgb, 1.0 / _Gamma);
                col.rgb = ((col.rgb - 0.5) * _Contrast + 0.5) * _Brightness;

                return col;
            }
            ENDCG
        }
    }
}
