using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TowerLevelHandler : NetworkBehaviour {

	[SyncVar (hook = "RedLevelUp")]
	public int towerRedLevel = 1;

	[SyncVar (hook = "BlueLevelUp")]
	public int towerBlueLevel = 1;

	[SyncVar (hook = "WhiteLevelUp")]
	public int towerWhiteLevel = 1;

	[SyncVar (hook = "BlackLevelUp")]
	public int towerBlackLevel = 1;

	public void LevelUp(string towerName){
		if (towerName == "red") {
			towerRedLevel += 1;
		} else if (towerName == "blue") {
			towerBlueLevel += 1;
		} else if (towerName == "white") {
			towerWhiteLevel += 1;
		} else if (towerName == "black") {
			towerBlackLevel += 1;
		} else {
			Debug.Log ("Invalid tower upgrade name " + towerName);
		}
	}

	void RedLevelUp(int level) {
		towerRedLevel = level;
		SyncTowers ("red");
	}

	void BlueLevelUp(int level) {
		towerBlueLevel = level;
		SyncTowers ("blue");
	}
	void BlackLevelUp(int level) {
		towerBlackLevel = level;
		SyncTowers ("black");
	}
	void WhiteLevelUp(int level) {
		towerWhiteLevel = level;
		SyncTowers ("white");
	}

	void SyncTowers(string tag) {
		GameObject[] towers = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject tower in towers) {
			tower.GetComponent<TowerCombat> ().SyncTowerLevel ();
		}
	}

	public int GetLevel(string towerTag) {
		if (towerTag == "red") {
			return towerRedLevel;
		} else if (towerTag == "blue") {
			return towerBlueLevel;
		} else if (towerTag == "white") {
			return towerWhiteLevel;
		} else if (towerTag == "black") {
			return towerBlackLevel;
		} else {
			Debug.Log ("Couldn't get level for " + towerTag);
			return 0;
		}
	}
}
