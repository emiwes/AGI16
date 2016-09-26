using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TowerSpawn : MonoBehaviour {

	public bool isActive;
	public float spawnDuration = 2f;

	public bool despawning = false;
	private float despawnTimer;
	private float despawnTime = 1f;

	private TouchScript.TouchTest touchTest;

	// Use this for initialization
	void Start () {
		isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(despawning) {
			despawnTimer += Time.deltaTime;
			if(despawnTimer > despawnTime) {
				despawning = false;
				Despawn();
			}
		}
	}

	public void StartDespawnTimer() {
		despawning = true;
		despawnTimer = 0;
	}

	public void StopDespawnTimer() {
		despawning = false;
	}

	public void Despawn() {
		Debug.Log("Ready to despawn");
		Debug.Log(gameObject);
		Debug.Log (touchTest);
		touchTest.DestroyMe (GetComponent<NetworkIdentity> ().netId, 1f);
	}

	IEnumerator SpawnTimer() {
		yield return new WaitForSeconds(spawnDuration);
		isActive = true;
	}

	public void AddTowerController(TouchScript.TouchTest tt) {
		touchTest = tt;
	}
}