﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TowerSpawn : NetworkBehaviour {

    //objects/script references
    public GameObject shootingRadiusIndicator;
    public GameObject circleProgressPrefab;

    private TouchScript.PixelSenseInputScript PSInputScript;
    private TerrainSurface terrainScript;

    private Camera topCamera;

    private GameObject towerCanvas;
    private GameObject buildProgress;
    private GameObject towerPlacementIndicator;
    private GameObject physicalTower;

    //SyncVars
    [SyncVar]
    public bool despawning = false;


    //Public primitives
    public bool isActive = false;
    public float spawnDuration = 2f;
    public bool noActiveOfThatKind = false;
    public bool validPlacement = false;


    //Private Primitives
    private float despawnTimer;
	private float despawnTime = 0.5f;
    private bool isBuildingTower = false;
    private float serverDespawnTime = 2f;
    private bool NonValidPlacementIndicatorRunning = false;

    //Enumerators (who needs to be saved and accessed later
    private IEnumerator fillBuildProgressEnumerator;
	private IEnumerator buildTowerOverTimeEnumerator;


    void Awake()
    {
        if (gameObject.transform.localScale != Vector3.one)
        {
            gameObject.transform.localScale = Vector3.one;
            Debug.LogWarning("You tried to change the scale on towers the wrong way.");
        }
    }

    void Start () {
        //refernces to scripts
        PSInputScript = FindObjectOfType<TouchScript.PixelSenseInputScript>();
        terrainScript = GameObject.Find("Islands terrain").GetComponent<TerrainSurface>();
        physicalTower = transform.FindChild("Tower").gameObject;
        physicalTower.SetActive(false);

        if (!DeterminePlayerType.isVive)
        {
            topCamera = GameObject.FindGameObjectWithTag("TopCamera").GetComponent<Camera>();
        }

        //initial values 
        isActive = false;

        //setup function calls
        createTowerCanvas();

        //check intial Position

        validPlacement = terrainScript.validTowerPlacement(transform.position);
    }

    void Update () {

        if (!validPlacement)
        {
            if (!DeterminePlayerType.isVive && !NonValidPlacementIndicatorRunning)//start the indicator
            {
                towerPlacementIndicator.SetActive(true);
                StartCoroutine(NonValidPlacmentIndicator(0.5f, Color.clear, Color.red));

            }
        }

        if(validPlacement && (PSInputScript.numbersOfActiveTowersWithTag(gameObject.tag) == 0))
        {
            activateTower();
        }

		if(despawning) {
			despawnTimer += Time.deltaTime;
			if(despawnTimer > despawnTime) {
				despawning = false;
				Despawn();

			}
		}
	}

    //Public Functions
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

        PSInputScript.DestroyMe (GetComponent<NetworkIdentity> ().netId, serverDespawnTime);

		//Destroy buildProgress Not needed destroys when tower destroys
		//Destroy(buildProgress, serverDespawnTime);
	}
    public void removeTower()
    {

    }
    //Private Functions
    private void activateTower()
    {
        Debug.Log("Activate Tower");
        Vector3 endPoint = transform.position;

        transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
        physicalTower.SetActive(true);
        buildTowerOverTimeEnumerator = MoveOverSeconds(endPoint, spawnDuration);
        StartCoroutine(buildTowerOverTimeEnumerator);
        //SPAWN THE TOWER WITH PROGRESS
        isBuildingTower = true;

        if (!DeterminePlayerType.isVive)
        {
            //Start build-up tower
            fillBuildProgressEnumerator = FillBuildProgress(spawnDuration, Color.red, Color.green, 0f, 1f);
            StartCoroutine(fillBuildProgressEnumerator);
            buildProgress.SetActive(true);
        }

    }
    private void createTowerCanvas() //creates a canvas and content
    {
        towerCanvas = new GameObject("towerCanvas");
        towerCanvas.layer = 5; //UI layer
        Canvas c = towerCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        //towerCanvas.AddComponent<CanvasGroup>();
        //CanvasScaler scaler = towerCanvas.AddComponent<CanvasScaler>();
        towerCanvas.AddComponent<GraphicRaycaster>();

        //add components
        buildProgress = (GameObject)Instantiate(circleProgressPrefab, topCamera.WorldToScreenPoint(transform.position), Quaternion.identity);
        buildProgress.transform.SetParent(towerCanvas.transform);
        buildProgress.SetActive(false);

        towerPlacementIndicator = (GameObject)Instantiate(circleProgressPrefab, topCamera.WorldToScreenPoint(transform.position), Quaternion.identity);
        towerPlacementIndicator.transform.SetParent(towerCanvas.transform);
        towerPlacementIndicator.SetActive(false);


        //set parent
        towerCanvas.transform.SetParent(gameObject.transform);
    }

    //Enumerators
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
    IEnumerator NonValidPlacmentIndicator(float blinkPeriod, Color startColor, Color endColor)
    {
        Image image = towerPlacementIndicator.GetComponent<Image>();
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
        towerPlacementIndicator.SetActive(false);
    }


}