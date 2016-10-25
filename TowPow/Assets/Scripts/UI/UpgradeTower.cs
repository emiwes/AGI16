using UnityEngine;
using System.Collections;

public class UpgradeTower : MonoBehaviour {
	public GameObject tower;
	private TowerSpawn towerSpawn;
	private TowerCombat towerCombat;

	void Start () {
		towerSpawn = tower.GetComponent<TowerSpawn>();
		towerCombat = tower.GetComponent<TowerCombat>();
	}

	void Update () {

	}

	public void DestroyMe() {
		GameObject.Destroy (gameObject);
	}

	public void Upgrade(){
		towerCombat.LevelUp ();
	}


}
