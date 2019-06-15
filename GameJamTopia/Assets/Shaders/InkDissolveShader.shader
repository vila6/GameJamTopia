Shader "Custom/InkDissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve", 2D) = "white" {}
        _Amount("Amount", Range(0,1)) = 0
        //_Tint("Tint", Color) = (1,1,0,0) // Color of the dissolve Line
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
	    Blend One OneMinusSrcAlpha
	    ColorMask RGB
	    Cull Off Lighting On ZWrite Off
	    BlendOp [_BlendOp]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 texcoordAmount : TEXCOORD1;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 color: COLOR;
                float4 vertex : SV_POSITION;
                float4 texcoordAmount : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _DissolveTex;

            //float4 _Tint;
            
            float4 _MainTex_ST;
            float4 _DissolveTex_ST;
            half _Amount;

            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.texcoordAmount = _Amount;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                //Dissolve function
                half dissolve_value = tex2D(_DissolveTex, i.uv).r;
                clip(dissolve_value - _Amount);

                //col *=  _Tint;// tint

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
