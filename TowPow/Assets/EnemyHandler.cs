using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyHandler : NetworkBehaviour {

	public Camera topCamera;
	public GameObject coinPrefab;

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


		//gameObject.GetComponent<CoinHandler>().CmdSpawnCoin (transform.position);

		GameObject coin = (GameObject)Instantiate(coinPrefab, topCamera.WorldToScreenPoint(pirate.transform.position), Quaternion.identity);
		coin.transform.SetParent(GameObject.Find("HUDCanvas").transform);
		NetworkServer.Spawn (coin);

		Destroy (pirate, animator.GetCurrentAnimatorStateInfo (0).length);
		//Also change kill counter on all clients
		GameObject.Find ("GameHandler").GetComponent<GameScript> ().killCounter += 1;
	}
}
