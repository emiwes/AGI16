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
		spawnCoin();
		Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length);
	}

	void spawnCoin (){

		GameObject coin = Instantiate ( coinPrefab, this.transform.position, coinPrefab.transform.rotation ) as GameObject;
		NetworkServer.Spawn (coin);

	}

}