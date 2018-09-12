Shader "_FatshihShader/PivotFadeOutShader"
{
	Properties
	{
		_MainColor ("Main Color", Color) = (1,1,1,1)
		_MaxDistance("Max Distance" , float) = 1
		_TimeCoefficient("Time Coefficient" , float) = 3

		_NoiseTex("Noise Texture", 2D) = "white" {}
		_NoiseCoef("Noise coefficient" , Range(0.0,1.0)) = 0.2

		_Baseline("Base line ( frags have larger y will be transparent )" , float) = 0
	}
	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull Off
		
		Tags
		{
			"RenderType"="Transparent"
			"Queue"="Transparent"
		}

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

				float noiseValue : TEXCOORD2;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD0;

				float noiseValue : TEXCOORD2;
			};

			fixed4 _MainColor;
			fixed3 _PivotPosition;
			float _MaxDistance;
			float _TimeCoefficient;
			float _NoiseCoef;

			float _Baseline;

			sampler2D _NoiseTex;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul (unity_ObjectToWorld, v.vertex);
				o.uv = v.uv;

				fixed2 noiseUV = o.worldPos.xz + _Time.x * _TimeCoefficient;
				o.noiseValue = tex2Dlod(_NoiseTex , float4(noiseUV , 0 ,0));

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float localBaseline = mul(unity_ObjectToWorld, float4(0,_Baseline,0,1)).y;

				float dist = abs(i.worldPos.y - localBaseline);

				// noise from [0 , 1], map to [0.8 , 1]
				float lerpParameter = dist / (_MaxDistance * ((1-_NoiseCoef) + i.noiseValue * _NoiseCoef));

				fixed finalAlpha = lerp(_MainColor.a , 0 , max(0, lerpParameter));
				fixed4 col = _MainColor;
				col.a = finalAlpha;

				return col;
			}
			ENDCG
		}
	}
}
