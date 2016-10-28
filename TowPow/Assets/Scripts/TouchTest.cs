﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
		private GraphicRaycaster hudCanvasRaycaster;
        

		public List<GameObject> towerTypes;
		public List<GameObject> towers;

        private TerrainSurface terrainScript;

		void Start(){
			towerTypes.Add(RedPrefab);
			towerTypes.Add(WhitePrefab);
			towerTypes.Add(BluePrefab);
			towerTypes.Add (BlackPrefab);

            terrainScript = GameObject.Find("Islands terrain").GetComponent<TerrainSurface>();

            if (!DeterminePlayerType.isVive) {
				topCamera = GameObject.FindGameObjectWithTag ("TopCamera").GetComponent<Camera>();
			}
			hudCanvasRaycaster = GameObject.Find ("HUDCanvas").GetComponent<GraphicRaycaster>();
		}

		private void OnEnable()
		{
			if (TouchManager.Instance != null)
			{
				TouchManager.Instance.TouchesBegan += touchesBeganHandler;
				TouchManager.Instance.TouchesEnded += touchesEndedHandler;
                TouchManager.Instance.TouchesMoved += touchesMoveHandler;
			}
		}

		private void OnDisable()
		{
			if (TouchManager.Instance != null)
			{
				TouchManager.Instance.TouchesBegan -= touchesBeganHandler;
				TouchManager.Instance.TouchesEnded -= touchesEndedHandler;
				TouchManager.Instance.TouchesMoved -= touchesMoveHandler;
            }
		}

		private void touchesBeganHandler(object sender, TouchEventArgs e) {
			if (DeterminePlayerType.isVive || !isLocalPlayer) { 
				return;
			}
				

			foreach (var point in e.Touches) {
				TouchBegin(point.Position, point.Tags);
			}
		}

        private void touchesMoveHandler(object sender, TouchEventArgs e)
        {
            if (DeterminePlayerType.isVive || !isLocalPlayer)
            {
                return;
            }

            foreach (var point in e.Touches)
            {
                TouchMove(point.Position, point.Tags);
            }
        }


		private void touchesEndedHandler(object sender, TouchEventArgs e) {
			if (DeterminePlayerType.isVive || !isLocalPlayer) { 
				return; 
			}

			foreach (var point in e.Touches) {
				TouchEnd (point.Position, point.Tags);
			}
		}

		void TouchBegin(Vector2 position, Tags tags) {
			Debug.Log ("Touch Start");

			Vector3 spawnPosition = topCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, 16f));
			spawnPosition.y = 16f;

			// UGLY HACK FOR TESTING WITHOUT PIXELSENSE
			if (tags.HasTag("Mouse")) {
				if(Input.GetKey(KeyCode.D)) {
					tags = new Tags("red");
				}
			}

			// Figure out what towertype we are dealing with
			string towerTag = null;
			foreach(GameObject tp in towerTypes) {
				if(tags.HasTag(tp.tag)) {
					towerTag = tp.tag;
					break;
				}
			}

			if(towerTag == null) {
				if (tags.HasTag ("Touch") || tags.HasTag("Mouse")) {
					PointerEventData ped = new PointerEventData (null);
					ped.position = position;
					List<RaycastResult> results = new List<RaycastResult> ();
					hudCanvasRaycaster.Raycast (ped, results);

					foreach (RaycastResult r in results) {
						if (r.gameObject.name == "CoinSprite(Clone)") {
							r.gameObject.GetComponent<CoinClick> ().DestroyCoin ();
							break;
						}

						else if(r.gameObject.tag == "upgradeButton"){
							r.gameObject.GetComponent<UpgradeTower>().Upgrade();
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
					CmdInstantiateTower(towerTag, spawnPosition, Quaternion.identity);
				}
			}
				
		}

        void TouchMove(Vector2 position, Tags tags)
        {
            Vector3 spawnPosition = topCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f));
            spawnPosition.y = 16f;

            string towerTag = null;
            foreach (GameObject tp in towerTypes)
            {
                if (tags.HasTag(tp.tag))
                {
                    towerTag = tp.tag;
                    break;
                }
            }
            GameObject activeTower = GetActiveTower(towerTag);


            // Check if we found anything
            if (activeTower == null) {
					// The tower is not placed
					// Create and spawn the tower
					CmdInstantiateTower (towerTag, spawnPosition, Quaternion.identity);
			} else {
				// The tower is placed
				// Check if it's close to the last position
				TowerSpawn spawnScript = activeTower.GetComponent<TowerSpawn> ();



				if (Vector3.Distance (activeTower.transform.position, spawnPosition) < distanceThreshold) {
                    // It's close
                    spawnScript.StopDespawnTimer ();
				} else {
                    // It's a new position

                    //check if valid.
                    bool validPlace = terrainScript.validTowerPlacement(spawnPosition);
					//spawnScript.validPlacement = validPlace;
                    CmdUpdateValidatePlacement(activeTower, validPlace);


                    if (!validPlace){	
						spawnScript.moveAlertTo (spawnPosition);
					}
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
						CmdStartDespawning(tower);
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

            
            
            GameObject t = (GameObject)Instantiate(towerPrefab, position, rotation);
            NetworkServer.Spawn(t);
     		}
			
		public void DestroyMe(NetworkInstanceId id, float time) {
			StartCoroutine (DestroyTowerInSeconds (id, time));
		}

		IEnumerator DestroyTowerInSeconds(NetworkInstanceId goId, float time) {
			// Find reference in towers

			yield return new WaitForSeconds (time);

			if (!goId.IsEmpty()) {
				CmdDestroyTowerByNetId (goId);
			}
		}

		[Command]
		void CmdDestroyTowerByNetId(NetworkInstanceId networkId) {
			NetworkServer.Destroy (NetworkServer.FindLocalObject (networkId));
		}

        GameObject GetActiveTower(string towerTag)
        {
            if (towerTag == null) return null;
            GameObject[] activeTowers = GameObject.FindGameObjectsWithTag(towerTag);
            GameObject activeTower = null;

            if (activeTowers.Length > 0)
            {
                activeTower = activeTowers[0];
            }

            return activeTower;
            
        }

		[Command]
		void CmdStartDespawning(GameObject tower){
			tower.GetComponent<TowerSpawn> ().StartDespawnTimer ();
		}

        [Command]
        void CmdUpdateValidatePlacement(GameObject tower, bool value)
        {
            tower.GetComponent<TowerSpawn>().validPlacement = value;
        }

        [Command]
        public void CmdUpdateSpawnedTower(GameObject tower, bool value)
        {
            tower.GetComponent<TowerSpawn>().spawnedTower = value;
        }
    }
}