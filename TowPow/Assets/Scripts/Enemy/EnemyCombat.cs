using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EnemyCombat : NetworkBehaviour {
	public float health = 100;
	public GameObject coinPrefab;
	public bool isNotDead = true;

	public void takeDamage (float damage) {
		health -= damage;
		if (health <= 0 && isNotDead){
			isNotDead = false;
			CmdDie ();
		}
	}

	[Command]
	void CmdDie(){
		
		Animator animator = gameObject.GetComponent<Animator> ();
//		animator.SetBool ("Die", true);

		animator.Play ("Die");
		Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length);
	}

	void spawnCoin (){
		Vector3 coinSpawnPosition = this.transform.position;
		coinSpawnPosition.y = 20;
		GameObject coin = Instantiate ( coinPrefab, coinSpawnPosition, coinPrefab.transform.rotation ) as GameObject;
		NetworkServer.Spawn (coin);

	}

	void OnDestroy(){
		Debug.Log ("On Destroy is CALLEEEEED");
		if (GameObject.Find ("PixelSenseComponents").activeSelf) {
			spawnCoin ();
		}
	}
}