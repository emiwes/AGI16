using UnityEngine;
using System.Collections;

public class TowerSpawn : MonoBehaviour {

	public bool isActive;
	public float spawnTime;
	public float timeSinceSpawn;

	// Use this for initialization
	void Start () {
		isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			timeSinceSpawn += Time.deltaTime;
		}
	}

	public void SpawnAt(Vector3 position){
		if (!isActive) {
			transform.position = position;
			isActive = true;
			spawnTime = Time.time;
		}
		Debug.Log (position.ToString());
		Debug.Log ("SPawntime: " + spawnTime);

		if (EligibleForRespawn ()) {
			spawnTime = Time.time;
			timeSinceSpawn = 0;
			transform.position = position;
		}
	}

	bool EligibleForRespawn(){
		Debug.Log (timeSinceSpawn);
		if (timeSinceSpawn < 2) {
			Debug.Log ("time since spawn: " + timeSinceSpawn);
			// det har inte gått två sekunder sedan vi senast placerade den
			return false;
		} else{
			Debug.Log ("över 2sek sedan spawn");
			return true;
		};
	}

}