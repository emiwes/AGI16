using UnityEngine;
using System.Collections;

public class UpgradeTower : MonoBehaviour {
	public GameObject tower;
	private TowerCombat towerCombat;
	public GameScript gameScript;

	void Start () {
		gameScript = GameObject.Find ("GameHandler").GetComponent<GameScript>();
		towerCombat = tower.GetComponent<TowerCombat>();
	}

	public void DestroyMe() {
		GameObject.Destroy (gameObject);
	}

	public void Upgrade(){
		int maxLevel = towerCombat.levelInfo.Count;

		if (towerCombat.level == maxLevel) {
			return;
		}

		if (gameScript.moneyCounter >= towerCombat.levelInfo [towerCombat.level - 1].costToUpgrade) {
			
			towerCombat.LevelUp ();
		} 
	}


}
