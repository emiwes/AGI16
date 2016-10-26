using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TowerSpawn : MonoBehaviour {

	public bool isActive;
	public float spawnDuration = 2f;
	public GameObject shootingRadiusIndicator;
	public GameObject circleProgressPrefab;
    public GameObject TowerCanvasPrefab;
    //public GameObject TowerPrefab;

    private TerrainSurface terrainScript;

    private GameObject towerCanvas;
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

    void Awake()
    {
        if (gameObject.transform.localScale != Vector3.one)
        {
            gameObject.transform.localScale = Vector3.one;
            Debug.LogWarning("You tried to change the scale on towers the wrong way.");
        }
    }

	void Start () {
		isActive = false;
		touchTest = FindObjectOfType<TouchScript.TouchTest> ();

        //get script on terrain
        terrainScript = GameObject.Find("Islands terrain").GetComponent<TerrainSurface>();

        if (!DeterminePlayerType.isVive){
            topCamera = GameObject.FindGameObjectWithTag("TopCamera").GetComponent<Camera>();
        }

        //instatiate canvas
        towerCanvas = new GameObject("towerCanvas");
        towerCanvas.layer = 5; //UI layer
        Canvas c = towerCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        towerCanvas.AddComponent<CanvasGroup>();
        CanvasScaler scaler = towerCanvas.AddComponent<CanvasScaler>();
        towerCanvas.AddComponent<GraphicRaycaster>();

        //set parent
        towerCanvas.transform.SetParent(gameObject.transform);

        //start spawn tower
        
        //Check if valid placement otherwise add UI element.
        //Debug.Log("Valid placement?!: " + terrainScript.validTowerPlacement(gameObject.transform.position));
        if (terrainScript.validTowerPlacement(transform.position))
        {
            Spawn();
        }
        else
        {
            //Debug.Log("invalid Placement of tower");
            if (!DeterminePlayerType.isVive)
            {
                buildProgress = (GameObject)Instantiate(circleProgressPrefab, topCamera.WorldToScreenPoint(transform.position), Quaternion.identity);
                buildProgress.transform.SetParent(towerCanvas.transform);

                StartCoroutine(AlertBuildProgress(0.5f, Color.clear, Color.red));
            }
            
        }
    }

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
		isActive = false;
		//Stop all coroutines
		if(isBuildingTower) {
			isBuildingTower = false;
			StopCoroutine (buildTowerOverTimeEnumerator);
            if (!DeterminePlayerType.isVive){
                StopCoroutine(fillBuildProgressEnumerator);
            }
		}

		// Vector3 endPoint = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
		Vector3 endPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		StartCoroutine(MoveOverSeconds(endPoint, spawnDuration));

        if (!DeterminePlayerType.isVive)
        {
            StartCoroutine(FillBuildProgress(spawnDuration, buildProgress.GetComponent<Image>().color, Color.red, buildProgress.GetComponent<Image>().fillAmount, 0f));
        }

		touchTest.DestroyMe (GetComponent<NetworkIdentity> ().netId, serverDespawnTime);

		//Destroy buildProgress Not needed destroys when tower destroys
		//Destroy(buildProgress, serverDespawnTime);
	}

	void Spawn() {
		Vector3 endPoint = transform.position;

//		transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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
            //buildProgress.transform.SetParent(GameObject.Find("HUDCanvas").transform);


            buildProgress.transform.SetParent(towerCanvas.transform);
            //buildProgress.transform.SetParent(GameObject.Find("HUDCanvas").transform);
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
        startColor = image.color;
        //image.color = startColor;
        endColor = (startColor == Color.red) ? Color.red : Color.clear;
		//Debug.Log ("FillBuildProgress");
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

    IEnumerator AlertBuildProgress(float blinkPeriod, Color startColor, Color endColor)
    {
        Image image = buildProgress.GetComponent<Image>();
        image.fillAmount = 1;
        
        float elapsedTime = 0f;
        while (true)
        {
            if (elapsedTime >= blinkPeriod)
                elapsedTime = 0f;

            if (elapsedTime <= (blinkPeriod/2))
                image.color = Color.Lerp(startColor, endColor, (elapsedTime / (blinkPeriod/2)));
            else
                image.color = Color.Lerp(endColor, startColor, ((elapsedTime- (blinkPeriod / 2)) / (blinkPeriod/2)));

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    //
    //	public void AddTowerController(TouchScript.TouchTest tt) {
    //		touchTest = tt;
    //	}

}