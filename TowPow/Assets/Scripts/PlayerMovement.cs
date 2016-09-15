using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed = 100.0f;

	void Update () {
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
		transform.Translate(x, 0, z);
	}
}
