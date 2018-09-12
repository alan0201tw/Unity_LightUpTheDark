// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified VertexLit shader. Differences from regular VertexLit one:
// - no per-material color
// - no specular
// - no emission

Shader "_FatshihShader/VertexLitSingleColor" 
{
    Properties 
    {
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                
                float3 lightColor : TEXCOORD1;
            };

            half4 _Color;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                o.lightColor = ShadeVertexLightsFull(v.vertex , v.normal, 8 , false);

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4( i.lightColor , 1 ) * _Color;
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}