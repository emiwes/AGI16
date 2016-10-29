using UnityEngine;
using System.Collections;

public class UpgradeTower : MonoBehaviour {
	public string tag;
	public GameObject gameHandler;
	private GameScript gameScript;
	private TowerLevelSynchronize towerLevelSynchronize;

	void Awake () {
		gameScript = gameHandler.GetComponent<GameScript>();
		towerLevelSynchronize = gameHandler.GetComponent<TowerLevelSynchronize> ();
	}

	public void Upgrade(){
		//Debug.Log ("Upgrade called!");
		int maxLevel = towerLevelSynchronize.levelInfo.Count;
		//Debug.Log ("Max level: " + maxLevel);
		int currentLevel = towerLevelSynchronize.towerLevelLookup [tag];
		//Debug.Log ("Current level: " + currentLevel);
		if (towerLevelSynchronize.towerLevelLookup[tag] == maxLevel) {
			return;
		}

		//Debug.Log ("Cost to upgrade: "+towerLevelSynchronize.levelInfo [currentLevel - 1].costToUpgrade);
		if (gameScript.moneyCounter >= towerLevelSynchronize.levelInfo[currentLevel - 1].costToUpgrade) {
			towerLevelSynchronize.LevelUp (tag);
		} 
	}
}
