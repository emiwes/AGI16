using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerTriggerActions : MonoBehaviour {
	GameObject tower;
	TowerCombat towerCombat;

	void Start() {
		tower = gameObject.transform.parent.gameObject;
		towerCombat = tower.GetComponent<TowerCombat> ();
	}

	void OnTriggerEnter (Collider other){
        if ((!towerCombat.nearbyEnemies.Contains(other.gameObject)) && (other.gameObject.tag == "Enemy"))
        {
            towerCombat.addNearbyEnemy(other.gameObject);
        }
	}

	void OnTriggerStay(Collider other) {
		towerCombat.updateClosestEnemy ();
	}

	void OnTriggerExit(Collider other) {
		if(towerCombat.nearbyEnemies.Contains(other.gameObject))
			towerCombat.nearbyEnemies.Remove(other.gameObject);

		towerCombat.checkLastLeave ();
	}
}
