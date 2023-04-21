Shader "Hybreed/Shaded Vertex Color Lit"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Emissive ("Emissive", float) = 1.0
	}
	SubShader
	{	
		Tags
		{
			"RenderType"="Opaque"
		}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;

		// Lights
		fixed _Emissive;

		struct Input
		{
			fixed4 color: COLOR;
			float2 uv_MainTex : TEXCOORD0;
		};

		static const fixed numStops = 16;

//		static const fixed ix_Emmisive = 3;

//		fixed PaletteCheck(fixed alpha, fixed ix)
//		{
//			fixed minValue = (ix - 0.01f) / (numStops - 1);
//			fixed maxValue = (ix + 0.01f) / (numStops - 1);
//
//			fixed test = clamp(-sign(alpha - maxValue), 0, 1);
//			test = test * clamp(sign(alpha - minValue), 0, 1);
//
//			return test;
//		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 col = IN.color;
			fixed shading = IN.uv_MainTex.x;
			o.Albedo = col * shading;

//			fixed emmisive = _Emmisive * PaletteCheck(IN.color.a, ix_Emmisive);
//			o.Emission = IN.color * (emmisive);
			o.Emission = 0;
		}
		ENDCG	
	}

	Fallback "VertexLit"
}
