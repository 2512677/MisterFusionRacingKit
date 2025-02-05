Shader "BOXOPHOBIC/Atmospherics/Height Fog Standalone" {
	Properties {
		[StyledBanner(Height Fog Standalone)] _Banner ("Banner", Float) = 0
		[StyledCategory(Fog)] _FogCat ("[ Fog Cat]", Float) = 1
		_FogIntensity ("Fog Intensity", Range(0, 1)) = 1
		[Enum(X Axis,0,Y Axis,1,Z Axis,2)] [Space(10)] _FogAxisMode ("Fog Axis Mode", Float) = 1
		[Enum(Multiply Distance and Height,0,Additive Distance and Height,1)] _FogLayersMode ("Fog Layers Mode", Float) = 0
		[Space(10)] [HDR] _FogColorStart ("Fog Color Start", Vector) = (0.4411765,0.722515,1,0)
		[HDR] _FogColorEnd ("Fog Color End", Vector) = (0.4411765,0.722515,1,0)
		_FogColorDuo ("Fog Color Duo", Range(0, 1)) = 1
		[Space(10)] _FogDistanceStart ("Fog Distance Start", Float) = -200
		_FogDistanceEnd ("Fog Distance End", Float) = 200
		_FogDistanceFalloff ("Fog Distance Falloff", Range(1, 8)) = 2
		[Space(10)] _FogHeightStart ("Fog Height Start", Float) = 0
		_FogHeightEnd ("Fog Height End", Float) = 200
		_FogHeightFalloff ("Fog Height Falloff", Range(1, 8)) = 2
		[StyledCategory(Skybox)] _SkyboxCat ("[ Skybox Cat ]", Float) = 1
		_SkyboxFogIntensity ("Skybox Fog Intensity", Range(0, 1)) = 0
		_SkyboxFogHeight ("Skybox Fog Height", Range(0, 1)) = 1
		_SkyboxFogFalloff ("Skybox Fog Falloff", Range(1, 8)) = 2
		_SkyboxFogOffset ("Skybox Fog Offset", Range(-1, 1)) = 0
		_SkyboxFogBottom ("Skybox Fog Bottom", Range(0, 1)) = 0
		_SkyboxFogFill ("Skybox Fog Fill", Range(0, 1)) = 0
		[StyledCategory(Directional)] _DirectionalCat ("[ Directional Cat ]", Float) = 1
		[HDR] _DirectionalColor ("Directional Color", Vector) = (1,0.8280286,0.6084906,0)
		_DirectionalIntensity ("Directional Intensity", Range(0, 1)) = 1
		_DirectionalFalloff ("Directional Falloff", Range(1, 8)) = 2
		_DirectionalDir ("Directional Dir", Vector) = (1,1,1,0)
		[StyledCategory(Noise)] _NoiseCat ("[ Noise Cat ]", Float) = 1
		_NoiseIntensity ("Noise Intensity", Range(0, 1)) = 1
		_NoiseDistanceEnd ("Noise Distance End", Float) = 10
		_NoiseScale ("Noise Scale", Float) = 30
		_NoiseSpeed ("Noise Speed", Vector) = (0.5,0.5,0,0)
		[ASEEnd] [StyledCategory(Advanced)] _AdvancedCat ("[ Advanced Cat ]", Float) = 1
		[HideInInspector] _FogAxisOption ("_FogAxisOption", Vector) = (0,0,0,0)
		[HideInInspector] _HeightFogStandalone ("_HeightFogStandalone", Float) = 1
		[HideInInspector] _IsHeightFogShader ("_IsHeightFogShader", Float) = 1
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