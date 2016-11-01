using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TowerCombat : NetworkBehaviour {

//	[System.Serializable]
	public struct TowerLevelInfo {
		public int level;
		public float damageMultiplier;
//		public float range;
		public float shootingSpeed;
		public int costToUpgrade;
		public float aoeSizeMultiplier, slowEffectMultiplier;

		public TowerLevelInfo(int l, float d, float s, int c, float a, float slow){
			level = l;
			damageMultiplier = d;
			shootingSpeed = s;
			costToUpgrade = c;
			aoeSizeMultiplier = a;
			slowEffectMultiplier = slow;
		}
	}
	
	public List<TowerLevelInfo> levelInfo = new List<TowerLevelInfo>();

	public TowerLevelSynchronize towerLevelSynchronize;

	public List<GameObject> nearbyEnemies = new List<GameObject>();
	GameObject closestEnemy = null;

	public GameObject bulletPrefab;
	public GameObject shootingRange;
	public GameObject shootingRangeIndicator;
	public float damageMultiplier, aoeSizeMultiplier, slowEffectMultiplier;
	public float shootingSpeed, timeSinceLastShot = float.MaxValue;
	private GameObject shootingModule;
	private TowerSpawn towerSpawn;

	public int level;

	public AudioClip shootSound;
	private AudioSource source;

	void Start() {
		switch(gameObject.tag){
			case "black":
			case "white":
			{
				levelInfo.Add (new TowerLevelInfo (1, 1.5f, 0.6f, 100, 1f, 1f));
				levelInfo.Add (new TowerLevelInfo (2, 2f, 0.5f, 200, 1f, 1f));
				levelInfo.Add (new TowerLevelInfo (3, 3f, 0.3f, 350, 1f, 1f));
				break;
			}
			case "red":
			{
				levelInfo.Add (new TowerLevelInfo (1, 1.5f, 1.7f, 100, 1f, 1f));
				levelInfo.Add (new TowerLevelInfo (2, 2.0f, 1.3f, 200, 1f, 1f));
				levelInfo.Add (new TowerLevelInfo (3, 3.0f, 1f, 350, 1f, 1f));
				break;
			}
			case "blue":
			{
				levelInfo.Add (new TowerLevelInfo (1, 1.5f, 2f, 100, 1f, 1f));
				levelInfo.Add (new TowerLevelInfo (2, 2.0f, 1.7f, 200, 1f, 1f));
				levelInfo.Add (new TowerLevelInfo (3, 3.0f, 1.5f, 350, 1f, 1f));
				break;
			}
		}
		// if(gameObject.tag == "black" || gameObject.tag == "white"){
		// 	levelInfo.Add (new TowerLevelInfo (1, 15f, 0.6f, 100));
		// 	levelInfo.Add (new TowerLevelInfo (2, 20f, 0.5f, 200));
		// 	levelInfo.Add (new TowerLevelInfo (3, 30f, 0.3f, 350));
		// }
		// levelInfo.Add (new TowerLevelInfo (1, 15f, 0.6f, 100));
		// levelInfo.Add (new TowerLevelInfo (2, 20f, 0.5f, 200));
		// levelInfo.Add (new TowerLevelInfo (3, 30f, 0.3f, 350));

		shootingModule = transform.Find ("Tower/ShootingModule").gameObject;
		shootingRangeIndicator.SetActive(true);
		towerSpawn = GetComponent<TowerSpawn> ();
		towerLevelSynchronize = GameObject.Find ("GameHandler").GetComponent<TowerLevelSynchronize>();
		SyncTowerLevel ();
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

		if (nearbyEnemies.Count == 0) {
			return;
		}

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
		projectileDamage.damage *= damageMultiplier;
		projectileDamage.areaOfEffectRadius *= aoeSizeMultiplier;
		projectileDamage.speedMultiplier /= slowEffectMultiplier;
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

//	public void LevelUp(){
//		GameObject localPlayer = ClientScene.FindLocalObject (GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId);
//		localPlayer.GetComponent<CoinHandler> ().CmdDecrementMoney (levelInfo [level-1].costToUpgrade);
//		localPlayer.GetComponent<TowerHandler> ().CmdLevelUp (gameObject.tag);
//	}

	public void SyncTowerLevel() {
		level = towerLevelSynchronize.GetLevel (gameObject.tag);
		damageMultiplier = levelInfo [level - 1].damageMultiplier;
		shootingSpeed = levelInfo [level - 1].shootingSpeed;
		aoeSizeMultiplier = levelInfo [level - 1].aoeSizeMultiplier;

		/*Debug.Log ("Tower level set to " + level);
		Debug.Log ("Tower speed set to " + shootingSpeed);
		Debug.Log ("Tower damage set to " + towerDamage);*/
	}
}