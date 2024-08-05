Shader "BadlyInterrogated/TwoColorGradientShader" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_TopColor ("Top Color", Color) = (1,0,0,1)
		_BottomColor ("Bottom Color", Color) = (0,0,1,1)
	}
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			sampler2D _MainTex;
			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			float4 _TopColor;
			float4 _BottomColor;

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				
				float verticalLerp = v.vertex.y * 0.5 + 0.5; // Normalize vertex.y to 0-1 range
				o.color = lerp(_BottomColor, _TopColor, verticalLerp);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				fixed4 texColor = tex2D(_MainTex, i.uv);
				return texColor * i.color;
			}
			ENDCG
		}
	}
}
