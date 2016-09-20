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

		public float distanceThreshold;

		private TowerSpawn blueTower;
		private TowerSpawn redTower;
		private TowerSpawn whiteTower;
		private TowerSpawn blackTower;

		public Camera topCamera;

		public List<GameObject> towers;
		public List<GameObject> towerTypes;

		void Start(){
			if (isServer) {
				towerTypes.Add(RedPrefab);
				towerTypes.Add(WhitePrefab);
				towerTypes.Add(BluePrefab);
				towerTypes.Add(BlackPrefab);

				var i = 0;
				foreach (GameObject tower in towerTypes) {
					GameObject t = (GameObject) Instantiate(tower, new Vector3(i, -3f, 0), Quaternion.identity);
					NetworkServer.Spawn (t);
					towers.Add (t);
					i += 2;
				}
			}
		}

		private void OnEnable()
		{
			Debug.Log ("start");
			if (TouchManager.Instance != null)
			{
				TouchManager.Instance.TouchesBegan += touchesBeganHandler;
				TouchManager.Instance.TouchesEnded += touchesEndedHandler;
			}
		}

		private void OnDisable()
		{
			if (TouchManager.Instance != null)
			{
				TouchManager.Instance.TouchesBegan -= touchesBeganHandler;
				TouchManager.Instance.TouchesEnded -= touchesEndedHandler;
			}
		}

		private void touchesBeganHandler(object sender, TouchEventArgs e)
		{
			
			foreach (var point in e.Touches) {
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

		private void touchesEndedHandler(object sender, TouchEventArgs e) {
			Debug.Log ("End handler");
			foreach (var point in e.Touches) {
				Despawn (point.Position, point.Tags);
			}
		}

//		bool isActiveTower(GameObject tower){
//			if (!activeTowers.Contains (tower)) {
//				activeTowers.Add (tower);
//				return false;
//			} else {
//				return true;
//			}
//		}

//		void activateTower (string tower){
//			activeTowers.Add (tower);
//		}

		//[ClientRpc]
		void Spawn(Vector2 position, Tags tags) {
			//Debug.Log ("tags: " + tags.ToString());
			//GameObject testObject = (GameObject)Instantiate (Prefab, transform.position, transform.rotation);
			//NetworkServer.Spawn (testObject);

			Vector3 spawnPosition = topCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, 0));
			spawnPosition.y = 0f;
			Quaternion spawnRotation = transform.rotation;

			foreach (GameObject tower in towers) {
				if(tags.HasTag(tower.tag)){
					if (Vector3.Distance (tower.transform.position, spawnPosition) > distanceThreshold) {
						Debug.Log("ska spawnas");
						tower.GetComponent<TowerSpawn>().Spawn (spawnPosition);
					}
				}
			}
		}

		void Despawn(Vector2 position, Tags tags) {
			Debug.Log ("Despawn");
			foreach (GameObject tower in towers) {
				if(tags.HasTag(tower.tag)){
					tower.GetComponent<TowerSpawn>().Despawn ();
				}
			}
		}
	}
}