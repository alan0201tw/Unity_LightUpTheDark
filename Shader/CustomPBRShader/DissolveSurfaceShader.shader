Shader "_FatshihShader/DissolveSurfaceShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NoiseTex("Noise_Tex" , 2D) = "white" {}

		_EdgeColor("Edge Color" , Color) = (1,1,1,1)
		_EdgeLength("Edge Length" , Range(0,0.2)) = 0.01
		_Threshold("Threshold" , Range(0,1)) = 0

		_NormalEmissionRate("Normal Emission Rate", Range(0,1)) = 1

		[Toggle] _IsVertexBased("Is sample done in vertex?", float) = 0
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Cull Off
		
		CGPROGRAM
		
		#pragma surface surf Lambert vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float sampleN;
		};

		fixed4 _Color;
		
		sampler2D _NoiseTex;

		float4 _EdgeColor;
		float _Threshold;
		float _EdgeLength;

		float _NormalEmissionRate;

		float _IsVertexBased;

		void vert (inout appdata_full v, out Input o)
		{
			o.sampleN = tex2Dlod(_NoiseTex , v.texcoord).r;
			o.uv_MainTex = v.texcoord;
      	}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float sampleN = (_IsVertexBased > 0) ? IN.sampleN : tex2D(_NoiseTex, IN.uv_MainTex);
			// clip(value) > this will discard fragment if value is less than zero
			clip( sampleN <= _Threshold ? -1 : 1);

			float4 mainColor = tex2D(_MainTex , IN.uv_MainTex);

			if(sampleN >= _Threshold + _EdgeLength)
			{
				o.Albedo = mainColor * _Color;
				o.Emission = mainColor * _Color * _NormalEmissionRate;
			}
			else
			{
				o.Albedo = _EdgeColor;
				o.Emission = _EdgeColor;
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
