using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionWind : MonoBehaviour {
	public float explosionForce = 1f;

	private PalmTreeContainer container;

	private PalmTree[] trees;

	// Use this for initialization
	void Start () {
		container = Object.FindObjectOfType<PalmTreeContainer> ();
		if (container != null) {
			Debug.Log ("Den hittades");
			Explosion ();
		} else {
			Debug.Log ("Containern kunde inte hittas");
		}
	}

	void Explosion () {
		trees = container.GetPalmTrees();
		foreach (PalmTree tree in trees) {
			GameObject go = tree.gameObject;
			Vector3 blastVector = (go.transform.position - transform.position);
			blastVector = blastVector.normalized / blastVector.magnitude;
			blastVector *= explosionForce;
			tree.SetSpeed(blastVector);
		}
	}
}
