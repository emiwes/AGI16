using UnityEngine;
using System.Collections;

public class playFXSound : MonoBehaviour {

	public AudioClip FXSound;
	private AudioSource source;
	private float volLowRange = .2f;
	private float volHighRange = .5f;


	void Awake() {
		source = GetComponent<AudioSource>();
	}
	// Use this for initialization
	void Start () {
		float vol = Random.Range (volLowRange, volHighRange);
		source.PlayOneShot(FXSound,vol);
	}

}
