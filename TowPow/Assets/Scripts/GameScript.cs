using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameScript : NetworkBehaviour {

	public Text WaveNrText;
	public Text MoneyText;
	public Text KillText;

    public Slider HealthSlider;
    public Text HealthText;
    private spawnEnemy enemySpawner;

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

    [Space(10)]
    [Header("Arcade Mode")]
    public bool ArcadeModeStarted = false;
    public int ArcadeStartAmount = 5;
    public int ArcadeMultiplier = 2;

    void Awake() {
		PlayerHealth = PlayerStartingHealth;
        enemySpawner = GameObject.Find("spawner").GetComponent<spawnEnemy>();
    }
    

    IEnumerator RunWaves(float spawnWaitTime, int nrOfCreeps) {
		for (int i = 0; i < nrOfCreeps; i++) {
			enemySpawner.spawnSingleEnemy();
			yield return new WaitForSeconds (spawnWaitTime);
		}
		yield return true;
	}
	
	// Update is called once per frame
	void Update () {
        /*
       * Not a fan of the long following if but it works
       * TODO: Make prettier!
       */

        if (isServer && Input.GetKeyUp(KeyCode.A))
        {
            ArcadeModeStarted = true;
            ArcadeSpawn(ArcadeStartAmount);
        }
        else if (isServer && Input.GetKeyUp(KeyCode.S))

        {
            GameStarted = true;
        }
		else if (isServer && Input.GetKeyUp(KeyCode.M))
		{
            resetGame();
		}

        /*describing the if
       * if host - only the host should spawn
       * if a wave is not running spawn
       * and not gameOver
       */
		if (isServer && GameStarted && !waveIsRunning && !GameOver) {
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

    public void EnemyReachedGoal()
    {

        //GameScriptRef.PlayerHealth -= 1;
        if ( isServer && PlayerHealth > 0)
        {
            PlayerHealth -= 1;
            //Debug.Log("PlayerHealth: " + PlayerHealth);
            //update GUI
        }
    }
    
    private void resetGame()
    {
        Debug.Log("reset pressed");
        foreach (Transform enemy in GameObject.Find("spawner").gameObject.transform)
        {
            if (enemy.gameObject.name != "target")
            {
                //Destroy all child pirates
                Destroy(enemy.gameObject);
            }
        }
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("projectile"))
        {
            //Destroy all bullets as well
            Destroy(bullet);
        }
        //Reset game values
        GameStarted = false;
        ArcadeModeStarted = false;
        waveNr = 0;
        waveIsRunning = false;
        GameOver = false;
        killCounter = 0;
        moneyCounter = 0;
        PlayerHealth = PlayerStartingHealth;
    }

    void ArcadeSpawn(int newEnemies)
    {
        
        //spawnEnemy enemySpawner = GameObject.Find("spawner").GetComponent<spawnEnemy>();
        for (int i = 0; i < newEnemies; i++)
        {
            enemySpawner.spawnSingleEnemy();
        }
    }
    void OnWaveChange(int wave){
		WaveNrText.text = wave.ToString();
	}
	void OnKillChange(int kills){
		KillText.text = kills.ToString();
        if (ArcadeModeStarted)
        {
            //spawn new enemies.
            ArcadeSpawn(ArcadeMultiplier);
        }
	}
	void OnMoneyChange(int money){
		MoneyText.text = money.ToString();
	}

    void OnChangeHealth(int health)
    {
        HealthText.text = health.ToString();
        HealthSlider.value = health;
        if(health <= 0)
        {
            GameOver = true;
            HealthText.text = "GAME OVER";
        }
    }
}