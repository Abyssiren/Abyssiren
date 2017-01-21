Shader "Custom/ToonLitDissolve" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
		_DissolveTex("Dissolve Map", 2D) = "white" {}
		_DissolveEdgeColor("Dissolve Edge Color", Color) = (1,1,1,0)
		_DissolveIntensity("Dissolve Intensity", Range(0.0, 1.0)) = 0
		_DissolveEdgeRange("Dissolve Edge Range", Range(0.0, 1.0)) = 0
		_DissolveEdgeMultiplier("Dissolve Edge Multiplier", Float) = 1
	}

		SubShader{ 
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200
		

		CGPROGRAM
#pragma surface surf ToonRamp addshadow

		sampler2D _Ramp;

	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif

		half d = dot(s.Normal, lightDir)*0.5 + 0.5;
		half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
		c.a = 0;
		return c;
	}


	sampler2D _MainTex;
	float4 _Color;
	sampler2D _DissolveTex;

	uniform float _BumpPower;
	uniform float4 _DissolveEdgeColor;
	uniform float _DissolveEdgeRange;
	uniform float _DissolveIntensity;
	uniform float _DissolveEdgeMultiplier;


	struct Input {
		float2 uv_MainTex : TEXCOORD0;
		float2 uv_DissolveTex;
		float3 worldPos;
		float3 viewDir;
		float3 worldNormal;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;

		float4 dissolveColor = tex2D(_DissolveTex, IN.uv_DissolveTex);
		half dissolveClip = dissolveColor.r - _DissolveIntensity;
		half edgeRamp = max(0, _DissolveEdgeRange - dissolveClip);
		clip(dissolveClip);

		o.Albedo = lerp(c.rgb, _DissolveEdgeColor, min(1, edgeRamp * _DissolveEdgeMultiplier));
		return;
	}
	ENDCG

	}

		Fallback "Diffuse"
}
