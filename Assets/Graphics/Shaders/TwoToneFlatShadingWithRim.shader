Shader "Custom/TwoToneFlatShading"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}

	[HDR]
		_ShadowColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
		_RimIntensity("Rim Intensity", Range(0.5, 2)) = 0.1
	}
		
	SubShader
	{
		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase"
				"PassFlags" = "OnlyDirectional"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

		#pragma multi_compile_fwdbase

		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "AutoLight.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float4 uv : TEXCOORD0;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float3 worldNormal : NORMAL;
			float2 uv : TEXCOORD0;
			float3 viewDir : TEXCOORD1;
			SHADOW_COORDS(2)
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;

		v2f vert(appdata v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.worldNormal = UnityObjectToWorldNormal(v.normal);
			o.viewDir = WorldSpaceViewDir(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			TRANSFER_SHADOW(o)
			return o;
		}

		float4 _Color;
		float4 _ShadowColor;
		float _RimThreshold;
		float _RimIntensity;
		
		float4 frag(v2f i) : SV_Target
		{
			float3 normal = normalize(i.worldNormal);
			float3 viewDir = normalize(i.viewDir);

			float NdotL = dot(_WorldSpaceLightPos0, normal);
			float shadow = SHADOW_ATTENUATION(i);
			float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);

			float4 light = lightIntensity * _LightColor0;

			half4 newColor = _Color * _LightColor0;


			if (lightIntensity < 0.1) {
			newColor = lerp(newColor, _ShadowColor, step(0.1, 1 - lightIntensity));
			}

			float4 sample = tex2D(_MainTex, i.uv);

			// Calculate rim lighting.
			float rimDot = 1 - dot(normalize(viewDir), normalize(normal));

			if (rimDot > _RimThreshold) {
				newColor = newColor * _RimIntensity;
			}

			return newColor * sample;
		}
		ENDCG
	}

	UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}