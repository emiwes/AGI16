using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spawnEnemy : NetworkBehaviour
{

    public GameObject enemyPrefab;
    [HideInInspector]
    public Transform target;
    //public GameObject pathPrefab;
    //public GameObject startPosition;
    //public GameObject spawObj;
	private bool isHost;
	private Transform VRPosition;

    // Use this for initialization
    void Start () {
        target = this.transform.Find("target");
		isHost = false;
		VRPosition = GameObject.Find ("[CameraRig]").transform;
        Debug.Log(VRPosition);
	}

	public void updateHostStatus (bool hostStatus) {
		isHost = hostStatus;
	}

	public void HPBarLookAtVRPlayerPos () {
		//Update all enemies so their health bars point to VR player
		Transform playerPos = GameObject.Find ("[CameraRig]").transform;
		VRPosition = playerPos;
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
			//Update target lookat for all current enemies
			enemy.transform.Find ("EnemyHPCanvas").GetComponent<CameraFacingBillboard> ().target = playerPos;
		}
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("space"))
        {
            spawnSingleEnemy();
        }

    }

    public void spawnSingleEnemy()
    {

        //Instatiate Enemy
        GameObject temp_enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity) as GameObject;
        //set path as a child to spawner Gameobject.
        temp_enemy.GetComponent<EnemyMovement>().target = target;
        temp_enemy.transform.SetParent(this.transform);
        //to spawn on all clients.
		//----without slider
		if (!isHost) { //If not VR-player, set hp inactive
			temp_enemy.transform.Find ("EnemyHPCanvas").gameObject.SetActive (false);
		} else {
            temp_enemy.transform.Find("EnemyHPCanvas").gameObject.SetActive(true);
            temp_enemy.transform.Find ("EnemyHPCanvas").GetComponent<CameraFacingBillboard> ().target = VRPosition;
		}
        NetworkServer.Spawn(temp_enemy);

    }
}
