

/** Game
*/
namespace Game
{
	/** Move_MonoBehaviour
	*/
	public class Move_MonoBehaviour : UnityEngine.MonoBehaviour
	{
		/** Update
		*/
		private void Update()
		{
			this.gameObject.transform.position = new UnityEngine.Vector3(
				UnityEngine.Mathf.Sin(UnityEngine.Time.realtimeSinceStartup)*1,
				0.0f,
				UnityEngine.Mathf.Cos(UnityEngine.Time.realtimeSinceStartup)*1
			);

			this.gameObject.transform.rotation = UnityEngine.Quaternion.AngleAxis(
				UnityEngine.Time.realtimeSinceStartup * 100,
				new UnityEngine.Vector3(0.0f,1.0f,0.0f)
			);
		}
	}
}

