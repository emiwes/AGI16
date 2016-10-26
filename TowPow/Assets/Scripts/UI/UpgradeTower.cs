using UnityEngine;
using System.Collections;

public class UpgradeTower : MonoBehaviour {
	public GameObject tower;
	private TowerSpawn towerSpawn;
	private TowerCombat towerCombat;
	public GameScript gameScript;

	void Start () {
		gameScript = GameObject.Find ("GameHandler").GetComponent<GameScript>();
		towerSpawn = tower.GetComponent<TowerSpawn>();
		towerCombat = tower.GetComponent<TowerCombat>();
	}

	void Update () {

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
