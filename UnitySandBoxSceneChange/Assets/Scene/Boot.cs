

/** Scene
*/
namespace Scene
{
	/** Boot
	*/
	public sealed class Boot : BlueBack.Scene.Scene_Base
	{
		/** constructor
		*/
		public Boot()
		{
		}

		/** [BlueBack.Scene.Scene_Base]シーン名。
		*/
		public string GetSceneName()
		{
			return null;
		}

		/** [BlueBack.Scene.Scene_Base]シーン開始。

			a_prev_scene : 前のシーン。

		*/
		public void SceneStart(BlueBack.Scene.Scene_Base a_prev_scene)
		{
			Engine.Engine.GetInstance().monobehaviour.StartCoroutine(this.CoroutineMain());
		}

		/** [BlueBack.Scene.Scene_Base]シーン終了。

			a_next_scene : 次のシーン。

		*/
		public void SceneEnd(BlueBack.Scene.Scene_Base a_next_scene)
		{
		}

		/** [BlueBack.Scene.Scene_Base]UnityUpdate
		*/
		public void UnityUpdate(BlueBack.Scene.PhaseType a_phase)
		{
		}

		/** [BlueBack.Scene.Scene_Base]UnityFixedUpdate
		*/
		public void UnityFixedUpdate(BlueBack.Scene.PhaseType a_phase)
		{
		}

		/** [BlueBack.Scene.Scene_Base]UnityLateUpdate
		*/
		public void UnityLateUpdate(BlueBack.Scene.PhaseType a_phase)
		{
		}

		/** CoroutineMain
		*/
		private System.Collections.IEnumerator CoroutineMain()
		{
			Engine.Engine t_engine = Engine.Engine.GetInstance();

			//wait_for_endofframe
			t_engine.wait_for_endofframe = new UnityEngine.WaitForEndOfFrame();

			//フェード。
			yield return new Engine.FadeExecute().Boot();

			//シーン。
			t_engine.scene_list = new BlueBack.Scene.Scene_Base[Config.SceneMax];
			for(int ii=0;ii<Config.SceneMax;ii++){
				switch(ii){
				case Config.SceneA.Index:		t_engine.scene_list[ii] = new SceneA();		break;
				case Config.SceneB.Index:		t_engine.scene_list[ii] = new SceneB();		break;
				default:
					{
						#if(UNITY_EDITOR)
						UnityEngine.Debug.LogError("error");
						#endif
					}break;
				}
			};

			//全初期化完了待ち。
			while(Engine.Engine.GetInstance().initialize > 0){
				yield return null;
			}

			//SetNextScene
			t_engine.scene.SetNextScene(
				t_engine.scene_list[Config.SceneA.Index],
				new BlueBack.Scene.ChangeAction_Item_Base[]{
					BlueBack.Scene.ChangeAction_SingleLoaRequestNextUnityScene.Create(true),
					BlueBack.Scene.ChangeAction_WaitActivationNextUnityScene.Create(5),
				}
			);

			yield break;
		}
	}
}

