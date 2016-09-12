using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {

	public GameObject testObjectPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Q)) {
			Spawn ();
		}
	}

	void Spawn() {
		Debug.Log ("Spawn");
	}
}