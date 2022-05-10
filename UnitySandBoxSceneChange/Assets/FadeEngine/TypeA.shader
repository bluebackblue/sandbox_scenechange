

/** @brief フェードエンジン。フェードＡ。
*/


Shader "FadeEngine/FadeA"
{
	Properties
	{
		/**  _MainTex
		*/
		_MainTex("_MainTex",2D) = "white"{}

		/** cell_w
		*/
		cell_w("cell_w",float) = 1.0

		/** cell_h
		*/
		cell_h("cell_h",float) = 1.0

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
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:setup

			#include "UnityCG.cginc"

			/** appdata
			*/
			struct appdata
			{
				/** position
				*/
				float4 position		: POSITION;

				/** uv
				*/
				float2 uv			: TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			/** v2f
			*/
			struct v2f
			{
				/** position
				*/
				float4 position		: SV_POSITION;

				/** uv
				*/
				float2 uv			: TEXCOORD0;

				/** color
				*/
				float4 color		: COLOR0;
			};

			#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)

			/** Status
			*/
			struct Status
			{
				/** position
				*/
				float2 position;

				/** uv_offset
				*/
				float2 uv_offset;

				/** color
				*/
				float4 color;
			};

			/** vertexbuffer
			*/
            StructuredBuffer<Status> status;

			#endif

			/** _MainTex
			*/
			sampler2D _MainTex;

			/** custom_matrix
			*/
			float4x4 custom_matrix;

			/** cell
			*/
			float cell_w;
			float cell_h;

			/** mode
			*/
			float move;

			/** setup
			*/
			void setup()
			{
			}

			/** vert
			*/
			v2f vert(appdata a_appdata)
			{
				UNITY_SETUP_INSTANCE_ID(a_appdata);

				v2f t_ret;

				#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
				{
					Status t_status = status[a_appdata.instanceID];

					float2 t_position = a_appdata.position.xy + t_status.position.xy;
					float2 t_uv = a_appdata.uv * float2(cell_w,cell_h) + t_status.uv_offset;
					t_uv.y = 1 - t_uv.y;

					float t_move_max = 1.0 / cell_w;

					float t_time = saturate((1.0 - t_status.uv_offset.x) + t_status.uv_offset.y * 0.1 - 2 + move) * 2;
					t_position += float2(-t_move_max * 0.1,t_move_max * 1) * pow(t_time,2);

					t_ret.position = mul(custom_matrix,float4(t_position.xy,0,1));
					t_ret.uv = t_uv;
					t_ret.color = status[a_appdata.instanceID].color;
				}
				#else
				{
					float4 t_position = a_appdata.position;
					t_ret.position = UnityObjectToClipPos(t_position);
					t_ret.uv = a_appdata.uv;
					t_ret.color = fixed4(0.0f,0.0f,0.0f,1.0f);
				}
				#endif

				return t_ret;
			}
			
			/** frag
			*/
			fixed4 frag(v2f a_v2f) : SV_Target
			{
				#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
				{
					fixed3 t_tex = tex2D(_MainTex,a_v2f.uv).rgb * a_v2f.color;
					
					fixed t_value = t_tex.r + t_tex.g + t_tex.b;
					t_value *= 0.1f;

					t_tex = fixed3(t_value,t_value,t_value);

					return fixed4(t_tex,a_v2f.color.a);
				}
				#else
				{
					return fixed4(0.0f,0.0f,0.0f,1.0f);
				}
				#endif
			}

			ENDCG
		}
	}
}

