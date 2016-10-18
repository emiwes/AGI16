using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EnemyCombat : NetworkBehaviour {
	[SyncVar (hook = "OnTakeDamage")]
	public float health = 100;
	public GameObject coinPrefab;
	public Camera topCamera;

	public Slider HPSlider;

	public AudioClip[] deathSoundArray;

	public AudioSource source;
	private float volLowRange = .5f;
	private float volHighRange = 1.0f;

	private GameObject localPlayer;

	void Start () {
		HPSlider.maxValue = health;
		if (!NetworkServer.active) {
//			topCamera = GameObject.FindGameObjectWithTag ("TopCamera").GetComponent<Camera> ();
		}
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

				GameObject coin = (GameObject)Instantiate(coinPrefab, topCamera.WorldToScreenPoint(gameObject.transform.position), Quaternion.identity);
				coin.transform.SetParent(GameObject.Find("HUDCanvas").transform);
				NetworkServer.Spawn (coin);
			}
		}
	}
		
	void OnTakeDamage(float health) {
		//Update health slider on all clients
		Debug.Log("<<<Health is>>> " + health);
		HPSlider.value = health;

		//Will only run once since health is never updated when it is below 0
		if (health <= 0) {
			EnemyCombat enemyCombatComponent = gameObject.GetComponent<EnemyCombat> ();
			Animator animator = gameObject.GetComponent<Animator> ();
			animator.Play ("Die");
			//Play death sound
			float vol = Random.Range (.5f, 1f);
			enemyCombatComponent.source.PlayOneShot (enemyCombatComponent.deathSoundArray [Random.Range (0, enemyCombatComponent.deathSoundArray.Length)], vol);

			Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length);
		}

	}

	void OnDestroy(){
		
	}

}