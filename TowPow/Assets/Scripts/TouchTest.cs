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
				TouchBegin(point.Position, point.Tags);
			}
		}

		private void touchesEndedHandler(object sender, TouchEventArgs e) {
			Debug.Log ("End handler");
			foreach (var point in e.Touches) {
				TouchEnd (point.Position, point.Tags);
			}
		}

		//[ClientRpc]
		void TouchBegin(Vector2 position, Tags tags) {
			//Debug.Log ("tags: " + tags.ToString());
			//GameObject testObject = (GameObject)Instantiate (Prefab, transform.position, transform.rotation);
			//NetworkServer.Spawn (testObject);

			Vector3 spawnPosition = topCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, 0));
			spawnPosition.y = 6f;

			// Figure out what towertype we are dealing with
			GameObject towerPrefab = null;
			foreach(GameObject tp in towerTypes) {
				if(tags.HasTag(tp.tag)) {
					towerPrefab = tp;
					break;
				}
			}
			if(towerPrefab == null) {
				Debug.Log("The fiducial does not represent a tower");
				return;
			}
			
			// Check if the tower is already placed and get the reference.
			GameObject activeTower = null;
			foreach(GameObject tower in towers) {
				if(tags.HasTag(tower.tag)) {
					activeTower = tower;
				}
			}

			if (!isLocalPlayer){
            	return;
			}
			
			// Check if we found anything
			if(activeTower == null) {
				// The tower is not placed
				// Create and spawn the tower
				CmdInstantiateTower(towerPrefab, spawnPosition, Quaternion.identity);
			} else {
				// The tower is placed
				// Check if it's close to the last position
				if (Vector3.Distance (activeTower.transform.position, spawnPosition) < distanceThreshold) {
					// It's close
					activeTower.GetComponent<TowerSpawn>().StopDespawnTimer();
				} else {
					// It's a new position
					activeTower.GetComponent<TowerSpawn>().Despawn();
					towers.Remove(activeTower);
					CmdInstantiateTower(towerPrefab, spawnPosition, Quaternion.identity);
				}
			}
		}

		void TouchEnd(Vector2 position, Tags tags) {
			Debug.Log ("Despawn");

			// Get the tower reference
			GameObject activeTower = null;
			foreach(GameObject tower in towers) {
				if(tags.HasTag(tower.tag)) {
					activeTower = tower;
				}
			}

			if(activeTower == null) {
				Debug.Log("The tower that you tried to remove doesn't exist");
				return;
			}

			// Start despawntimer
			activeTower.GetComponent<TowerSpawn>().StartDespawnTimer();
		}

		[Command]
		void CmdInstantiateTower(GameObject prefab, Vector3 position, Quaternion rotation) {
			GameObject t = (GameObject)Instantiate(prefab, position, rotation);
			t.GetComponent<TowerSpawn> ().AddTowerController (this);
			towers.Add(t);
			NetworkServer.SpawnWithClientAuthority(t, connectionToClient);
		}

		public void DestroyMe(GameObject go, float time) {
			if(!towers.Remove(go)) {
				Debug.Log("The tower could not be removed");
				return;
			}

			StartCoroutine (DestroyTowerInSeconds (go, time));
		}

		IEnumerator DestroyTowerInSeconds(GameObject go, float time) {
			// Find reference in towers

			yield return new WaitForSeconds (time);
			NetworkServer.Destroy (go);
		}
	}
}