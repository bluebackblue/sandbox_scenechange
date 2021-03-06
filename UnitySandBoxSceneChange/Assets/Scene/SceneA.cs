

/** Scene
*/
namespace Scene
{
	/** SceneA
	*/
	public sealed class SceneA : BlueBack.Scene.Scene_Base
	{
		/** scene
		*/
		private string scene_name;
		private int scene_index;

		/** time
		*/
		private float time;
		private float time_max;

		/** fadein
		*/
		private bool fadein_flag;

		/** constructor
		*/
		public SceneA(string a_scene_name,int a_scene_index)
		{
			this.scene_name = a_scene_name;
			this.scene_index = a_scene_index;
		}

		/** [BlueBack.Scene.Scene_Base]ユニティーシーン名。取得。
		*/
		public string GetUnitySceneName()
		{
			return this.scene_name;
		}

		/** [BlueBack.Scene.Scene_Base]シーンインデックス。取得。

			任意の値。

		*/
		public int GetSceneIndex()
		{
			return this.scene_index;
		}

		/** [BlueBack.Scene.Scene_Base]シーン開始。

			a_prev_scene : 前のシーン。

		*/
		public void SceneStart(BlueBack.Scene.Scene_Base a_prev_scene)
		{
			Engine.Engine t_engine = Engine.Engine.GetInstance();

			//time
			this.time = UnityEngine.Time.realtimeSinceStartup;
			this.time_max = 4.0f;

			//フェードイン。
			this.fadein_flag = true;
			t_engine.fade.SceneChangeFadeInStart();
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
			Engine.Engine t_engine = Engine.Engine.GetInstance();

			//フェードイン。
			if(this.fadein_flag == true){
				this.fadein_flag = t_engine.fade.SceneChangeFadeInUpdate();
			}

			if(a_phase == BlueBack.Scene.PhaseType.Running){
				float t_delta = UnityEngine.Time.realtimeSinceStartup - this.time;
				if((t_delta >= this.time_max)&&(this.fadein_flag == false)){
					t_engine.scene.SetNextScene(
						t_engine.scene_list[Config.SceneB.Index],
						new BlueBack.Scene.ChangeAction_Item_Base[]{
							//シーンロードリクエスト。
							BlueBack.Scene.ChangeAction_SingleLoaRequestNextUnityScene.Create(false),
							//フェードアウト。
							FadeEngine.SceneChangeAction_FadeOut.Create(),
						}
					);
				}
			}else{
				//フェードアウト。
				t_engine.fade.SceneChangeFadeOutUpdate();
			}
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
	}
}

