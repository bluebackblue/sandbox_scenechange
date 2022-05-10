

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
		public int layerindex;
		public float cameradepth;
		public int cell_w;
		public int cell_h;

		/** CreateDefault
		*/
		public static InitParam CreateDefault()
		{
			return new InitParam(){
				fade_material = null,
				loading_material = null,
				canvas_prefab = null,
				layerindex = 10,
				cameradepth = 10.0f,
				cell_w = 16,
				cell_h = 9,
			};
		}
	}
}

