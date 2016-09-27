using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour {
	public float health = 100;

	public void takeDamage (float damage) {
		health -= damage;
		if (health <= 0)
			die ();
	}

	void die(){
		Animator animator = gameObject.GetComponent<Animator> ();
		animator.SetBool ("Die", true);
		// Debug.Log (animator.GetCurrentAnimatorStateInfo (0).length);
		Destroy (gameObject, animator.GetCurrentAnimatorStateInfo (0).length);
	}

}