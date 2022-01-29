Shader "Unlit/GroundShake"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [HDR] _Color("Color", Color) = (0,0,0,0)
        _Pointer("Pointer", Vector) = (0,0,0)

        _Falloff("Falloff", Range(0,10)) = 0

        _Scale("Scale", Float) = 1

        _Timer("Timer", Range(0,1)) = 1
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            Blend one OneMinusSrcAlpha
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #pragma multi_compile_fog

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
                    float3 location : POSITION1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                float3 _Pointer;
                float4 _Color;

                float _Falloff;

                float _Timer;
                float _Scale;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);

                    o.location = mul(unity_ObjectToWorld, v.vertex);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float dist = distance(i.location, _Pointer);

                

                // Set the fade start by subtracting the value and deviding the rest with the falloff
                float correctedDistance = ((dist - _Timer * (_Scale + 1)) / _Falloff);
                correctedDistance = 1 - abs(correctedDistance);

                clip(correctedDistance);

                // Sample texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color * clamp(correctedDistance, 0, 1);
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
        }
}
