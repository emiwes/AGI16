Shader "Custom/Leaves" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Cutoff ("Cutoff", Range(0,1)) = 0.5
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DisplacementTex ("Displacement", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_DisMul ("Displacement Multiplier", Range(0,1)) = 0.5
		_Wind ("Wind", Vector) = (0,0,0)
		_Speed ("Speed", Range(0,10)) = 1
		_Offset ("Offset", Range(0,10)) = 1
	}
	SubShader {
		Tags { "RenderType"="Cutoff" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows addshadow alphatest:_Cutoff vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DisplacementTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_DisplacemenTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _DisMul;
		float4 _Wind;
		float _Speed;
		float _Offset;

		void vert (inout appdata_full v) {
        	
        	float time = _Time[3] * _Speed + _Offset;
			float sinTime = sin(time);
			float4 disTex = tex2Dlod(_DisplacementTex, v.texcoord);
			v.vertex.xyz += disTex.xyz * _DisMul * sinTime * _Wind.xyz;
        	//o.customColor = abs(v.normal);
        	//UNITY_INITIALIZE_OUTPUT(Input,o);
      	}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
