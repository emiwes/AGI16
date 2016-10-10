Shader "Unlit/LeavesUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_AlphaCutoff ("Alpha Cutoff", Range(0,1)) = 0.5
		_DisplacementTex ("Displacement", 2D) = "white" {}
		_DisMul ("Displacement Multiplier", Range(0,1)) = 0.5
		_Wind ("Wind", Vector) = (0,0,0)
		_Speed ("Speed", Range(0,10)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Cutoff" }

		LOD 100

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase
			#include "UnityCG.cginc"

			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float3 lightDir : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float2 uv : TEXCOORD2;
				UNITY_FOG_COORDS(3)
				float4 pos : SV_POSITION;
                LIGHTING_COORDS(4,5)
			};

			sampler2D _MainTex;
			float _AlphaCutoff;
			sampler2D _DisplacementTex;
			float _DisMul;
			float4 _Wind;
			float _Speed;
			float4 _MainTex_ST;

			float4 _LightColor0;
			
			v2f vert (appdata v)
			{
				// Displace vers
				float time = _Time[3] * _Speed;
				float sinTime = sin(time);
				float4 disTex = tex2Dlod(_DisplacementTex, float4(v.uv.xy, 0, 0));
				v.vertex.xyz += disTex.xyz * _DisMul * sinTime * _Wind.xyz;

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
				o.normal = normalize(v.normal).xyz;
				UNITY_TRANSFER_FOG(o,o.pos);

				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);



				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				// apply shadows
				float3 L = normalize(i.lightDir);
				float3 N = normalize(i.normal);	 

				float attenuation = LIGHT_ATTENUATION(i);
				float4 ambient = UNITY_LIGHTMODEL_AMBIENT;

				float NdotL = saturate(dot(N, L));
				float4 diffuseTerm = NdotL * _LightColor0 * attenuation;

				float4 finalColor = (ambient + diffuseTerm) * col;

//				float4 ambient = UNITY_LIGHTMODEL_AMBIENT * 4;
//				float attenuation = LIGHT_ATTENUATION(i) * 4;
//				col *= ambient * attenuation;

				// Discard if alpha is less than cutoff
				if(col.a < _AlphaCutoff) {
					discard;
				}

				return finalColor;
			}
			ENDCG
		}
	}
	FallBack "Standard"
}
