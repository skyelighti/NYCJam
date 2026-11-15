Shader "Custom/Billboard"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Flip("Flip", Float) = 1
    }
   
    SubShader
    {
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "DisableBatching" = "True" }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off   

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            const float3 vect3Zero = float3(0.0, 0.0, 0.0);
            float _Flip;
            sampler2D _MainTex;

            v2f vert(appdata v)
            {
                v2f o;



                float4 camPos = float4(UnityObjectToViewPos(vect3Zero).xyz, 1.0);    // UnityObjectToViewPos(pos) is equivalent to mul(UNITY_MATRIX_MV, float4(pos, 1.0)).xyz,
                                                                                    // This gives us the camera's origin in 3D space (the position (0,0,0) in Camera Space)

                float4 viewDir = float4(v.pos.x * _Flip, v.pos.y, 0.0, 0.0);     // Since w is 0.0, in homogeneous coordinates this represents a vector direction instead of a position
                float4 outPos = mul(UNITY_MATRIX_P, camPos + viewDir);            // Add the camera position and direction, then multiply by UNITY_MATRIX_P to get the new projected vert position

                o.pos = outPos;
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Don't need to do anything special, just render the texture
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}