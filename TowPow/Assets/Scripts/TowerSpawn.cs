using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TowerSpawn : MonoBehaviour {

	public bool isActive;
	public float spawnDuration = 2f;
	public GameObject circleProgressPrefab;
	private Camera topCamera;

	public bool despawning = false;
	private float despawnTimer;
	private float despawnTime = 0.5f;

	private GameObject buildProgress;
	private bool isBuildingTower = false;

	private float serverDespawnTime = 2f;

	private IEnumerator fillBuildProgressEnumerator;
	private IEnumerator buildTowerOverTimeEnumerator;

	private TouchScript.TouchTest touchTest;

	// Use this for initialization
	void Start () {
		isActive = false;
		touchTest = FindObjectOfType<TouchScript.TouchTest> ();
		topCamera = GameObject.FindGameObjectWithTag ("TopCamera").GetComponent<Camera>();
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
		//isBuildingTower = false;
		despawning = true;
		despawnTimer = 0;
	}

	public void StopDespawnTimer() {
		despawning = false;
	}

	public void Despawn() {
		isActive = false;
		//Stop all coroutines
		if(isBuildingTower) {
			isBuildingTower = false;
			StopCoroutine (buildTowerOverTimeEnumerator);
			StopCoroutine (fillBuildProgressEnumerator);
		}
		Vector3 endPoint = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
		StartCoroutine(MoveOverSeconds(endPoint, spawnDuration));
		StartCoroutine(FillBuildProgress(spawnDuration, buildProgress.GetComponent<Image>().color, Color.red, buildProgress.GetComponent<Image>().fillAmount, 0f));
		touchTest.DestroyMe (GetComponent<NetworkIdentity> ().netId, serverDespawnTime);
		//Destroy buildProgress
		Destroy(buildProgress, serverDespawnTime);
	}

	void Spawn() {
		Vector3 endPoint = transform.position;
		transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
		buildTowerOverTimeEnumerator = MoveOverSeconds (endPoint, spawnDuration);
		StartCoroutine(buildTowerOverTimeEnumerator);
		//SPAWN THE TOWER WITH PROGRESS
		isBuildingTower = true;
		buildProgress = (GameObject)Instantiate(circleProgressPrefab, topCamera.WorldToScreenPoint(endPoint), Quaternion.identity);
		buildProgress.transform.SetParent (GameObject.Find ("HUDCanvas").transform);
		//buildProgress.transform.position = topCamera.WorldToScreenPoint(endPoint);
		fillBuildProgressEnumerator = FillBuildProgress(spawnDuration, Color.red, Color.green, 0f, 1f);
		StartCoroutine(fillBuildProgressEnumerator);
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

	IEnumerator FillBuildProgress(float time, Color startColor, Color endColor, float startValue, float endValue) {
		Image image = buildProgress.GetComponent<Image> ();
		image.fillAmount = startValue;
		image.color = startColor;
		Debug.Log ("FillBuildProgress");
		float elapsedTime = 0f;
		while (elapsedTime < time) {
			image.fillAmount =  Mathf.Lerp(startValue, endValue, elapsedTime/time);
			image.color = Color.Lerp (startColor, endColor, (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		image.color = endColor;
		image.fillAmount = endValue;
		isBuildingTower = false;
	}

	public void AddTowerController(TouchScript.TouchTest tt) {
		touchTest = tt;
	}

	private void updateBuildProgress() {
		
	}
}