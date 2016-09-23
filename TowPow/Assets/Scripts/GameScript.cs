using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

	public Text WaveNrText;

	public int creepsPerWave;

	private int waveNr = 0;

	public float spawnWaitTime;

	private bool waveIsRunning = false;

	public bool GameOver = false;


	IEnumerator RunWaves(float spawnWaitTime, int nrOfCreeps) {
		spawnCreeps creepSpawner = GameObject.Find ("spawner").GetComponent<spawnCreeps> ();
		for (int i = 0; i < nrOfCreeps; i++) {
			creepSpawner.spawnZombie ();
			yield return new WaitForSeconds (spawnWaitTime);
		}
		yield return true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!waveIsRunning && !GameOver) {
			//If no wave is running, spawn a new wave
			waveNr += 1;
			WaveNrText.text = waveNr.ToString();
			//Spawn more creatures based on waveNr
			int nrOfCreeps = creepsPerWave * waveNr;
			waveIsRunning = true;

			StartCoroutine (RunWaves(spawnWaitTime, nrOfCreeps));

			//spawnCreeps creepSpawner = GameObject.Find ("spawner").GetComponent<spawnCreeps> ();
			//for (int i = 0; i < nrOfCreeps; i++) {
			//	creepSpawner.spawnZombie ();
			//}

		} else {
			if (GameOver || GameObject.FindWithTag ("Enemy") == null) {
				//If no more enemies, stop wave
				waveIsRunning = false;
			}
		}
	}
}
