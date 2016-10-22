using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class spawnEnemy : NetworkBehaviour
{

    public GameObject enemyPrefab;
// <<<<<<< HEAD
    //[HideInInspector]
    //public List<Transform> targetPoints = new List<Transform>();
    public List<Transform> startPoints = new List<Transform>();
    //public GameObject pathPrefab;
    //public GameObject startPosition;
    //public GameObject spawObj;
    // private bool isHost;
// =======
   
	// [HideInInspector]
    // public Transform target;

// >>>>>>> refs/remotes/origin/develop
	private Transform VRPosition;

    void Start () {
// <<<<<<< HEAD
//         //target = this.transform.Find("target");
// 		isHost = false;
// =======
        // target = this.transform.Find("target");
// >>>>>>> refs/remotes/origin/develop
		VRPosition = GameObject.Find ("[CameraRig]").transform;
	}

	public void HPBarLookAtVRPlayerPos () {
		//Update all enemies so their health bars point to VR player
		Transform playerPos = GameObject.Find ("[CameraRig]").transform;
		VRPosition = playerPos;
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
			//Update target lookat for all current enemies
			enemy.transform.Find ("PirateHPCanvas").GetComponent<CameraFacingBillboard> ().target = playerPos;
		}
	}
	

	void Update () {
        if (Input.GetKeyDown("space"))
        {
            spawnSingleEnemy();
        }
    }

    public void spawnSingleEnemy(int spawnPoint)
    {
// <<<<<<< HEAD

        //Instatiate Enemy
        GameObject temp_enemy = Instantiate(enemyPrefab, startPoints[spawnPoint].position, Quaternion.identity) as GameObject;
        //set path as a child to spawner Gameobject.
        temp_enemy.GetComponent<EnemyMovement>().target = startPoints[spawnPoint];
// =======
//         GameObject temp_enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity) as GameObject;
        
// 		//set path as a child to spawner Gameobject.
//         temp_enemy.GetComponent<EnemyMovement>().target = target;
// >>>>>>> refs/remotes/origin/develop
        temp_enemy.transform.SetParent(this.transform);
        //to spawn on all clients.
		//----without slider
		if (!DeterminePlayerType.isVive) { //If not VR-player, set hp inactive
			temp_enemy.transform.Find ("PirateHPCanvas").gameObject.SetActive (false);
		} else {
			temp_enemy.transform.Find("PirateHPCanvas").gameObject.SetActive(true);
			temp_enemy.transform.Find ("PirateHPCanvas").GetComponent<CameraFacingBillboard> ().target = VRPosition;
		}
        NetworkServer.Spawn(temp_enemy);

    }

    public void spawnSingleEnemy()
    {
        int spawnPoint = Random.Range(0, startPoints.Count);//max exclusive for int, inclusive for floats
       // int targetPoint = Random.Range(0, targetPoints.Count);//max exclusive for int, inclusive for floats

        //Debug.Log("Spawn point randomized: "+ spawnPoint);
        spawnSingleEnemy(spawnPoint);
    }
}
