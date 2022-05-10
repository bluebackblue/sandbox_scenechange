

/** @brief シーン。
*/


/** Engine
*/
namespace Engine
{
	/** SceneExecute
	*/
	public sealed class SceneExecute
	{
		/** constructor
		*/
		public SceneExecute()
		{
			Engine.GetInstance().initialize++;
		}

		/** Boot
		*/
		public System.Collections.IEnumerator Boot(BlueBack.Scene.Scene_Base a_scene)
		{
			Engine t_engine = Engine.GetInstance();

			BlueBack.Scene.Scene t_scene = new BlueBack.Scene.Scene();
			t_engine.scene = t_scene;
			t_scene.SetNextScene(a_scene,null);

			t_engine.initialize--;
			yield break;
		}
	}
}

