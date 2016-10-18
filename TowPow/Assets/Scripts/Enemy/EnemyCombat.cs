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
	private bool dead = false;

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
		health -= damage;
		if (health <= 0 && !dead) {
			//Only kill object once
			dead = true;
			Destroy (HPSlider.transform.GetChild(1).gameObject);
			// Retrieving local players game object from a netId that was stored when localPlayer was instantiated.
			localPlayer = ClientScene.FindLocalObject (GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId);
			localPlayer.GetComponent<EnemyHandler>().CmdDie (gameObject);
		}
	}
		
	void OnTakeDamage(float health) {
		//Update health slider on all clients
		Debug.Log("<<<Health is>>> " + health);
		HPSlider.value = health;
	}

	void OnDestroy(){
		
	}

}