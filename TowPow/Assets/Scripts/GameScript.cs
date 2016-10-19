using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameScript : NetworkBehaviour {

	public Text WaveNrText;
	public Text MoneyText;
	public Text KillText;

	public int creepsPerWave;
	[SyncVar (hook = "OnWaveChange")]
	public int waveNr = 0;
	[SyncVar (hook = "OnKillChange")]
	public int killCounter = 0;
	[SyncVar (hook = "OnMoneyChange")]
	public int moneyCounter = 0;

	public int PlayerStartingHealth = 10;

	[HideInInspector]
    [SyncVar(hook = "OnChangeHealth")]
    public int PlayerHealth;

	public float spawnWaitTime;

	private bool waveIsRunning = false;
	public bool GameOver = false;

    public bool GameStarted = false;
    public bool isHost = false;


    IEnumerator RunWaves(float spawnWaitTime, int nrOfCreeps) {
		spawnEnemy enemySpawner = GameObject.Find ("spawner").GetComponent<spawnEnemy> ();
		for (int i = 0; i < nrOfCreeps; i++) {
			enemySpawner.spawnSingleEnemy();
			yield return new WaitForSeconds (spawnWaitTime);
		}
		yield return true;
	}

	void Awake() {
		PlayerHealth = PlayerStartingHealth;
	}
	
	// Update is called once per frame
	void Update () {
        /*
       * Not a fan of the long following if but it works
       * TODO: Make prettier!
       */

        if (isHost && Input.GetKeyUp(KeyCode.S))
        {
            GameStarted = true;
        }
		else if (isHost && Input.GetKeyUp(KeyCode.M))
		{
			foreach (Transform enemy in GameObject.Find("spawner").gameObject.transform) {
				if (enemy.gameObject.name != "target") {
					//Destroy all child pirates
					Destroy (enemy.gameObject);
				}
			}
			foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("projectile")) {
				//Destroy all bullets as well
				Destroy (bullet);
			}
			//Reset game values
			GameStarted = false;
			waveNr = 0;
			waveIsRunning = false;
			GameOver = false;
			killCounter = 0;
			moneyCounter = 0;
			PlayerHealth = PlayerStartingHealth;
			//Update GUI as well
			//GameObject.Find("target").GetComponent<OnEnemyReachGoal>().HealthSliderValue = PlayerStartingHealth;
		}

        /*describing the if
       * if host - only the host should spawn
       * if a wave is not running spawn
       * and not gameOver
       */

      
        if (isHost && GameStarted && !waveIsRunning && !GameOver) {
			//If no wave is running, spawn a new wave
			waveNr += 1;

			//Spawn more creatures based on waveNr
			int nrOfCreeps = creepsPerWave * waveNr;
			waveIsRunning = true;

			StartCoroutine (RunWaves(spawnWaitTime, nrOfCreeps));

		} else {
			if (GameOver || GameObject.FindWithTag ("Enemy") == null) {
				//If no more enemies, stop wave
				waveIsRunning = false;
			}
		}
	}

	void OnWaveChange(int wave){
		WaveNrText.text = wave.ToString();
	}
	void OnKillChange(int kills){
		KillText.text = kills.ToString();
	}
	void OnMoneyChange(int money){
		MoneyText.text = money.ToString();
	}

    void OnChangeHealth(int health)
    {
        //HealthSlider.value = health;
        //HealthText.text = health.ToString();
    }
}