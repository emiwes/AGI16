﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

	public Text WaveNrText;

	public int creepsPerWave;

	private int waveNr = 0;

	public float spawnWaitTime;

	private bool waveIsRunning = false;
	public bool GameOver = false;

    //public bool GameStarted = false;
    public bool isHost = false;


    IEnumerator RunWaves(float spawnWaitTime, int nrOfCreeps) {
		spawnEnemy enemySpawner = GameObject.Find ("spawner").GetComponent<spawnEnemy> ();
        Debug.Log("run wave!!");
		for (int i = 0; i < nrOfCreeps; i++) {
			enemySpawner.spawnSingleEnemy();
			yield return new WaitForSeconds (spawnWaitTime);
		}
		yield return true;
	}
	
	// Update is called once per frame
	void Update () {
        /*describing the if
         * if host - only the host should spawn
         * if a wave is not running spawn
         * and not gameOver
         */
        
		if (isHost && !waveIsRunning && !GameOver) {
			//If no wave is running, spawn a new wave
			waveNr += 1;
			WaveNrText.text = waveNr.ToString();
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
}
