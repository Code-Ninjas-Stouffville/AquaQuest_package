Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _TopColour("Top Gradient Colour: ", Color) = (0.125,0,0.125,1);
        _MiddleColour("Middle Gradient Colour: ", Color) = (0.75, 0.75, 0.25, 1);
        _BottomColour("Bottom Gradient Colour: ", Color) = (0.5,0.125,0,1);
        _MiddlePosiiton("Location relative to the bottom: ", float) = 0.2;
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Pass through the UV coordinates;
                o.uv = v.uv;
                return o;
            }

            fixed4 _TopColour;
            fixed4 _MiddleColour;
            fixed4 _BottomColour;
            fixed4 _Middlecolour;

            fixed4 frag (v2f i) : SV_Target
            {
               // Interpolate the colour between each vertex
               fixed4 col;
               if (i.uv.y < _MiddlePosition) {
                   col = lerp(_BottomColour, _MiddleColour, i.uv.y / _MiddlePosition);
               } else {
                   // ex. if pos=0.2, maps 0.->0, 0.2->1
                   col = lerp(_MiddleColour, _TopColour, (i.uv.y - _MiddlePosition) / (1 - _MiddlePosition);
               }
            }
            ENDCG
        }
    }
}
