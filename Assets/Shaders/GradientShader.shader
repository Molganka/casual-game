Shader "Custom/GradientShader_AutoUI"
{
    Properties 
    {
        _TopColour("Top Gradient Colour", Color) = (1, 1, 1, 1)
        _BottomColour("Bottom Gradient Colour", Color) = (0, 0, 0, 1)
        [PerRendererData] _MainTex("Main Texture", 2D) = "white" {}
        _TopAlphaMultiplier("Top Colour Alpha Multiplier", Range(0,1)) = 0.7
    }

    SubShader
    {
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        LOD 100
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            fixed4 _TopColour;
            fixed4 _BottomColour;
            float _TopAlphaMultiplier;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);

                fixed4 topColor = _BottomColour;
                fixed4 bottomColor = _TopColour;

                // ѕровер€ем, бела€ ли текстура
                bool isWhiteTexture = (abs(texColor.r - 1.0) < 0.01) && (abs(texColor.g - 1.0) < 0.01) && (abs(texColor.b - 1.0) < 0.01);

                if (isWhiteTexture)
                {
                    // Ёто 3D объект без спрайта Ч делаем topColor немного прозрачным
                    topColor.a *= _TopAlphaMultiplier;
                }
                // »наче (спрайт или UI) Ч оставл€ем полную альфу

                // √радиент от bottom к top
                fixed4 gradientColor = lerp(bottomColor, topColor, i.uv.x);

                return texColor * gradientColor;
            }
            ENDCG
        }
    }
}
