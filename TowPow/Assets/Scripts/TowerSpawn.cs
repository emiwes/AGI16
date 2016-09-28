using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TowerSpawn : MonoBehaviour {

	public bool isActive;
	public float spawnDuration = 2f;
	public GameObject circleProgressPrefab;

	public bool despawning = false;
	private float despawnTimer;
	private float despawnTime = 1f;

	private GameObject buildProgress;
	private bool isBuildingTower = false;

	private TouchScript.TouchTest touchTest;

	// Use this for initialization
	void Start () {
		isActive = false;
		touchTest = FindObjectOfType<TouchScript.TouchTest> ();
		Spawn ();
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
		//Destroy buildProgress
		Destroy(buildProgress);
	}

	void Spawn() {
		Vector3 endPoint = transform.position;
		transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
		StartCoroutine(MoveOverSeconds(endPoint, spawnDuration));
		//SPAWN THE TOWER WITH PROGRESS
		isBuildingTower = true;
		buildProgress = GameObject.Instantiate(circleProgressPrefab);
		buildProgress.transform.SetParent (GameObject.Find ("PSCanvas").transform);
		buildProgress.transform.position = endPoint;
	}

	IEnumerator SpawnTimer() {
		yield return new WaitForSeconds(spawnDuration);
		isActive = true;
	}

	IEnumerator MoveOverSeconds(Vector3 endPoint, float time) {
		float elapsedTime = 0;
		Vector3 startingPos = transform.position;
		while (elapsedTime < time) {
			transform.position = Vector3.Lerp (startingPos, endPoint, (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		transform.position = endPoint;
		isActive = true;
	}

	public void AddTowerController(TouchScript.TouchTest tt) {
		touchTest = tt;
	}

	private void updateBuildProgress() {
		
	}
}