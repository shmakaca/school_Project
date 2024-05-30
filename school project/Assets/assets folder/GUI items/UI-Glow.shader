// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutlineShader"
{
    Properties
    {
        _Color ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (0.0, 0.03)) = 0.005
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            ZWrite On
            ColorMask RGB
            Cull Front
            Offset 10,10

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            float _Outline;
            float4 _OutlineColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                float4 offset = UnityObjectToClipPos(v.vertex);
                offset.xyz += normalize(offset.xyz) * _Outline;
                o.pos = offset;

                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                return _OutlineColor;
            }
            ENDCG
        }
        Pass
        {
            Name "BACKFACE"
            Tags { "LightMode" = "Always" }
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                return 0;
            }
            ENDCG
        }
    }
}
