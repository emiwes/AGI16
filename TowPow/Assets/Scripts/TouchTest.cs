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

		//public List<NetworkInstanceId> towerIds;
		public List<GameObject> towerTypes;
		public List<GameObject> towers;

		void Start(){
			// if (isServer) {
				towerTypes.Add(RedPrefab);
				towerTypes.Add(WhitePrefab);
				towerTypes.Add(BluePrefab);
				towerTypes.Add(BlackPrefab);
			// }
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
			string towerTag = null;
			foreach(GameObject tp in towerTypes) {
				if(tags.HasTag(tp.tag)) {
					towerTag = tp.tag;
					break;
				}
			}
			if(towerTag == null) {
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

			// Debug.Log("Towerprefab: " + towerPrefab.ToString());
			
			// Check if we found anything
			if(activeTower == null) {
				// The tower is not placed
				// Create and spawn the tower
				CmdInstantiateTower(towerTag, spawnPosition, Quaternion.identity);
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
					CmdInstantiateTower(towerTag, spawnPosition, Quaternion.identity);
				}
			}
		}

		void TouchEnd(Vector2 position, Tags tags) {
			Debug.Log ("Despawn");

			// Get the tower reference
			GameObject activeTower = null;
			foreach(GameObject tower in towers) {
				Debug.Log ("tower in towers is: " + tower.tag);
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
		void CmdInstantiateTower(string tag, Vector3 position, Quaternion rotation) {
			// Figure out what towertype we are dealing with
			GameObject towerPrefab = null;

			
			foreach(GameObject tp in towerTypes) {
				if(tp.tag == tag) {
					towerPrefab = tp;
					break;
				}
			}
			if(towerPrefab == null) {
				Debug.Log("The fiducial does not represent a tower");
				return;
			}
			Debug.Log("Towerprefab vi fick in: " + towerPrefab.ToString());
			GameObject t = (GameObject)Instantiate(towerPrefab, position, rotation);
			t.GetComponent<TowerSpawn> ().AddTowerController (this);
			towers.Add(t);
			Debug.Log("Ska spawna torn på server");
			NetworkServer.Spawn(t);
			NetworkInstanceId netId = t.GetComponent<NetworkIdentity> ().netId;
			RpcAddTowersToClients (netId);
		}

		//[Command]
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
			//NetworkServer.Destroy (go);
			Debug.Log("Ready to destroy");

			CmdDestroyTowerByNetId(go.GetComponent<NetworkIdentity>().netId);
		}

		[Command]
		void CmdDestroyTowerByNetId(NetworkInstanceId networkId) {
			NetworkServer.Destroy (NetworkServer.FindLocalObject (networkId));
			RpcRemoveTowersToClients (networkId);
		}

		[ClientRpc]
		void RpcAddTowersToClients(NetworkInstanceId netId) {
			if(netId.IsEmpty()){
				Debug.Log("Tornet vi ska lägga till är null");
			} else{
				Debug.Log("Lägger till torn i towers: " + NetworkServer.FindLocalObject (netId).ToString());
				towers.Add (NetworkServer.FindLocalObject (netId));
			}
		}

		[ClientRpc]
		void RpcRemoveTowersToClients(NetworkInstanceId netId) {
			Debug.Log("Tar bort torn i towers: " + NetworkServer.FindLocalObject (netId).ToString());			
			towers.Remove (NetworkServer.FindLocalObject (netId));
		}
	}
}