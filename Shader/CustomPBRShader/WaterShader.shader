// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified VertexLit shader. Differences from regular VertexLit one:
// - no per-material color
// - no specular
// - no emission

Shader "_FatshihShader/WaterShader" 
{
    Properties 
    {
        _Color ("Main Color", COLOR) = (1,1,1,0.1)
        _IntersectionColor ("_IntersectionColor", COLOR) = (1,1,1,1)

        _IntersectionMax("Intersection Max" , float) = 2
        _IntersectionDamper("Intersection Damper" , Range(0,1)) = 0
    }

    SubShader 
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 80

        // No culling or depth
		//Cull Off 
        ZWrite Off 
        ZTest LEqual

		//Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass 
        {
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
                float4 screenPos : TEXCOORD2;
            };

            half4 _Color;
            half4 _IntersectionColor;

            float _IntersectionMax;
            float _IntersectionDamper;

            sampler2D _CameraDepthTexture;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                o.screenPos = ComputeScreenPos(o.vertex);
                o.lightColor = ShadeVertexLightsFull(v.vertex , v.normal, 8 , false);

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float rawDepth = UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                float linearDepth = LinearEyeDepth (rawDepth);
                float pixelZ = i.screenPos.z;

                float intersection_dist = sqrt(pow(linearDepth - pixelZ,2));

				half highlight_mask = max(0, sign(_IntersectionMax - intersection_dist));
				highlight_mask *= 1 - intersection_dist / _IntersectionMax * _IntersectionDamper;

				fixed4 col = _Color;

				highlight_mask *= _IntersectionColor.a;

				col *=  (1-highlight_mask);
				col +=  _IntersectionColor * highlight_mask;

				col.a = max(highlight_mask, col.a);

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

                fixed4 finalColor = col + fixed4( i.lightColor , 0 );
                finalColor.rgb /= 2;
                finalColor.a = col.a;

                

				return finalColor;
            }
            ENDCG
        }
    }

    // unchecking will cause shadow casting
    //Fallback "Diffuse"
}