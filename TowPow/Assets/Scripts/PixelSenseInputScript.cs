using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TouchScript
{
	public class PixelSenseInputScript : NetworkBehaviour {

        ////Public variables
        //Prefabs
		public GameObject RedPrefab;
		public GameObject WhitePrefab;
		public GameObject BluePrefab;
		public GameObject BlackPrefab;
		public GameObject BulletPrefab;
        //Lists
        public List<GameObject> towerTypes;
        public List<GameObject> towers;
        //Primitives
        public float distanceThreshold;

        ////Private variables
        //Scripts/Objects
        private TowerSpawn blueTower;
		private TowerSpawn redTower;
		private TowerSpawn whiteTower;
		private TowerSpawn blackTower;

		private Camera topCamera;
		private GraphicRaycaster hudCanvasRaycaster;
        
        /*
        Observe that Input handlers only forward if we are not Vive
        */
		

        void Update()
        {
            Tags black = new Tags("black");

            if (Input.GetKeyDown(KeyCode.D))
            {
                
                TouchBegin(Input.mousePosition, black);
                //CmdInstantiateTower("blue", hit.point, Quaternion.identity);
                
             
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                TouchEnd(Input.mousePosition, black);
            }

        }

		void Start(){
            //adds Towertypes
			towerTypes.Add(RedPrefab);
			towerTypes.Add(WhitePrefab);
			towerTypes.Add(BluePrefab);
			towerTypes.Add (BlackPrefab);

            if (!DeterminePlayerType.isVive) {
				topCamera = GameObject.FindGameObjectWithTag ("TopCamera").GetComponent<Camera>();
			}
            //Needed for coins
			hudCanvasRaycaster = GameObject.Find ("HUDCanvas").GetComponent<GraphicRaycaster>();
		}

		private void OnEnable()
		{
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

		private void touchesBeganHandler(object sender, TouchEventArgs e) {
			if (DeterminePlayerType.isVive) { 
				return; 
			}

			foreach (var point in e.Touches) {
				TouchBegin(point.Position, point.Tags);
			}
		}

		private void touchesEndedHandler(object sender, TouchEventArgs e) {
			if (DeterminePlayerType.isVive) { 
				return; 
			}

			foreach (var point in e.Touches) {
				TouchEnd (point.Position, point.Tags);
			}
		}

		void TouchBegin(Vector2 position, Tags tags) //Detects start of touches 
        {
			Debug.Log ("Touch Begin");

			Vector3 touchPositionInWorld = topCamera.ScreenToWorldPoint(position);
            touchPositionInWorld.y = 16f;

			// Figure out what towertype we are dealing with
			string towerTag = getTowerTag(tags);
			
			if(towerTag == null) {
                touchAndMouseInput(tags, position);
				return;
			}

            GameObject activeTower = getActiveTower(towerTag);

            // Check if we found anything
            if (activeTower == null)
            {
                // The tower is not placed
                // Create and spawn the tower
                CmdInstantiateTower(towerTag, touchPositionInWorld, Quaternion.identity);

            }
            else
            {
                // The tower is placed
                // Check if it's close to the last position
                if (Vector3.Distance(activeTower.transform.position, touchPositionInWorld) < distanceThreshold)
                {
                    // It's close
                    activeTower.GetComponent<TowerSpawn>().StopStartDespawnTimer();
                }
                else
                {
                    // It's a new position
                    //activeTower.GetComponent<TowerSpawn>().Despawn();
					CmdDespawn (activeTower);
                    CmdInstantiateTower(towerTag, touchPositionInWorld, Quaternion.identity);
                }
            }
		}

		void TouchEnd(Vector2 position, Tags tags) {
			Debug.Log ("TouchEnd");

			string towerTag = getTowerTag(tags);

			if (towerTag != null) {
                //despawn all towers of that type
				foreach (GameObject tower in GameObject.FindGameObjectsWithTag (towerTag)) {
					if (!tower.GetComponent<TowerSpawn> ().despawning) {
                        //CmdStartDespawning(tower);
						tower.GetComponent<TowerSpawn> ().StartDespawnTimer ();
					}
				}
			}
		}

        //Public Functions
        /*public void DestroyMe(NetworkInstanceId id, float time)
        {
            //StartCoroutine(DestroyTowerInSeconds(id, time));
            CmdDestroyTowerByNetId(id);
        }*/
        public int numbersOfActiveTowersWithTag(string tag)
        {
            int amount = 0;
            foreach (GameObject tower in GameObject.FindGameObjectsWithTag(tag))
            {
                if (!tower.GetComponent<TowerSpawn>().despawning)
                {
                    amount += 1;
                }
            }
            return amount;
        }

        //Private Functions
        private string getTowerTag(Tags tags)
        {
            string towerTag = null;
            foreach (GameObject tp in towerTypes)
            {
                if (tags.HasTag(tp.tag))
                {
                    towerTag = tp.tag;
                    break;
                }
            }
            return towerTag;
        }
        private void touchAndMouseInput(Tags tags, Vector2 position)
        {
            if (tags.HasTag("Touch") || tags.HasTag("Mouse"))
            {
                PointerEventData ped = new PointerEventData(null);
                ped.position = position;
                List<RaycastResult> results = new List<RaycastResult>();
                hudCanvasRaycaster.Raycast(ped, results);

                foreach (RaycastResult r in results)
                {
                    if (r.gameObject.name == "CoinSprite(Clone)")
                    {
                        r.gameObject.GetComponent<CoinClick>().DestroyCoin();
                        break;
                    }

                    else if (r.gameObject.tag == "upgradeButton")
                    {
                        r.gameObject.GetComponent<UpgradeTower>().Upgrade();
                    }
                }
            }
        }
        private GameObject getActiveTower(string towerTag)
        {
            // Check if the tower is already placed and get the reference.
            // Get first active tower sorted by tags.
            GameObject[] activeTowers = GameObject.FindGameObjectsWithTag(towerTag);
            GameObject activeTower = null;
            if (activeTowers.Length > 0)
            {
                activeTower = activeTowers[0];
            }
            return activeTower;
        }

        //Enumerators
		public IEnumerator DestroyTower(GameObject tower, float time)
        {
			Debug.Log ("Destroy Tower");
			yield return new WaitForSeconds(time);
    
			//Debug.Log (tower);
            if (tower)
            {
                CmdDestroyTower(tower);
            }
        }

        //Commands
        [Command]
		void CmdInstantiateTower(string tag, Vector3 position, Quaternion rotation) {
			// Figure out what towertype we are dealing with
			Debug.Log("CMDINSTANTIATE TOWER");
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
        [Command]
        public void CmdDespawn(GameObject tower)
        {
			Debug.Log("CMD DESPAWN TOWER");
            //tower.GetComponent<TowerSpawn>().StartDespawnTimer();
            if (tower)
            {
                tower.GetComponent<TowerSpawn>().Despawn();
            }
        }
        [Command]
		public void CmdDestroyTower(GameObject tower) {
			Debug.Log ("Cmd Destroy Tower");

			NetworkServer.Destroy (tower);
		}
	}
}