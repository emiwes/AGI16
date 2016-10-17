Shader "TowPow/IceSurfaceShader" {
	// Properties {
	// 	_Color ("Color", Color) = (1,1,1,1)
	// 	_MainTex ("Albedo (RGB)", 2D) = "white" {}
	// 	_Glossiness ("Smoothness", Range(0,1)) = 0.5
	// 	_Metallic ("Metallic", Range(0,1)) = 0.0
	// }
	// SubShader {
	// 	Tags { "RenderType"="Transparent" }
	// 	LOD 200
		
	// 	CGPROGRAM
	// 	// Physically based Standard lighting model, and enable shadows on all light types
	// 	#pragma surface surf Standard fullforwardshadows

	// 	// Use shader model 3.0 target, to get nicer looking lighting
	// 	#pragma target 3.0

	// 	sampler2D _MainTex;

	// 	struct Input {
	// 		float2 uv_MainTex;
	// 	};

	// 	half _Glossiness;
	// 	half _Metallic;
	// 	fixed4 _Color;

	// 	void surf (Input IN, inout SurfaceOutputStandard o) {
	// 		// Albedo comes from a texture tinted by color
	// 		fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
	// 		o.Albedo = c.rgb;
	// 		// Metallic and smoothness come from slider variables
	// 		o.Metallic = _Metallic;
	// 		o.Smoothness = _Glossiness;
	// 		o.Alpha = c.a / 2;
	// 	}
	// 	ENDCG
	// }
	// FallBack "Diffuse"


	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_BaseColor ("Base Color", Color) = (1, 1, 1, 1)
		_SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,1)
		_RimPower ("Rim Power", Range(0.5,10.0)) = 1.7
		_Shininess ("Shininess", Range (0.01, 1)) = 0.72
		_Transparency ("Transparency", Range(0,1)) = 0.95
		
	}
	SubShader {
		Tags {
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Opaque"
		}
		LOD 300
		
		CGPROGRAM
			#pragma surface surf BlinnPhong alpha
			
			half _Shininess;
			float4 _BaseColor;
			sampler2D _MainTex;
			float _Transparency;
			sampler2D _BumpMap;
			float4 _RimColor;
			float _RimPower;
			
			
			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
				float3 viewDir;
			};
			
			void surf (Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D (_MainTex, IN.uv_MainTex) +  _BaseColor;
				o.Albedo = c.rgb;
				o.Gloss = 0.5;
				o.Specular = _Shininess;
				o.Alpha = _Transparency;
				o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
				half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
				o.Emission = _RimColor.rgb * pow (rim, _RimPower);
			}
		ENDCG
	}
	FallBack "Transparent/VertexLit"
}
