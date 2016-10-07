using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EnemyCombat : NetworkBehaviour {
	public float health = 100;

	public void takeDamage (float damage) {
		health -= damage;
		if (health <= 0) {
			GameObject.Find ("GameHandler").GetComponent<GameScript> ().killCounter += 1;
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

}