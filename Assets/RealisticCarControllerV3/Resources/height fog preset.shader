Shader "BOXOPHOBIC/Atmospherics/Height Fog Preset" {
	Properties {
		[HideInInspector] _IsHeightFogPreset ("_IsHeightFogPreset", Float) = 1
		[HideInInspector] _IsHeightFogShader ("_IsHeightFogShader", Float) = 1
		[StyledBanner(Height Fog Preset)] _Banner ("[ Banner ]", Float) = 1
		[StyledCategory(Fog)] _FogCat ("[ FogCat]", Float) = 1
		_FogIntensity ("Fog Intensity", Range(0, 1)) = 1
		[Enum(X Axis,0,Y Axis,1,Z Axis,2)] _FogAxisMode ("Fog Axis Mode", Float) = 1
		[Enum(Multiply Distance And Height,0,Additive Distance And Height,1)] _FogLayersMode ("Fog Layers Mode", Float) = 0
		[HideInInspector] _FogAxisOption ("_FogAxisOption", Vector) = (0,0,0,0)
		[Space(10)] [HDR] _FogColorStart ("Fog Color Start", Vector) = (0.4411765,0.722515,1,1)
		[HDR] _FogColorEnd ("Fog Color End", Vector) = (0.8862745,1.443137,2,1)
		_FogColorDuo ("Fog Color Duo", Range(0, 1)) = 1
		[Space(10)] _FogDistanceStart ("Fog Distance Start", Float) = -200
		_FogDistanceEnd ("Fog Distance End", Float) = 200
		_FogDistanceFalloff ("Fog Distance Falloff", Range(1, 8)) = 2
		[Space(10)] _FogHeightStart ("Fog Height Start", Float) = 0
		_FogHeightEnd ("Fog Height End", Float) = 200
		_FogHeightFalloff ("Fog Height Falloff", Range(1, 8)) = 2
		[StyledCategory(Skybox)] _SkyboxCat ("[ SkyboxCat ]", Float) = 1
		_SkyboxFogIntensity ("Skybox Fog Intensity", Range(0, 1)) = 1
		_SkyboxFogHeight ("Skybox Fog Height", Range(0, 1)) = 1
		_SkyboxFogFalloff ("Skybox Fog Falloff", Range(1, 8)) = 1
		_SkyboxFogOffset ("Skybox Fog Offset", Range(-1, 1)) = 0
		_SkyboxFogBottom ("Skybox Fog Bottom", Range(-1, 1)) = 0
		_SkyboxFogFill ("Skybox Fog Fill", Range(0, 1)) = 1
		[StyledCategory(Directional)] _DirectionalCat ("[ DirectionalCat ]", Float) = 1
		[HDR] _DirectionalColor ("Directional Color", Vector) = (1,0.7793103,0.5,1)
		_DirectionalIntensity ("Directional Intensity", Range(0, 1)) = 1
		_DirectionalFalloff ("Directional Falloff", Range(1, 8)) = 2
		[HideInInspector] _DirectionalDir ("Directional Dir", Vector) = (0,0,0,0)
		[StyledCategory(Noise)] _NoiseCat ("[ NoiseCat ]", Float) = 1
		[HideInInspector] _NoiseModeBlend ("_NoiseModeBlend", Float) = 1
		_NoiseIntensity ("Noise Intensity", Range(0, 1)) = 1
		_NoiseDistanceEnd ("Noise Distance End", Float) = 50
		_NoiseScale ("Noise Scale", Float) = 30
		[ASEEnd] _NoiseSpeed ("Noise Speed", Vector) = (0.5,0,0.5,0)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	//CustomEditor "HeightFogShaderGUI"
}