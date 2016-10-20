using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class spawnEnemy : NetworkBehaviour
{

    public GameObject enemyPrefab;
   
	[HideInInspector]
    public Transform target;

	private Transform VRPosition;

    void Start () {
        target = this.transform.Find("target");
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

    public void spawnSingleEnemy()
    {
        GameObject temp_enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity) as GameObject;
        
		//set path as a child to spawner Gameobject.
        temp_enemy.GetComponent<EnemyMovement>().target = target;
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
}
