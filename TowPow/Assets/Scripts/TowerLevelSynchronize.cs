using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TowerLevelSynchronize : NetworkBehaviour {

	//	[System.Serializable]
	public struct TowerLevelInfo {
		public int level;
		public int costToUpgrade;

		public TowerLevelInfo(int l, int c){
			level = l;
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
		levelInfo.Add (new TowerLevelInfo (1, 100));
		levelInfo.Add (new TowerLevelInfo (2, 200));
		levelInfo.Add (new TowerLevelInfo (3, 350));
		levelInfo.Add (new TowerLevelInfo (4, 500));

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
	}

	void UpdateUI(){
		if(levelInfo [towerLevelLookup ["red"] - 1].level == levelInfo.Count){
			redTowerUpgradeText.text = "-";
			redTowerLevelText.text = "MAX";
		} else{
			redTowerUpgradeText.text = levelInfo [towerLevelLookup ["red"] - 1].costToUpgrade.ToString () + "$";
			redTowerLevelText.text = towerLevelLookup ["red"].ToString ();
		}
		if(levelInfo [towerLevelLookup ["black"] - 1].level == levelInfo.Count){
			blackTowerUpgradeText.text = "-";
			blackTowerLevelText.text = "MAX";
		} else{
			blackTowerUpgradeText.text =levelInfo [towerLevelLookup ["black"] - 1].costToUpgrade.ToString () + "$";
			blackTowerLevelText.text = towerLevelLookup ["black"].ToString ();
		}
		if(levelInfo [towerLevelLookup ["blue"] - 1].level == levelInfo.Count){
			blueTowerUpgradeText.text = "-";
			blueTowerLevelText.text = "MAX";
		} else{
			blueTowerUpgradeText.text = levelInfo [towerLevelLookup ["blue"] - 1].costToUpgrade.ToString () + "$";
			blueTowerLevelText.text = towerLevelLookup ["blue"].ToString ();
		}
		if(levelInfo [towerLevelLookup ["white"] - 1].level == levelInfo.Count){
			whiteTowerUpgradeText.text = "-";
			whiteTowerLevelText.text = "MAX";
		} else{
			whiteTowerUpgradeText.text = levelInfo [towerLevelLookup ["white"] - 1].costToUpgrade.ToString () + "$";
			whiteTowerLevelText.text = towerLevelLookup ["white"].ToString ();
		}

	}
}
