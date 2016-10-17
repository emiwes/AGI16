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
			GameObject localPlayer = NetworkServer.FindLocalObject (GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId);
			Debug.Log ("Enemy Combat, localplayer id is: " + localPlayer.GetComponent<PlayerID>().myNetId.ToString());
			localPlayer.GetComponent<EnemyHandler>().CmdDie (gameObject);
//			CmdDie ();
		}
	}

//	[Command]
//	void CmdDie(){
//		Debug.Log ("DYING...");
//		Animator animator = gameObject.GetComponent<Animator> ();
////		animator.SetBool ("Die", true);
//
//		animator.Play ("Die");
//		//Play death sound
//		float vol = Random.Range (volLowRange, volHighRange);
//		source.PlayOneShot(deathSoundArray[Random.Range(0, deathSoundArray.Length)],vol);
//		//Also change kill counter on all clients
//		Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length);
//		GameObject.Find ("GameHandler").GetComponent<GameScript> ().killCounter += 1;
//	}

	void OnTakeDamage(float health) {
		//Update health slider on all clients
		HPSlider.value = health;
	}

	void OnDestroy(){
//		if (!NetworkServer.active) {
			CmdSpawnCoin ();
//		}
	}

	[Command]
	void CmdSpawnCoin (){
		GameObject coin = (GameObject)Instantiate(coinPrefab, topCamera.WorldToScreenPoint(transform.position), Quaternion.identity);
		coin.transform.SetParent(GameObject.Find("HUDCanvas").transform);
		NetworkServer.Spawn (coin);
	}

}