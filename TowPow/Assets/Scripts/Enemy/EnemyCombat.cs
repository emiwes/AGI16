using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EnemyCombat : NetworkBehaviour {
	[SyncVar (hook = "OnTakeDamage")]
	public float health = 100;
	public GameObject coinPrefab;
	private Camera topCamera;

	public Slider HPSlider;

	public AudioClip[] deathSoundArray;

	public AudioSource source;
	private float volLowRange = .2f;
	private float volHighRange = .5f;

	private GameObject localPlayer;
	private float initialEnemySpeed;

	void Start () {
		HPSlider.maxValue = health;
		if(!DeterminePlayerType.isVive) {
			topCamera = GameObject.FindGameObjectWithTag ("TopCamera").GetComponent<Camera> ();
		}
	}

	void Awake() {
		source = GetComponent<AudioSource>();
		initialEnemySpeed = GetComponent<NavMeshAgent>().speed;
	}


	public void HandleIncomingProjectile(ProjectileDamage projectile){
		ProjectileDamage p = projectile;
		
		// Take damage from projectile
		if(p.damage != 0){
			takeDamage(p.damage);
		}

		// Slow down enemy
		if(p.slowFactor != 1){
			StartCoroutine(AffectOverSeconds(p.speedOverTime, null, p.speedMultiplier));
			// gameObject.GetComponent<NavMeshAgent>().speed *= p.slowFactor;
		}
	}

	IEnumerator AffectOverSeconds(float time, float damage = null, float speedMultiplier = null) {
		float elapsedTime = 0;

		// The NavMeshAgent of this enemy
		NavMeshAgent nma = gameObject.GetComponent<NavMeshAgent>();

		// The most slowed the enemy will be, which is right when the projectile hits
		float peakSlow;
		speedMultiplier ? peakSlow = initialEnemySpeed * speedMultiplier :;

		while (elapsedTime < time) {
			// If we have a speed multiplier, change the speed over time
			speedMultiplier ? nma.speed = peakSlow + (initialEnemySpeed - (peakSlow)) * (elapsedTime/time)):;
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		// Reset the enemy speed to the normal value
		nma.speed = initialEnemySpeed;
	}

	public void takeDamage (float damage) {
		
		if (isServer && health > 0) {
			health -= damage;
			if (health <= 0) {
				//dead = true;

				Destroy (HPSlider.gameObject);
				GameObject.Find ("GameHandler").GetComponent<GameScript> ().killCounter += 1;


			}
		}
	}
		
	void OnTakeDamage(float health) {
		//Update health slider on all clients
		HPSlider.value = health;

		//Will only run once since health is never updated when it is below 0
		if (health <= 0) {
			EnemyCombat enemyCombatComponent = gameObject.GetComponent<EnemyCombat> ();
			Animator animator = gameObject.GetComponent<Animator> ();
			animator.Play ("Die");
			//Play death sound
			float vol = Random.Range (volLowRange, volHighRange);
			enemyCombatComponent.source.PlayOneShot (enemyCombatComponent.deathSoundArray [Random.Range (0, enemyCombatComponent.deathSoundArray.Length)], vol);

			if (!DeterminePlayerType.isVive) {
				GameObject coin = (GameObject)Instantiate (coinPrefab, topCamera.WorldToScreenPoint (gameObject.transform.position), Quaternion.identity);
				coin.transform.SetParent (GameObject.Find ("HUDCanvas").transform);
			}
			Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length);
		}
	}

}