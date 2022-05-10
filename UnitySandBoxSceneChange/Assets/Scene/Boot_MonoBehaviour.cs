

/** @brief ブート。
*/


/** Scene
*/
namespace Scene
{
	/** Boot_MonoBehaviour
	*/
	public sealed class Boot_MonoBehaviour : UnityEngine.MonoBehaviour
	{
		/** Initialize
		*/
		[UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Initialize()
		{
			UnityEngine.GameObject t_gameobject = new UnityEngine.GameObject("boot");
			Boot_MonoBehaviour t_monobehaviour = t_gameobject.AddComponent<Boot_MonoBehaviour>();
			UnityEngine.GameObject.DontDestroyOnLoad(t_gameobject);
			Engine.Engine.CreateInstance(t_monobehaviour);
			t_monobehaviour.StartCoroutine(new Engine.SceneExecute().Boot(new Boot()));
		}

		/** OnDestroy
		*/
		private void OnDestroy()
		{
			Engine.Engine.DeleteInstance();
		}
	}
}

