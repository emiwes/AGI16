using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerCombat : MonoBehaviour {
	public List<GameObject> nearbyEnemies = new List<GameObject>();
	GameObject closestEnemy = null;

	public GameObject bulletPrefab;
	public GameObject shootingRange;
	public GameObject shootingRangeIndicator;
	public float towerDamage;
	public float shootingSpeed, timeSinceLastShot = float.MaxValue;
	private GameObject shootingModule;
	private TowerSpawn towerSpawn;

	public AudioClip shootSound;
	private AudioSource source;

	void Start() {
		shootingModule = transform.Find ("ShootingModule").gameObject;
		//InvokeRepeating ("fireAtClosestEnemy", 0.5f, shootingSpeed);
		shootingRangeIndicator.SetActive(true);
		// transform.Find("ShootingRadiusIndicator").gameObject.SetActive(true);
		towerSpawn = GetComponent<TowerSpawn> ();
	}

	void Awake() {
		source = GetComponent<AudioSource>();
	}

	void Update() {
		if (towerSpawn.isActive) {
			if (timeSinceLastShot > shootingSpeed) {
				fireAtClosestEnemy ();
				timeSinceLastShot = 0f;
			}
			timeSinceLastShot += Time.deltaTime;
		}
	}

	public void addNearbyEnemy(GameObject enemy){
        Debug.Log("Adding nearby enemy "+enemy.name);
		nearbyEnemies.Add (enemy);
	}

	public void removeNearbyEnemy(GameObject enemy){
		nearbyEnemies.Remove (enemy);
	}

	public void towerLookAt (Vector3 pos) {
		pos.y = 0;
		shootingModule.transform.LookAt (pos);
	}
		
	public void updateClosestEnemy(){
		// Check if any enemies have been killed since last iteration, remove null references. 
		checkForDead ();
        if (nearbyEnemies.Count == 0) return;

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

		GameObject bullet = (GameObject)Instantiate (bulletPrefab, shootingModule.transform.position, shootingModule.transform.rotation);

		BulletMovement bulletMovement = bullet.GetComponent<BulletMovement> ();
		ProjectileDamage projectileDamage = bullet.GetComponent<ProjectileDamage> ();
		bulletMovement.target = closestEnemy;
		bulletMovement.speed = 30.0f;
		projectileDamage.damage = towerDamage;
		//Play sound
		float vol = Random.Range (0.7f, 1f);
		source.PlayOneShot(shootSound,vol);
	}

	void checkForDead() {
		for(int i = 0; i < nearbyEnemies.Count ; i++) {
			if (nearbyEnemies [i] == null) {
				nearbyEnemies.RemoveAt (i);
			}
		}
	}
}
