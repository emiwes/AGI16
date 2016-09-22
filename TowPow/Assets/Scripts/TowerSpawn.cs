using UnityEngine;
using System.Collections;

public class TowerSpawn : MonoBehaviour {

	public bool isActive;
	public float spawnDuration = 2f;

	private bool despawning = false;
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
		touchTest.DestroyMe (gameObject, 1);
	}

	IEnumerator SpawnTimer() {
		yield return new WaitForSeconds(spawnDuration);
		isActive = true;
	}

	public void AddTowerController(TouchScript.TouchTest tt) {
		touchTest = tt;
	}
}
