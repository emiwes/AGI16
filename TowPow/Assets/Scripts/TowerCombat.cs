using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerCombat : MonoBehaviour {
	private float towerDamage = 10;
	public List<GameObject> nearbyEnemies = new List<GameObject>();
	GameObject closestEnemy = null;

	public GameObject bulletPrefab;
	public GameObject shootingRange;

	void Start() {
		InvokeRepeating ("fireAtClosestEnemy", 0.5f, 1.0f);
	}

	public void addNearbyEnemy(GameObject enemy){
		nearbyEnemies.Add (enemy);
	}

	public void removeNearbyEnemy(GameObject enemy){
		nearbyEnemies.Remove (enemy);
	}

	public void towerLookAt (Vector3 pos) {
		pos.y = 0;
		transform.LookAt (pos);
	}
		
	public void updateClosestEnemy(){
		// Check if any enemies have been killed since last iteration, remove null references. 
		checkForDead ();

		float minDist = Vector3.Distance(transform.position, nearbyEnemies[0].transform.position);
		GameObject closestInList = nearbyEnemies[0];

		foreach (GameObject enemy in nearbyEnemies){
			float enemyDistance = Vector3.Distance (transform.position, enemy.transform.position);

			if (enemyDistance <= minDist) {
				closestInList = enemy;
				minDist = enemyDistance;
			}
		}

		if ((closestEnemy == null) || (closestInList != closestEnemy)) {
			closestEnemy = closestInList;
		}

		if (nearbyEnemies.Count == 0)
			closestEnemy = null;

		towerLookAt (closestEnemy.transform.position);
	}

	public void checkLastLeave () {
		if (nearbyEnemies.Count == 0) {
			closestEnemy = null;
		}
	}

	void fireAtClosestEnemy() {
		if (closestEnemy == null)
			return;

		GameObject towerObject = GameObject.Find ("TowerObject");
		GameObject bullet = (GameObject)Instantiate (bulletPrefab, towerObject.transform.position, towerObject.transform.rotation);

		BulletMovement bulletMovement = bullet.GetComponent<BulletMovement> ();
		bulletMovement.target = closestEnemy;
		bulletMovement.speed = 30.0f;
		bulletMovement.damage = towerDamage;
	}

	void checkForDead() {
		for(int i = 0; i < nearbyEnemies.Count ; i++) {
			if (nearbyEnemies [i] == null) {
				nearbyEnemies.RemoveAt (i);
			}
		}
	}
}
