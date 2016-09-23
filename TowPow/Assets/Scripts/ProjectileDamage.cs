using UnityEngine;
using System.Collections;

public class ProjectileDamage : MonoBehaviour {
	public float damage;
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			other.gameObject.GetComponent<EnemyCombat> ().takeDamage(damage);
			Destroy (gameObject);
		}
	}
}
