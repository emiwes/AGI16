using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TowerLevelSynchronize : NetworkBehaviour {

	//	[System.Serializable]
	public struct TowerLevelInfo {
		public int level;
		public float damage;
		//		public float range;
		public float speed;
		public int costToUpgrade;

		public TowerLevelInfo(int l, float d, float s, int c){
			level = l;
			damage = d;
			speed = s;
			costToUpgrade = c;
		}
	}

	public List<TowerLevelInfo> levelInfo = new List<TowerLevelInfo>();
	public Dictionary<string, int> towerLevelLookup = new Dictionary<string, int> ();

	public Text redTowerLevelText;
	public Text redTowerUpgradeText;

	public Text blueTowerLevelText;
	public Text blueTowerUpgradeText;

	public Text whiteTowerLevelText;
	public Text whiteTowerUpgradeText;

	public Text blackTowerLevelText;
	public Text blackTowerUpgradeText;

	[SyncVar (hook = "RedLevelUp")]
	public int towerRedLevel = 1;

	[SyncVar (hook = "BlueLevelUp")]
	public int towerBlueLevel = 1;

	[SyncVar (hook = "WhiteLevelUp")]
	public int towerWhiteLevel = 1;

	[SyncVar (hook = "BlackLevelUp")]
	public int towerBlackLevel = 1;

	void Start() {
		// Red Tower Levels
		levelInfo.Add (new TowerLevelInfo (1, 15f, 0.6f, 100));
		levelInfo.Add (new TowerLevelInfo (2, 20f, 0.5f, 200));
		levelInfo.Add (new TowerLevelInfo (3, 30f, 0.3f, 350));

		// Set up dictionary for retrieving levels
		towerLevelLookup.Add ("red", towerRedLevel);
		towerLevelLookup.Add ("blue", towerBlueLevel);
		towerLevelLookup.Add ("black", towerBlackLevel);
		towerLevelLookup.Add ("white", towerWhiteLevel);

		UpdateUI ();
	}

	public void LevelUp(string tag){
		GameObject localPlayer = ClientScene.FindLocalObject (GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId);
		localPlayer.GetComponent<CoinHandler> ().CmdDecrementMoney (levelInfo [towerLevelLookup [tag] - 1].costToUpgrade);
		localPlayer.GetComponent<TowerHandler> ().CmdLevelUp (tag);
	}

	void RedLevelUp(int level) {
		towerRedLevel = level;
		towerLevelLookup["red"] = level;
		SyncTowers ("red");
		UpdateUI ();
	}

	void BlueLevelUp(int level) {
		towerBlueLevel = level;
		towerLevelLookup["blue"] = level;
		SyncTowers ("blue");
		UpdateUI ();
	}
	void BlackLevelUp(int level) {
		towerBlackLevel = level;
		towerLevelLookup["black"] = level;
		SyncTowers ("black");
		UpdateUI ();
	}
	void WhiteLevelUp(int level) {
		towerWhiteLevel = level;
		towerLevelLookup["white"] = level;
		SyncTowers ("white");
		UpdateUI ();
	}

	void SyncTowers(string tag) {
		GameObject[] towers = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject tower in towers) {
			tower.GetComponent<TowerCombat> ().SyncTowerLevel ();
		}
	}

	public int GetLevel(string towerTag) {
		return towerLevelLookup [towerTag];
//		if (towerTag == "red") {
//			return towerRedLevel;
//		} else if (towerTag == "blue") {
//			return towerBlueLevel;
//		} else if (towerTag == "white") {
//			return towerWhiteLevel;
//		} else if (towerTag == "black") {
//			return towerBlackLevel;
//		} else {
//			Debug.Log ("Couldn't get level for " + towerTag);
//			return 0;
//		}
	}

	void UpdateUI(){
		redTowerUpgradeText.text = levelInfo [towerLevelLookup ["red"] - 1].costToUpgrade.ToString () + "$";
		blueTowerUpgradeText.text = levelInfo [towerLevelLookup ["blue"] - 1].costToUpgrade.ToString () + "$";
		blackTowerUpgradeText.text = levelInfo [towerLevelLookup ["black"] - 1].costToUpgrade.ToString () + "$";
		whiteTowerUpgradeText.text = levelInfo [towerLevelLookup ["white"] - 1].costToUpgrade.ToString () + "$";

		redTowerLevelText.text = towerLevelLookup ["red"].ToString ();
		blueTowerLevelText.text = towerLevelLookup ["blue"].ToString ();
		blackTowerLevelText.text = towerLevelLookup ["black"].ToString ();
		whiteTowerLevelText.text = towerLevelLookup ["white"].ToString ();
	}
}
