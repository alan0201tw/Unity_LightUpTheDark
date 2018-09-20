Shader "_FatshihShader/VertexLitStandardWorldPos" 
{
    Properties 
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Color ("Main Color", COLOR) = (1,1,1,1)

        _IgnoreDimension("Ignore Dimension", int) = 0
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

                float2 worldPosUV : TEXCOORD2;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                
                float3 lightColor : TEXCOORD1;
                float2 worldPosUV : TEXCOORD2;
            };

            half4 _Color;

            sampler2D _MainTex;
            float4 _MainTex_ST; // for tilling and offset

            float _IgnoreDimension;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                if(_IgnoreDimension == 0) // ignore x
                    o.worldPosUV = mul(unity_ObjectToWorld, v.vertex).yz;
                else if(_IgnoreDimension == 1) // ignore y
                    o.worldPosUV = mul(unity_ObjectToWorld, v.vertex).xz;
                else if(_IgnoreDimension == 2) // ignore z
                    o.worldPosUV = mul(unity_ObjectToWorld, v.vertex).xy;
                else
                    o.worldPosUV = mul(unity_ObjectToWorld, v.vertex).xy;

                o.lightColor = ShadeVertexLightsFull(v.vertex , v.normal, 8 , false);

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4( i.lightColor , 1 ) * _Color * tex2D(_MainTex, i.worldPosUV);
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}