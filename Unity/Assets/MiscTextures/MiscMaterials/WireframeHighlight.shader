Shader "Custom/WireframeHighlight" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM

#pragma surface Highlighter Lambert
			struct Input {
			float4 color : COLOR;
		};
		void Highlighter(Input IN, inout SurfaceOutput o) {
			o.Albedo = (1, 1, 1, 0.5);
		}

		ENDCG
	}
	Fallback "Diffuse"
}
