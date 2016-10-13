using UnityEngine;
using System.Collections;

public class ProjectileDamage : MonoBehaviour {

	public AudioClip hitSound;
	private AudioSource source;
	public float damage;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			other.gameObject.GetComponent<EnemyCombat> ().takeDamage(damage);
			//Play sound if arrow (== tag friendly)
			if (gameObject.tag == "Friendly") {
				source.PlayOneShot (hitSound, .7f);
			}
			Destroy (gameObject);
		}
	}
}
