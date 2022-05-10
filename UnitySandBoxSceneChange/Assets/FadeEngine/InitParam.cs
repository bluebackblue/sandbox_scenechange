

/** FadeEngine
*/
namespace FadeEngine
{
	/** InitParam
	*/
	public sealed class InitParam
	{
		public UnityEngine.Material fade_material;
		public UnityEngine.Material loading_material;
		public UnityEngine.GameObject canvas_prefab;

		/** CreateDefault
		*/
		public static InitParam CreateDefault()
		{
			return new InitParam(){
				fade_material = null,
				loading_material = null,
				canvas_prefab = null,
			};
		}
	}
}

