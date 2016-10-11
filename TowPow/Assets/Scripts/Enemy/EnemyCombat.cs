using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EnemyCombat : NetworkBehaviour {
	public float health = 100;
	public GameObject coinPrefab;
	public bool isNotDead = true;
	private Camera topCamera;

	void Start(){
		if (!NetworkServer.active) {
			topCamera = GameObject.FindGameObjectWithTag ("TopCamera").GetComponent<Camera> ();
		}
	}
		
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

	void OnDestroy(){
		if (!NetworkServer.active) {
			spawnCoin ();
		}
	}

	void spawnCoin (){

		GameObject coin = (GameObject)Instantiate(coinPrefab, topCamera.WorldToScreenPoint(transform.position), Quaternion.identity);
		coin.transform.SetParent(GameObject.Find("HUDCanvas").transform);

		//GameObject coin = Instantiate ( coinPrefab, this.transform.position, coinPrefab.transform.rotation ) as GameObject;
		//NetworkServer.Spawn (coin);
	}

}