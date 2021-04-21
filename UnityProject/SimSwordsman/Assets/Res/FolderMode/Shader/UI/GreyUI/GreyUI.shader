Shader "XHH/UI/GreyUI"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" { }
        [Toggle]_IsGrey ("Is Grey", float) = 1
        
        //MASK SUPPORT ADD
        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector]_ColorMask ("Color Mask", Float) = 15
        //MASK SUPPORT END
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }
        
        //MASK SUPPORT ADD
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]
        //MASK SUPPORT END
        
        Pass
        {
            ZWrite off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv: TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex: SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _IsGrey;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));
                
                if (_IsGrey == 1)
                    return float4(grey, grey, grey, col.a);
                
                return col;
            }
            ENDCG
            
        }
    }
}
