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
		if (!isServer)
			return;

		if (Input.GetKeyUp (KeyCode.Q)) {
			RpcSpawn ();
		}
	}

	[ClientRpc]
	void RpcSpawn() {
		Debug.Log ("Spawn");
		GameObject testObject = (GameObject)Instantiate (testObjectPrefab, transform.position, transform.rotation);
		NetworkServer.Spawn (testObject);
	}
}