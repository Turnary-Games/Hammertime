Shader "Toony Colors/Toony Colors"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
	
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		
        _SColor ("Shadow Color", Color) = (0.0,0.0,0.0,1)
		_LColor ("Highlight Color", Color) = (0.5,0.5,0.5,1)
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
CGPROGRAM
		#pragma surface surf ToonRamp
		
		sampler2D _MainTex;
		sampler2D _Ramp;
		float4 _LColor;
		float4 _SColor;
		float4 _Color;
		
		// custom lighting function that uses a texture ramp based
		// on angle between light direction and normal
		#pragma lighting ToonRamp exclude_path:prepass
		inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
		{
			#ifndef USING_DIRECTIONAL_LIGHT
			lightDir = normalize(lightDir);
			#endif
			
			//Calculate N . L
			half d = dot (s.Normal, lightDir)*0.5 + 0.5;
			//Basic toon shading
			half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;
			//Gooch shading
			ramp = lerp(_SColor,_LColor,ramp);
			
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * atten;
			c.a = s.Alpha;
			
			return c;
		}
		
		struct Input
		{
			float2 uv_MainTex : TEXCOORD0;
		};
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			
			o.Albedo = c.rgb * _Color.rgb;
			o.Alpha = 1;
		}
ENDCG
	
	}
	
	Fallback "Toon/Lighted"
}
