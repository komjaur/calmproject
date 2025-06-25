Shader "Sprites/PlayerTeamColorSwap"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite", 2D) = "white" {}
        _TintColor ("Team Color", Color) = (1, 0, 0, 1)  // Default red
        _TargetColor ("Swap Color", Color) = (1, 0.09, 0, 1) // #FF1600 (pure red)
        _Tolerance ("Color Match Tolerance", Range(0, 1)) = 0.05
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Cull Off Lighting Off ZWrite Off Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _TintColor;
            float4 _TargetColor;
            float _Tolerance;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv);

                // Check if color matches _TargetColor within tolerance
                float d = distance(c.rgb, _TargetColor.rgb);
                if (d <= _Tolerance)
                    c.rgb = _TintColor.rgb * c.a;

                return c;
            }
            ENDCG
        }
    }
}
