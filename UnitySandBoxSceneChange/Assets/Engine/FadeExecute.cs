

/** @brief フェード。
*/


/** Engine
*/
namespace Engine
{
	/** FadeExecute
	*/
	public sealed class FadeExecute
	{
		/** constructor
		*/
		public FadeExecute()
		{
			Engine.GetInstance().initialize++;
		}

		/** Boot
		*/
		public System.Collections.IEnumerator Boot()
		{
			Engine t_engine = Engine.GetInstance();

			FadeEngine.InitParam t_initparam = FadeEngine.InitParam.CreateDefault();
			{
				t_initparam.fade_material = UnityEngine.Resources.Load<UnityEngine.Material>("FadeEngine/FadeA");
				t_initparam.loading_material = UnityEngine.Resources.Load<UnityEngine.Material>("FadeEngine/Loading");
				t_initparam.canvas_prefab = UnityEngine.Resources.Load<UnityEngine.GameObject>("FadeEngine/Canvas");

			}
			FadeEngine.FadeEngine t_fade = new FadeEngine.FadeEngine(in t_initparam);
			t_engine.fade = t_fade;
			t_engine.initialize--;
			yield break;
		}
	}
}

