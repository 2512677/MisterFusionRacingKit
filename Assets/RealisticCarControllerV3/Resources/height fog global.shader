Shader "Hidden/BOXOPHOBIC/Atmospherics/Height Fog Global" {
	Properties {
		[StyledCategory(Fog)] _FogCat ("[ Fog Cat]", Float) = 1
		[Enum(X Axis,0,Y Axis,1,Z Axis,2)] [Space(10)] _FogAxisMode ("Fog Axis Mode", Float) = 1
		[StyledCategory(Skybox)] _SkyboxCat ("[ Skybox Cat ]", Float) = 1
		[StyledCategory(Directional)] _DirectionalCat ("[ Directional Cat ]", Float) = 1
		[StyledCategory(Noise)] _NoiseCat ("[ Noise Cat ]", Float) = 1
		[StyledCategory(Advanced)] _AdvancedCat ("[ Advanced Cat ]", Float) = 1
		[HideInInspector] _HeightFogGlobal ("_HeightFogGlobal", Float) = 1
		[HideInInspector] _IsHeightFogShader ("_IsHeightFogShader", Float) = 1
		[ASEEnd] [StyledBanner(Height Fog Global)] _Banner ("[ Banner ]", Float) = 1
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