using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class KeyboardInput : NetworkBehaviour {
	public GameObject spawnMe;
	
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")){
			SpawnSamplePrefab ();
		}
	}

//	[Client]
	void SpawnSamplePrefab(){
		GameObject spawnedObject = (GameObject)Instantiate (spawnMe, new Vector3 (0, 1, 0), Quaternion.identity);
		GameObject player = GameObject.Find ("Player(Clone)");
		Debug.Log (player);
		//player.GetComponent<PlayerSampleSpawn> ().CmdSpawnSample(spawnedObject);
	}


}
