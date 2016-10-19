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

	private AudioSource source;
	private float volLowRange = .5f;
	private float volHighRange = 1.0f;

	void Start () {
		HPSlider.maxValue = health;
		topCamera = GameObject.FindGameObjectWithTag ("TopCamera").GetComponent<Camera> ();
	}

	void Awake() {
		source = GetComponent<AudioSource>();
	}

	public void takeDamage (float damage) {
		if (isServer && health > 0) {
			health -= damage;
			if (health <= 0) {
				Destroy (HPSlider.transform.GetChild(1).gameObject);
				GameObject.Find ("GameHandler").GetComponent<GameScript> ().killCounter += 1;
			}
		}
	}

	void OnTakeDamage(float health) {
		//Update health slider on all clients
		HPSlider.value = health;
		if (health <= 0) {
			Animator animator = gameObject.GetComponent<Animator> ();
			
			animator.Play ("Die");
			//Play death sound
			float vol = Random.Range (volLowRange, volHighRange);
			source.PlayOneShot(deathSoundArray[Random.Range(0, deathSoundArray.Length)],vol);

			if (!isServer) {
				spawnCoin ();
			}

			Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length);
		}
	}

	void spawnCoin (){
		GameObject coin = (GameObject)Instantiate(coinPrefab, topCamera.WorldToScreenPoint(transform.position), Quaternion.identity);
		coin.transform.SetParent(GameObject.Find("HUDCanvas").transform);
	}
}