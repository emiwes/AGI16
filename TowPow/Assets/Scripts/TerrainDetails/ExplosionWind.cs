using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionWind : MonoBehaviour {
	public float explosionForce = 1f;

	private Leaf[] leaves;

	// Use this for initialization
	void Start () {
		leaves = Object.FindObjectsOfType<Leaf> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Space)) {
			Explosion ();
		}
	}

	void Explosion () {
		foreach (Leaf leaf in leaves) {
			GameObject go = leaf.gameObject;
			Vector3 blastVector = (go.transform.position - transform.position);
			blastVector = blastVector.normalized / blastVector.magnitude;
			blastVector *= explosionForce;
			leaf.SetSpeed(blastVector);
		}
	}
}
