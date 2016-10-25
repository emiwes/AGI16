using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionWind : MonoBehaviour {
	public float explosionForce = 1f;

	private PalmTree[] trees;

	// Use this for initialization
	void Start () {
		trees = Object.FindObjectsOfType<PalmTree> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Space)) {
			Explosion ();
		}
	}

	void Explosion () {
		foreach (PalmTree tree in trees) {
			GameObject go = tree.gameObject;
			Vector3 blastVector = (go.transform.position - transform.position);
			blastVector = blastVector.normalized / blastVector.magnitude;
			blastVector *= explosionForce;
			tree.SetSpeed(blastVector);
		}
	}
}
