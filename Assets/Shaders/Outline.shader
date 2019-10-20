Shader "Custom/Outline"
{Properties {
		_Color ("Tint", Color) = (0, 0, 0, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_Smoothness ("Smoothness", Range(0, 1)) = 0
		_Metallic ("Metalness", Range(0, 1)) = 0
		[HDR] _Emission ("Emission", color) = (0,0,0)
		_FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
		[PowerSlider(4)] _FresnelExponent ("Fresnel Exponent", Range(0.25, 4)) = 1

	}
	SubShader {
		Tags{ "RenderType"="Opaque" "Queue"="Geometry"}

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		float3 _FresnelColor;
		float _FresnelExponent;

		half _Smoothness;
		half _Metallic;
		half3 _Emission;

		struct Input {
			float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
    		INTERNAL_DATA
		};

		void surf (Input i, inout SurfaceOutputStandard o) {
			fixed4 col = tex2D(_MainTex, i.uv_MainTex);
			col *= _Color;
			o.Albedo = col.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;

			//get the dot product between the normal and the direction
			float fresnel = dot(i.worldNormal, i.viewDir);
			 //clamp the value between 0 and 1 so we don't get dark artefacts at the backside
    		fresnel = saturate(1-fresnel);
			//raise the fresnel value to the exponents power to be able to adjust it
    		fresnel = pow(fresnel, _FresnelExponent);
			//combine the fresnel value with a color
    		float3 fresnelColor = fresnel * _FresnelColor;
			o.Emission = _Emission + fresnelColor;
		}
		ENDCG
	}
	FallBack "Standard"
}