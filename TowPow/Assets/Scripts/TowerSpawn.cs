using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TowerSpawn : NetworkBehaviour {

	public bool isActive;
	public float spawnDuration = 2f;
	public GameObject shootingRadiusIndicator;
	public GameObject circleProgressPrefab;
	public GameObject upgradeButtonPrefab;
	private Camera topCamera;

	[SyncVar]
	public bool despawning = false;
	private float despawnTimer = 0f;
	private float despawnTime = 1.0f;

	private GameObject buildProgress;
	private GameObject upgradeButton;
	private bool isBuildingTower = false;

	private float serverDespawnTime = 2f;

	private IEnumerator fillBuildProgressEnumerator;
	private IEnumerator buildTowerOverTimeEnumerator;

	private TouchScript.TouchTest touchTest;

	void Start () {
		isActive = false;
		touchTest = FindObjectOfType<TouchScript.TouchTest> ();

        if (!DeterminePlayerType.isVive){
            topCamera = GameObject.FindGameObjectWithTag("TopCamera").GetComponent<Camera>();
        }

		Spawn ();
	}

	void Update () {
		if(despawning) {
			despawnTimer += Time.deltaTime;
			if(despawnTimer > despawnTime) {
				despawning = false;
				despawnTimer = 0;
				Despawn();
			}
		}
	}


	public void StartDespawnTimer() {
		despawning = true;
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
			if (!DeterminePlayerType.isVive){
				StopCoroutine(fillBuildProgressEnumerator);
			}
		}

		
		Vector3 endPoint = new Vector3(transform.position.x, transform.position.y - 11, transform.position.z);
		// Vector3 endPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		
		// Start the despawning animation

		StartCoroutine(MoveOverSeconds(endPoint, spawnDuration));

        if (!DeterminePlayerType.isVive)
        {
            StartCoroutine(FillBuildProgress(spawnDuration, buildProgress.GetComponent<Image>().color, Color.red, buildProgress.GetComponent<Image>().fillAmount, 0f));
        }

		touchTest.DestroyMe (GetComponent<NetworkIdentity> ().netId, serverDespawnTime);

		//Destroy buildProgress
		Destroy(buildProgress, serverDespawnTime);
	}

	void Spawn() {
		Vector3 endPoint = transform.position;

		transform.position = new Vector3(transform.position.x, transform.position.y - 11, transform.position.z);
		// transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		buildTowerOverTimeEnumerator = MoveOverSeconds (endPoint, spawnDuration);
		StartCoroutine(buildTowerOverTimeEnumerator);
		//SPAWN THE TOWER WITH PROGRESS
		isBuildingTower = true;

		Vector3 pos = gameObject.transform.position;
//		pos.y = pos.y + 1.5f;
		// GameObject indicator = (GameObject)Instantiate(shootingRadiusIndicator, pos, Quaternion.identity);
		// indicator.transform.parent = gameObject.transform;

        if (!DeterminePlayerType.isVive){
            buildProgress = (GameObject)Instantiate(circleProgressPrefab, topCamera.WorldToScreenPoint(endPoint), Quaternion.identity);
            buildProgress.transform.SetParent(GameObject.Find("HUDCanvas").transform);
        }

        //buildProgress.transform.position = topCamera.WorldToScreenPoint(endPoint);
        if (!DeterminePlayerType.isVive)
        {
            fillBuildProgressEnumerator = FillBuildProgress(spawnDuration, Color.red, Color.green, 0f, 1f);
            StartCoroutine(fillBuildProgressEnumerator);
        }

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
			if(image == null){
				break;
			}
			image.fillAmount =  Mathf.Lerp(startValue, endValue, elapsedTime/time);
			image.color = Color.Lerp (startColor, endColor, (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		if(image != null){
			image.color = endColor;
			image.fillAmount = endValue;
		}
		isBuildingTower = false;

		// Attach upgrade button now that the tower has spawned
		AttachUpgradeButtonToTower();
	}

	void AttachUpgradeButtonToTower(){
		Vector3 upgradeButtonPos = topCamera.WorldToScreenPoint(gameObject.transform.position);
		upgradeButtonPos.y -= 30;
		upgradeButton = (GameObject)Instantiate(upgradeButtonPrefab, upgradeButtonPos, Quaternion.identity);
		upgradeButton.transform.SetParent(GameObject.Find("HUDCanvas").transform);

		upgradeButton.GetComponent<UpgradeTower>().tower = gameObject;
	}
//
//	public void AddTowerController(TouchScript.TouchTest tt) {
//		touchTest = tt;
//	}

}