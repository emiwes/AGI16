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

    private TerrainSurface terrainScript;

    private GameObject towerCanvas;
	private GameObject towerModel;
	private Camera topCamera;

    public bool validPlacement;
	public bool spawnedTower = false;
    private bool runningAlert;

	
	

	[SyncVar]
	public bool despawning = false;
	private float despawnTimer = 0f;
	private float despawnTime = 1.0f;

	private GameObject buildProgress;
    private GameObject towerPlacementAlert;

    private GameObject upgradeButton;
	private bool isBuildingTower = false;

	private float serverDespawnTime = 2f;

	private IEnumerator fillBuildProgressEnumerator;
	private IEnumerator buildTowerOverTimeEnumerator;
	private IEnumerator attachButtonWhenTowerSpawned;
    //private IEnumerator alertBuildProgress;


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

		//get the model of the tower and set it to inactive
		towerModel = transform.FindChild("Model").gameObject;
		towerModel.SetActive (false);

        //instatiate canvas
        towerCanvas = new GameObject("towerCanvas");
        towerCanvas.layer = 5; //UI layer
        Canvas c = towerCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        towerCanvas.AddComponent<CanvasGroup>();
        CanvasScaler scaler = towerCanvas.AddComponent<CanvasScaler>();
        towerCanvas.AddComponent<GraphicRaycaster>();

		buildProgress = (GameObject)Instantiate(circleProgressPrefab, topCamera.WorldToScreenPoint(transform.position), Quaternion.identity);
		buildProgress.transform.SetParent(towerCanvas.transform);
		buildProgress.SetActive (false);

		towerPlacementAlert = (GameObject)Instantiate(circleProgressPrefab, topCamera.WorldToScreenPoint(transform.position), Quaternion.identity);
		towerPlacementAlert.transform.SetParent(towerCanvas.transform);
		towerPlacementAlert.SetActive (false);

        //set parent
        towerCanvas.transform.SetParent(gameObject.transform);



        //start spawn tower
        
        //Check if valid placement otherwise add UI element.
        //Debug.Log("Valid placement?!: " + terrainScript.validTowerPlacement(gameObject.transform.position));
        if (terrainScript.validTowerPlacement(transform.position))
        {
            validPlacement = true;
        }
        else
        {
            //Debug.Log("invalid Placement of tower");
            validPlacement = false;
            
        }
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

		if (!validPlacement) {
			towerModel.SetActive (false);
            if (spawnedTower)
            {
                Despawn(false);
            }
            startAlert();
		}

        else if (validPlacement && !spawnedTower)
        {
			spawnedTower = true;
			towerModel.SetActive (true);
			runningAlert = false;
			towerPlacementAlert.SetActive (false);

            //spawn new
            Spawn();
        }
       
	}

    private void startAlert()
    {
		if (!DeterminePlayerType.isVive && !runningAlert) //  && !spawnedTower
        {
            runningAlert = true;
            towerPlacementAlert.transform.position = topCamera.WorldToScreenPoint(transform.position);
            towerPlacementAlert.SetActive(true);
            StartCoroutine(AlertBuildProgress(0.5f, Color.clear, Color.red));
        }
    }


	public void StartDespawnTimer() {
		despawning = true;
	}

	public void StopDespawnTimer() {
		despawning = false;
	}


	public void Despawn(){
		Despawn (true);
	}

	public void Despawn(bool remove) {
		isActive = false;

		if (spawnedTower) {
			//Stop all coroutines
			if (isBuildingTower) {
				isBuildingTower = false;
				StopCoroutine (buildTowerOverTimeEnumerator);
				if (!DeterminePlayerType.isVive) {
					StopCoroutine (fillBuildProgressEnumerator);
				}
			}

			Vector3 endPoint = new Vector3(transform.position.x, transform.position.y - 11, transform.position.z);
			StartCoroutine (MoveOverSeconds (endPoint, spawnDuration));

			if (!DeterminePlayerType.isVive) {
				StartCoroutine (FillBuildProgress (spawnDuration, buildProgress.GetComponent<Image> ().color, Color.red, buildProgress.GetComponent<Image> ().fillAmount, 0f));
			}

			if (remove) {
				touchTest.DestroyMe (GetComponent<NetworkIdentity> ().netId, serverDespawnTime);
			}
		} else {
			if (remove) {
				touchTest.DestroyMe (GetComponent<NetworkIdentity> ().netId, 0f);
			}
		}
		
	}

	void Spawn() {
		Vector3 endPoint = transform.position;


		transform.position = new Vector3(transform.position.x, transform.position.y - 11, transform.position.z);
		buildTowerOverTimeEnumerator = MoveOverSeconds (endPoint, spawnDuration);
		StartCoroutine(buildTowerOverTimeEnumerator);
		//SPAWN THE TOWER WITH PROGRESS
		isBuildingTower = true;

        
        if (!DeterminePlayerType.isVive)
        {
			buildProgress.transform.position = topCamera.WorldToScreenPoint (transform.position);
			buildProgress.SetActive (true);
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

	}

    IEnumerator AlertBuildProgress(float blinkPeriod, Color startColor, Color endColor)
    {
        Image image = towerPlacementAlert.GetComponent<Image>();
        image.fillAmount = 1;
        
        float elapsedTime = 0f;
        while (!validPlacement)
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
		towerPlacementAlert.SetActive (false);
    }


    public void moveAlertTo(Vector3 newPos)
    {
		towerPlacementAlert.transform.position = topCamera.WorldToScreenPoint(newPos);
		transform.position = newPos;
    	
	}
}