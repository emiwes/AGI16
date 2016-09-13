using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using TouchScript.InputSources;

public class BulletSpawner : NetworkBehaviour {

	public GameObject bulletPrefab;	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			CmdFire();
		}
	}

	[Command]
	void CmdFire(){
		// create the bullet object from the bullet prefab
		var bullet = (GameObject)Instantiate(bulletPrefab, transform.position - transform.forward, Quaternion.identity);

		// make the bullet move away in front of the player
		bullet.GetComponent<Rigidbody>().velocity = -transform.forward*4;

		// spawn the bullet on the clients
		NetworkServer.Spawn(bullet);

		// when the bullet is destroyed on the server it will automaticaly be destroyed on clients
		Destroy(bullet, 2.0f);      
	}
}
