﻿using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {
	public GameObject target;
	public float speed;
	public float damage;

	void Update () {
		float step = speed * Time.deltaTime;
		if (target != null) {
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, step);
		} else {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			other.gameObject.GetComponent<EnemyCombat> ().takeDamage(damage);
			Destroy (gameObject);
		}
	}
}
