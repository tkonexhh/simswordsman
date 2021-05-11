﻿Shader "Custom/MyCircleShader"
{
    
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" { }
        
        _Color ("Tint", Color) = (1, 1, 1, 1)
        
        _StencilComp ("Stencil Comparison", Float) = 8
        
        _Stencil ("Stencil ID", Float) = 0
        
        _StencilOp ("Stencil Operation", Float) = 0
        
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        
        _ColorMask ("Color Mask", Float) = 15
        
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        
        _Center ("Center", vector) = (0, 0, 0, 0)
        
        _Slider ("Slider", Range(0, 1500)) = 1500
        _AlphaSlider ("AlphaSlider", Range(0, 1)) = 0
        //[KeywordEnum(ROUND, RECTANGLE, NULL)] _MaskMode ("Mask mode", Float) = 0
        _MaskMode ("Mask mode", Float) = 0
        _rectSize ("矩形尺寸", vector) = (0, 0, 0, 0)
    }
    
    SubShader
    {
        
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True" }
        
        Stencil
        {
            
            Ref [_Stencil]
            
            Comp [_StencilComp]
            
            Pass [_StencilOp]
            
            ReadMask [_StencilReadMask]
            
            WriteMask [_StencilWriteMask]
        }
        
        Cull Off
        
        Lighting Off
        
        ZWrite Off
        
        ZTest [unity_GUIZTestMode]
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        ColorMask [_ColorMask]
        
        Pass
        {
            
            Name "Default"
            
            CGPROGRAM
            
            #pragma vertex vert
            
            #pragma fragment frag
            
            #pragma target 2.0
            
            #include "UnityCG.cginc"
            
            #include "UnityUI.cginc"
            
            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            
            #pragma multi_compile __ UNITY_UI_ALPHACLIP
            
            //#pragma multi_compile _MASKMODE_ROUND _MASKMODE_RECTANGLE _MASKMODE_NULL
            
            struct appdata_t
            {
                
                float4 vertex: POSITION;
                
                float4 color: COLOR;
                
                float2 texcoord: TEXCOORD0;
                
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f
            {
                
                float4 vertex: SV_POSITION;
                
                fixed4 color: COLOR;
                
                float2 texcoord: TEXCOORD0;
                
                float4 worldPosition: TEXCOORD1;
                
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            fixed4 _Color;
            
            fixed4 _TextureSampleAdd;
            
            float4 _ClipRect;
            
            float2 _Center;
            
            float _Slider;
            float _AlphaSlider;
            float2 _rectSize;
            uniform float _MaskMode;
            
            v2f vert(appdata_t v)
            {
                v2f OUT;
                
                UNITY_SETUP_INSTANCE_ID(v);
                
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                OUT.worldPosition = v.vertex;
                
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                
                OUT.texcoord = v.texcoord;
                
                OUT.color = v.color * _Color;
                
                return OUT;
            }
            
            sampler2D _MainTex;
            
            fixed4 frag(v2f IN): SV_Target
            {
                half4 finallColor = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                
                #ifdef UNITY_UI_CLIP_RECT
                    finallColor.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
                #ifdef UNITY_UI_ALPHACLIP
                    clip(finallColor.a - 0.1);
                #endif
                
                if(_MaskMode<1.0)
                {
                    if (distance(IN.worldPosition.xy, _Center.xy) - _Slider >= _Slider * _AlphaSlider)
                        finallColor.a *= 1;
                    else if (distance(IN.worldPosition.xy, _Center.xy) - _Slider <= 0)
                    finallColor.a *= 0;
                    else
                    finallColor.a *= lerp(0, 1, (distance(IN.worldPosition.xy, _Center.xy) - _Slider) / (_Slider * _AlphaSlider));
                    
                    finallColor.rgb *= finallColor.a;
                }else
                {
                    // 计算片元世界坐标和目标中心位置的距离
                    half disX = distance(IN.worldPosition.x, _Center.x);
                    half disY = distance(IN.worldPosition.y, _Center.y);
                    // x决定像素点应该去掉返回1，不去掉返回0
                    int clipX = step(disX, _rectSize.x);
                    int clipY = step(disY, _rectSize.y);
                    
                    clip(disX - (_rectSize.x) * clipY);
                    clip(disY - (_rectSize.y) * clipX);
                    
                    // x在范围内返回1，不在范围内返回0
                    int insideX = step(disX, _rectSize.x);
                    int insideY = step(disY, _rectSize.y);
                    half alphaX = (1 - insideX) + insideX * (disX - (_rectSize.x));
                    half alphaY = (1 - insideY) + insideY * (disY - (_rectSize.y));
                    finallColor.a *= max(alphaX, alphaY);
                }
                //#ifdef _MASKMODE_ROUND
                //    if (distance(IN.worldPosition.xy, _Center.xy) - _Slider >= _Slider * _AlphaSlider)
                //        finallColor.a *= 1;
                //    else if (distance(IN.worldPosition.xy, _Center.xy) - _Slider <= 0)
                //    finallColor.a *= 0;
                //    else
                //    finallColor.a *= lerp(0, 1, (distance(IN.worldPosition.xy, _Center.xy) - _Slider) / (_Slider * _AlphaSlider));
                    
                //    finallColor.rgb *= finallColor.a;
                //#elif _MASKMODE_RECTANGLE
                //    // 计算片元世界坐标和目标中心位置的距离
                //    half disX = distance(IN.worldPosition.x, _Center.x);
                //    half disY = distance(IN.worldPosition.y, _Center.y);
                //    // x决定像素点应该去掉返回1，不去掉返回0
                //    int clipX = step(disX, _rectSize.x);
                //    int clipY = step(disY, _rectSize.y);
                    
                //    clip(disX - (_rectSize.x) * clipY);
                //    clip(disY - (_rectSize.y) * clipX);
                    
                //    // x在范围内返回1，不在范围内返回0
                //    int insideX = step(disX, _rectSize.x);
                //    int insideY = step(disY, _rectSize.y);
                //    half alphaX = (1 - insideX) + insideX * (disX - (_rectSize.x));
                //    half alphaY = (1 - insideY) + insideY * (disY - (_rectSize.y));
                //    finallColor.a *= max(alphaX, alphaY);
                //#endif
                
                return finallColor;
            }
            
            ENDCG
            
        }
    }
}