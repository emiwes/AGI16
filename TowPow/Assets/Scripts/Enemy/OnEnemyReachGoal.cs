using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnEnemyReachGoal : MonoBehaviour {


    private GameScript GameScriptRef;

	void OnTriggerEnter(Collider other) {
		if (!GameScriptRef) {
			GameScriptRef = GameObject.Find("GameHandler").GetComponent<GameScript>();
		}
		if (other.gameObject.tag == "Enemy") {
			GameScriptRef.EnemyReachedGoal(other.gameObject);
		}
	}

	
}