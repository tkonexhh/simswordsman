Shader "Unlit/SpriteCircleProgress"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

        _CenterPos("CenterPos", vector) = (.5,.5,0,0)
        _StartPos("StartPos", vector) = (0,-0.5,0,0)
        _FillPercent("FillPercent", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            float4 _CenterPos;
            float4 _StartPos;
            float _FillPercent;

            v2f vert(appdata_t IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID (IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 result = SampleSpriteTexture (IN.texcoord) * IN.color;

                fixed2 p = fixed2(IN.texcoord.x - _CenterPos.x, IN.texcoord.y - _CenterPos.y);

                float alpha = 1;
                if (_FillPercent < 0.5)
                {
                    float compare = (_FillPercent * 2 - 0.5)*3.1415926;
                    float theta = atan(p.y / p.x);
                    if (theta > compare)
                    {
                        alpha = 0;
                    }
                    if (p.x > 0)
                    {
                        alpha = 0;
                    }
                }
                else
                {
                    float compare = ((_FillPercent - 0.5) * 2 - 0.5)*3.1415926;
                    float theta = atan(p.y / p.x);
                    if (p.x > 0)
                    {
                        if (theta > compare)
                        {
                            alpha = 0;
                        }
                    }
                }

                result.a *= alpha;
                result.rgb *= result.a;

                return result;
            }
        ENDCG
        }
    }
}
