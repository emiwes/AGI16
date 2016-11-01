using UnityEngine;
using System.Collections;

public class DestroyIn : MonoBehaviour {

	public float time = 3f;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, time);
	}
}
