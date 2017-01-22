Shader "Custom/RimEffect_Atmosfera" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_FresnelFalloff ("Fresnel Falloff", Range(0.1, 20.0)) = 2
		_Cutoff ("CutOff",  Range(0, 1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		half _FresnelFalloff;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};
	
		fixed4 _Color;
		fixed4 _Color2;
		float _Cutoff;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

			float InvNdotView = 1 - saturate(dot(o.Normal, normalize(IN.viewDir)));
			//concentrar o InvNdotView nas extremidades
			float rimEffect = pow( InvNdotView, _FresnelFalloff);

			if(rimEffect > _Cutoff )
			{
				//
//				o.Emission = (_Color2.rgb * rimEffect);
			}else{
				o.Emission = (lerp(_Color.rgb,_Color2.rgb,rimEffect) );
			}

			o.Albedo = c.rgb;	
			// Metallic and smoothness come from slider variables
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
