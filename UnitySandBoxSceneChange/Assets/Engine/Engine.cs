

/** @brief エンジン。
*/


/** Engine
*/
namespace Engine
{
	/** Engine
	*/
	public sealed class Engine
	{
		/** シングルトン。
		*/
		private static Engine instance = null;
		public static void CreateInstance(UnityEngine.MonoBehaviour a_monobehaviour)
		{
			Engine.instance = new Engine(a_monobehaviour);
		}
		public static Engine GetInstance()
		{
			return Engine.instance;
		}
		public static void DeleteInstance()
		{
			if(Engine.instance != null){
				Engine.instance.Dispose();
				Engine.instance = null;
			}
		}

		/** initialize
		*/
		public int initialize;

		/** monobehaviour
		*/
		public UnityEngine.MonoBehaviour monobehaviour;

		/** wait_for_endofframe
		*/
		public UnityEngine.WaitForEndOfFrame wait_for_endofframe;

		/** scene
		*/
		public BlueBack.Scene.Scene scene;

		/** fade
		*/
		public FadeEngine.FadeEngine fade;

		/** scene_list
		*/
		public BlueBack.Scene.Scene_Base[] scene_list;

		/** constructor
		*/
		public Engine(UnityEngine.MonoBehaviour a_monobehaviour)
		{
			this.initialize = 0;
			this.monobehaviour = a_monobehaviour;
			this.scene = null;
			this.fade = null;
			this.scene_list = null;
		}

		/** 破棄。
		*/
		public void Dispose()
		{
			//initialize
			this.initialize = 0;

			//monobehaviour
			this.monobehaviour = null;

			//wait_for_endofframe
			this.wait_for_endofframe = null;

			//scene
			if(this.scene != null){
				this.scene.Dispose();
				this.scene = null;
			}

			//fade
			if(this.fade != null){
				this.fade.Dispose();
				this.fade = null;
			}

			//scene_list
			this.scene_list = null;
		}
	}
}

