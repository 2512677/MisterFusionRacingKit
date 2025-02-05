Shader "TOZ/Debug/VertexColors" {
	SubShader {
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry" "IgnoreProjector" = "True" }
		LOD 100

		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing

			struct a2v {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 pos : SV_POSITION;
				fixed4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(a2v v) {
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_TRANSFER_INSTANCE_ID(v2f, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				return i.color;
			}
			ENDCG
		}
	}

	Fallback Off
}