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
		Debug.Log (other.gameObject.name+ " entered shooting radius of "+tower.name);
        if ((!towerCombat.nearbyEnemies.Contains(other.gameObject)) && (other.gameObject.tag == "Enemy"))
        {
            Debug.Log(other.gameObject.name + " has the tag Enemy and is not in the nearby enemies list");
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
