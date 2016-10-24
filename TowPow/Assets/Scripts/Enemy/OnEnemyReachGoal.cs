using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OnEnemyReachGoal : NetworkBehaviour {


    private GameScript GameScriptRef;

	void Awake() {
        GameScriptRef = GameObject.Find("GameHandler").GetComponent<GameScript>();

    }

	void OnTriggerEnter(Collider other) {
        //if (!GameScriptRef)
        //{
        //    GameScriptRef = GameObject.Find("GameHandler").GetComponent<GameScript>();
        //}
		if (isServer && other.gameObject.tag == "Enemy") {
			Destroy(other.gameObject);
            GameScriptRef.EnemyReachedGoal();
		}
	}

	
}