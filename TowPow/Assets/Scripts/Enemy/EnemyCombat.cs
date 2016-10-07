using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EnemyCombat : NetworkBehaviour {
	public float health = 100;
	private bool dead = false;

	public void takeDamage (float damage) {
		health -= damage;
		if (health <= 0 && !dead) {
			//Only kill object once
			dead = true;
			CmdDie ();
		}
			
	}
	[Command]
	void CmdDie(){
		Animator animator = gameObject.GetComponent<Animator> ();
//		animator.SetBool ("Die", true);
		animator.Play ("Die");
		//Also change kill counter on all clients
		Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length);
		GameObject.Find ("GameHandler").GetComponent<GameScript> ().killCounter += 1;
	}

}