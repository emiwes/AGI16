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

	void Start () {
		HPSlider.maxValue = health;
		//TODO: THIS FAILS ON THE VIVE BECAUSE TOP CAMERA IS DEACTIVATED.
		topCamera = GameObject.FindGameObjectWithTag ("TopCamera").GetComponent<Camera> ();
	}

	void Awake() {
		source = GetComponent<AudioSource>();
	}

	public void takeDamage (float damage) {
		
		if (isServer && health > 0) {
			health -= damage;
			if (health <= 0) {
				//dead = true;

				Destroy (HPSlider.transform.GetChild (1).gameObject);
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

			//TODO: WILL FAIL IF PIXELSENSE IS HOST
			if (!isServer) {
				GameObject coin = (GameObject)Instantiate (coinPrefab, topCamera.WorldToScreenPoint (gameObject.transform.position), Quaternion.identity);
				coin.transform.SetParent (GameObject.Find ("HUDCanvas").transform);
			}

			Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length);
		}
	}

}