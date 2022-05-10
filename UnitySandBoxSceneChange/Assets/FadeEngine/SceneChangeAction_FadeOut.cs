

/** FadeEngine
*/
namespace FadeEngine
{
	/** SceneChangeAction_FadeOut
	*/
	public sealed class SceneChangeAction_FadeOut : BlueBack.Scene.ChangeAction_Base<SceneChangeAction_FadeOut.ID>
	{
		/** ID
		*/
		public enum ID{Default=0};

		/** Create
		*/
		public static BlueBack.Scene.ChangeAction_Item_Base Create()
		{
			return new BlueBack.Scene.ChangeAction_Item<SceneChangeAction_FadeOut.ID>(new SceneChangeAction_FadeOut(),ID.Default);
		}

		/** endflag
		*/
		private bool endflag;

		/** constructor
		*/
		public SceneChangeAction_FadeOut()
		{
		}

		/** [ChangeAction_Base<ID>]Change

			return == true : 完了。

		*/
		public void Change(SceneChangeAction_FadeOut.ID a_id,BlueBack.Scene.Scene a_scene)
		{
			this.endflag = false;
			Engine.Engine.GetInstance().monobehaviour.StartCoroutine(this.CoroutineMain());
		}

		/** [ChangeAction_Base<ID>]Action

			return == true : 完了。

		*/
		public bool Action(SceneChangeAction_FadeOut.ID a_id,BlueBack.Scene.Scene a_scene)
		{
			return this.endflag;
		}

		/** CoroutineMain
		*/
		public System.Collections.IEnumerator CoroutineMain()
		{
			Engine.Engine t_engine = Engine.Engine.GetInstance();

			//レンダリング待ち。
			yield return t_engine.wait_for_endofframe;

			//フェード経過時間。
			t_engine.fade.SetFadeMove(0.0f);

			//スクリーンショット。
			{
				UnityEngine.Texture2D t_texture = new UnityEngine.Texture2D(UnityEngine.Screen.width,UnityEngine.Screen.height,UnityEngine.TextureFormat.RGB24,false);
				t_texture.ReadPixels(new UnityEngine.Rect(0,0,t_texture.width,t_texture.height),0,0);
				t_texture.Apply();
				t_engine.fade.SetTexture(t_texture);
			}

			//最低表示時間。
			{
				float t_time = UnityEngine.Time.realtimeSinceStartup;
				float t_delta;
				do{
					t_delta = UnityEngine.Time.realtimeSinceStartup - t_time;
					yield return null;
				}while(t_delta <= 0.5f);
			}

			//次のシーンの起動。
			{
				if(t_engine.scene.loadscene_async != null){
					t_engine.scene.loadscene_async.allowSceneActivation = true;
					do{
						yield return null;
					}while(t_engine.scene.loadscene_async.isDone == false);
					t_engine.scene.loadscene_async = null;
				}
			}

			this.endflag = true;
			yield break;
		}
	}
}

