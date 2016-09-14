using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace TouchScript
{
	public class TouchTest : NetworkBehaviour {

		public GameObject RedPrefab;
		public GameObject WhitePrefab;
		public GameObject BluePrefab;
		public GameObject BlackPrefab;
		public GameObject BulletPrefab;
		ArrayList towerTypes = new ArrayList ();

		private List<string> activeTowers;

		void Start(){
			towerTypes.Add("red");
			towerTypes.Add("blue");
			towerTypes.Add("white");
			towerTypes.Add("black");
		}

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
//				Debug.Log ("pointHit: " + point.Hit);
//				Debug.Log ("pointID: " + point.Id);
//				Debug.Log ("pointSource: " + point.InputSource);
//				Debug.Log ("pointProps: " + point.Properties.Keys);
//				Debug.Log ("pointTags: " + point.Tags.HasTag("blue"));
//				Debug.Log ("pointTarget: " + point.Target);
//				Debug.Log ("sender: " + sender);
//				Debug.Log ("e: " + e);
				Spawn(point.Position, point.Tags);
			}
		}

		bool isActiveTower(string towerName){
			if (!activeTowers.Contains (towerName)) {
				activeTowers.Add (towerName);
				return false;
			} else {
				return true;
			}
		}

		void activateTower (string tower){
			activeTowers.Add (tower);
		}

		//[ClientRpc]
		void Spawn(Vector2 position, Tags tags)
		{
			//GameObject testObject = (GameObject)Instantiate (Prefab, transform.position, transform.rotation);
			//NetworkServer.Spawn (testObject);

			GameObject obj = null;

			if (tags.HasTag ("blue")) {
				obj = Instantiate (BluePrefab) as GameObject;
				if (!isActiveTower ("blue")) {
					Debug.Log ("Spawnar blue");
				}
			} else if (tags.HasTag ("white")) {
				obj = Instantiate (WhitePrefab) as GameObject;
				if (!isActiveTower ("white")) {
					Debug.Log ("Spawnar white");
				}
			} else if (tags.HasTag ("red")) {
				obj = Instantiate (RedPrefab) as GameObject;
				if (!isActiveTower ("red")) {
					Debug.Log ("Spawnar red");
				}
			} else if (tags.HasTag ("black")) {
				obj = Instantiate (BlackPrefab) as GameObject;
				if (!isActiveTower ("black")) {
					Debug.Log ("Spawnar black");
				}
			} else {
				obj = Instantiate (BulletPrefab) as GameObject;
			}

			if (obj) {
				obj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10));
				obj.transform.rotation = transform.rotation;
				NetworkServer.Spawn (obj);
			}
		}


	}
}