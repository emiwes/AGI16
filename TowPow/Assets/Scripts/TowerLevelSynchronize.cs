using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TowerLevelSynchronize : NetworkBehaviour {

	[SyncVar (hook = "RedLevelUp")]
	public int towerRedLevel = 1;

	[SyncVar (hook = "BlueLevelUp")]
	public int towerBlueLevel = 1;

	[SyncVar (hook = "WhiteLevelUp")]
	public int towerWhiteLevel = 1;

	[SyncVar (hook = "BlackLevelUp")]
	public int towerBlackLevel = 1;

//	public void LevelUp(string towerName){
//		if (towerName == "red") {
//			towerRedLevel += 1;
//		} else if (towerName == "blue") {
//			towerBlueLevel += 1;
//		} else if (towerName == "white") {
//			towerWhiteLevel += 1;
//		} else if (towerName == "black") {
//			towerBlackLevel += 1;
//		} else {
//			Debug.Log ("Invalid tower upgrade name " + towerName);
//		}
//	}

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
		Debug.Log ("setting white level to: " + level);
		towerWhiteLevel = level;
		SyncTowers ("white");
	}

	void SyncTowers(string tag) {
		GameObject[] towers = GameObject.FindGameObjectsWithTag (tag);
		int level;
		foreach (GameObject tower in towers) {

			if (tag == "red") {
				 level = towerRedLevel;
			} else if (tag == "blue") {
				level = towerBlueLevel;
			} else if (tag == "white") {
				level = towerWhiteLevel;
			} else if (tag == "black") {
				level = towerBlackLevel;
			} else {
				level = 0;
				Debug.Log ("Couldn't get level for " + tag);
			}
			
			tower.GetComponent<TowerCombat> ().SetLevel (level);
		}
	}
}
