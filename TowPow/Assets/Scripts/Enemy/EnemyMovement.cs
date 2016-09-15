using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
		transform.Translate (0, 0, speed * 0.01f);
	}
}
