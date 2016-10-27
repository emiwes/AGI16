using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnEnemyReachGoal : MonoBehaviour {


    private GameScript GameScriptRef;

	void Awake() {
        GameScriptRef = GameObject.Find("GameHandler").GetComponent<GameScript>();

    }

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.tag == "Enemy") {
			GameScriptRef.EnemyReachedGoal(other.gameObject);
		}
	}

	
}