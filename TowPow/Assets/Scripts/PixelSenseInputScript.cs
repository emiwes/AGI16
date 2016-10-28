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
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Tags blue = new Tags("black");
                    TouchBegin(Input.mousePosition, blue);
                    //CmdInstantiateTower("blue", hit.point, Quaternion.identity);
                    //if (Input.GetMouseButtonUp(0))
                    //{
                    //    TouchEnd(Input.mousePosition, blue);
                    //}
                }
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
			Debug.Log ("Touch Start");

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
                    activeTower.GetComponent<TowerSpawn>().StopDespawnTimer();
                }
                else
                {
                    // It's a new position
                    activeTower.GetComponent<TowerSpawn>().Despawn();
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
						tower.GetComponent<TowerSpawn> ().StartDespawnTimer ();
					}
				}
			}
		}

        //Public Functions
        public void DestroyMe(NetworkInstanceId id, float time)
        {
            StartCoroutine(DestroyTowerInSeconds(id, time));
        }
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
            if (tags.HasTag("Touch"))// || tags.HasTag("Mouse"))
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
        IEnumerator DestroyTowerInSeconds(NetworkInstanceId goId, float time)
        {
            // Find reference in towers

            yield return new WaitForSeconds(time);

            if (!goId.IsEmpty())
            {
                CmdDestroyTowerByNetId(goId);
            }
        }

        //Commands
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
		[Command]
		void CmdDestroyTowerByNetId(NetworkInstanceId networkId) {
			NetworkServer.Destroy (NetworkServer.FindLocalObject (networkId));
		}
	}
}