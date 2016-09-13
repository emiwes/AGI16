using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace TouchScript
{
	public class TouchTest : NetworkBehaviour {

		public GameObject Prefab;

		private void OnEnable()
		{
			Debug.Log ("start");
			if (TouchManager.Instance != null)
			{
				TouchManager.Instance.TouchesBegan += touchesBeganHandler;
			}
		}

		private void OnDisable()
		{
			if (TouchManager.Instance != null)
			{
				TouchManager.Instance.TouchesBegan -= touchesBeganHandler;
			}
		}

		private void touchesBeganHandler(object sender, TouchEventArgs e)
		{
			
			foreach (var point in e.Touches)
			{
				Debug.Log (point);
				Spawn(point.Position);
			}
		}

		//[ClientRpc]
		void Spawn(Vector2 position)
		{
			Debug.Log (Prefab);
			//GameObject testObject = (GameObject)Instantiate (Prefab, transform.position, transform.rotation);
			//NetworkServer.Spawn (testObject);

			var obj = Instantiate(Prefab) as GameObject;
			obj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10));
			obj.transform.rotation = transform.rotation;
			 NetworkServer.Spawn (obj);
		}


	}
}