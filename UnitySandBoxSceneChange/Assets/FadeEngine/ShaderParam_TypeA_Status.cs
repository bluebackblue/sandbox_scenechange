

/** FadeEngine
*/
namespace FadeEngine
{
	/** ShaderParam_TypeA_Status
	*/
	public readonly struct ShaderParam_TypeA_Status
	{
		/** position
		*/
		public readonly UnityEngine.Vector2 position;

		/** uv_offset
		*/
		public readonly UnityEngine.Vector2 uv_offset;

		/** color
		*/
		public readonly UnityEngine.Vector4 color;

		/** constructor
		*/
		public ShaderParam_TypeA_Status(UnityEngine.Vector2 a_position,UnityEngine.Vector2 a_uv_offset,UnityEngine.Vector4 a_color)
		{
			//position
			this.position = a_position;

			//uv_offset
			this.uv_offset = a_uv_offset;

			//color
			this.color = a_color;
		}
	}
}

