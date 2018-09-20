Shader "_FatshihShader/VertexLitStandard" 
{
    Properties 
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Color ("Main Color", COLOR) = (1,1,1,1)
    }

    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        LOD 80

        // Non-lightmapped
        Pass {
            Tags { "LightMode" = "Vertex" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                
                float2 uv : TEXCOORD0;
                float3 lightColor : TEXCOORD1;
            };

            half4 _Color;

            sampler2D _MainTex;
            float4 _MainTex_ST; // for tilling and offset
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                o.lightColor = ShadeVertexLightsFull(v.vertex , v.normal, 8 , false);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4( i.lightColor , 1 ) * _Color * tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}