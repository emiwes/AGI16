using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {
	public GameObject target;
	public float speed;

	void Update () {
		float step = speed * Time.deltaTime;
		if (target != null) {
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, step);
		} else {
			// Target destroyed by other bullet / projectile
			Destroy (gameObject);
		}
	}
}