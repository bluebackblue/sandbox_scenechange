

/** @brief フェードエンジン。ローディング。
*/


Shader "FadeEngine/Loading"
{
	Properties
	{
		/** _MainTex
		*/
		_MainTex("_MainTex",2D) = "white"{}

		/** move
		*/
		move("move",float) = 0
	}
	SubShader
	{
		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}
		Pass
		{
			Cull Off
			ZTest Always
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			/** include
			*/
			#include "UnityCG.cginc"

			/** appdata
			*/
			struct appdata
			{
				float4 vertex	: POSITION;
				float2 uv		: TEXCOORD0;
				float4 color	: COLOR0;
			};

			/** v2d
			*/
			struct v2f
			{
				float2 uv		: TEXCOORD0;
				float4 vertex	: SV_POSITION;
				float4 color	: COLOR0;
			};

			/** _MainTex
			*/
			sampler2D _MainTex;
			float4 _MainTex_ST;

			/** mode
			*/
			float move;

			/** vert
			*/
			v2f vert(appdata a_appdata)
			{
				v2f t_ret;
				{
					float4 t_vertex = a_appdata.vertex;
					float t_add = saturate(sin(t_vertex.x * 0.03 - move));
					t_vertex.y += t_add * 20;

					fixed4 t_color = a_appdata.color;
					t_color.g = saturate(t_color.g + t_add * 0.2);

					t_ret.vertex = UnityObjectToClipPos(t_vertex);
					t_ret.uv = TRANSFORM_TEX(a_appdata.uv,_MainTex);
					t_ret.color = t_color;
				}
				return t_ret;
			}

			/** flag
			*/
			fixed4 frag(v2f a_v2f) : SV_Target
			{
				float t_alpha = tex2D(_MainTex,a_v2f.uv).a * a_v2f.color.a;
				if (t_alpha <= 0.0f){
					discard;
				}

				return fixed4(a_v2f.color.rgb,t_alpha);
			}

			ENDCG
		}
	}
}

