using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class spawnEnemy : NetworkBehaviour
{

    public GameObject enemyPrefab;

    public List<Transform> startPoints = new List<Transform>();

	private Transform VRPosition;

    void Start () {
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
        if (Input.GetKeyDown("space")){
            spawnSingleEnemy();
        }
    }

    public void spawnSingleEnemy(int spawnPoint){
        //Instatiate Enemy
        GameObject temp_enemy = Instantiate(enemyPrefab, startPoints[spawnPoint].position, Quaternion.identity) as GameObject;
        //set path as a child to spawner Gameobject.
        temp_enemy.GetComponent<EnemyMovement>().target = startPoints[spawnPoint];

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

    public void spawnSingleEnemy(){
        int spawnPoint = Random.Range(0, startPoints.Count);//max exclusive for int, inclusive for floats
       // int targetPoint = Random.Range(0, targetPoints.Count);//max exclusive for int, inclusive for floats

        //Debug.Log("Spawn point randomized: "+ spawnPoint);
        spawnSingleEnemy(spawnPoint);
    }
}
