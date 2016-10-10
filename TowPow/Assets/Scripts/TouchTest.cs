﻿using UnityEngine;
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

		private Camera topCamera;

		public List<GameObject> towerTypes;
		public List<GameObject> towers;

		void Start(){
			towerTypes.Add(RedPrefab);
			towerTypes.Add(WhitePrefab);
			towerTypes.Add(BluePrefab);
			towerTypes.Add (BlackPrefab);

			topCamera = GameObject.FindGameObjectWithTag ("TopCamera").GetComponent<Camera>();
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

		void TouchBegin(Vector2 position, Tags tags) {
			Debug.Log ("Touch Start");

			Vector3 spawnPosition = topCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f));
			spawnPosition.y = 10f;

			// Figure out what towertype we are dealing with
			string towerTag = null;
			foreach(GameObject tp in towerTypes) {
				//Debug.Log (tp.tag);
				if(tags.HasTag(tp.tag)) {
					towerTag = tp.tag;
					break;
				}
			}
			if(towerTag == null) {
				Debug.Log("The fiducial does not represent a tower");
				// CHECK FOR TOUCH INPUT
				if(tags.HasTag("Touch")){
					Vector3 down = new Vector3(0, -1, 0);

					RaycastHit hit;
					spawnPosition.y = 22f;
					Debug.DrawRay (spawnPosition, down);
					if(Physics.Raycast(spawnPosition, down, out hit, 10)) {
						Debug.Log (hit.transform.name + " was hit!");
						if (hit.transform.tag == "coin") {
							Destroy (hit.transform.gameObject);
						}
					}
				}


				return;
			}
			
			// Check if the tower is already placed and get the reference.
			// Get first active tower sorted by tags.
			GameObject[] activeTowers = GameObject.FindGameObjectsWithTag (towerTag);
			GameObject activeTower = null;

			if (activeTowers.Length > 0) {
				activeTower = activeTowers [0];
			}
				
			
			// Check if we found anything
			if(activeTower == null) {
				// The tower is not placed
				// Create and spawn the tower
				Debug.Log("instantiating tower to CMD");
				CmdInstantiateTower(towerTag, spawnPosition, Quaternion.identity);
			} else {
				// The tower is placed
				// Check if it's close to the last position
				if (Vector3.Distance (activeTower.transform.position, spawnPosition) < distanceThreshold) {
					// It's close
					activeTower.GetComponent<TowerSpawn>().StopDespawnTimer();
				} else {
					// It's a new position
					Debug.Log(activeTower);
					activeTower.GetComponent<TowerSpawn>().Despawn();
					CmdInstantiateTower(towerTag, spawnPosition, Quaternion.identity);
				}
			}
		}

		void TouchEnd(Vector2 position, Tags tags) {
			Debug.Log ("TouchEnd");

			string towerTag = null;
			foreach(GameObject tp in towerTypes) {
				if(tags.HasTag(tp.tag)) {
					towerTag = tp.tag;
					break;
				}
			}

			if (towerTag != null) {
				foreach (GameObject tower in GameObject.FindGameObjectsWithTag (towerTag)) {
					if (!tower.GetComponent<TowerSpawn> ().despawning) {
						tower.GetComponent<TowerSpawn> ().StartDespawnTimer ();
					}
				}
			}
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
			//Debug.Log("Towerprefab vi fick in: " + towerPrefab.ToString());
			GameObject t = (GameObject)Instantiate(towerPrefab, position, rotation);

			//Debug.Log("Ska spawna torn på server");
			NetworkServer.Spawn(t);
		}
			
		public void DestroyMe(NetworkInstanceId id, float time) {
			StartCoroutine (DestroyTowerInSeconds (id, time));
		}

		IEnumerator DestroyTowerInSeconds(NetworkInstanceId goId, float time) {
			// Find reference in towers

			yield return new WaitForSeconds (time);

			Debug.Log("Ready to destroy");

			if (!goId.IsEmpty()) {
				CmdDestroyTowerByNetId (goId);
			}
		}

		[Command]
		void CmdDestroyTowerByNetId(NetworkInstanceId networkId) {
			NetworkServer.Destroy (NetworkServer.FindLocalObject (networkId));
		}
	}
}