using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyHandler : NetworkBehaviour {

	[Command]
	public void CmdDie(GameObject pirate){
		Debug.Log ("DYING...");
		EnemyCombat enemyCombatComponent = pirate.GetComponent<EnemyCombat>();
		Animator animator = pirate.GetComponent<Animator> ();
		//		animator.SetBool ("Die", true);

		animator.Play ("Die");
		//Play death sound
		float vol = Random.Range (.5f, 1f);
		enemyCombatComponent.source.PlayOneShot(enemyCombatComponent.deathSoundArray[Random.Range(0, enemyCombatComponent.deathSoundArray.Length)],vol);
		//Also change kill counter on all clients
		Destroy (pirate, animator.GetCurrentAnimatorStateInfo (0).length);
		GameObject.Find ("GameHandler").GetComponent<GameScript> ().killCounter += 1;
	}
}
