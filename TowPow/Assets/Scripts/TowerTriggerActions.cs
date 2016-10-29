using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerTriggerActions : MonoBehaviour {
	TowerCombat towerCombat;

	void Start() {
		GameObject tower = gameObject.transform.parent.transform.parent.gameObject;
		towerCombat = tower.GetComponent<TowerCombat> ();
	}

	void OnTriggerEnter (Collider other){
		if ((other.gameObject.tag == "Enemy") && (!towerCombat.nearbyEnemies.Contains(other.gameObject)))
        {
            towerCombat.addNearbyEnemy(other.gameObject);
        }
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			towerCombat.updateClosestEnemy ();
		}
	}

	void OnTriggerExit(Collider other) {
		if(towerCombat.nearbyEnemies.Contains(other.gameObject))
			towerCombat.nearbyEnemies.Remove(other.gameObject);

		towerCombat.checkLastLeave ();
	}
}
