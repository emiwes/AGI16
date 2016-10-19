using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OnEnemyReachGoal : NetworkBehaviour {

	public Slider HealthSlider;
	public Text HealthText;

    private GameScript GameScriptRef;

	void Awake() {
        GameScriptRef = GameObject.Find("GameHandler").GetComponent<GameScript>();

    }

	void OnTriggerEnter(Collider other) {
        if (!GameScriptRef)
        {
            Debug.Log("sets gamescript ref");
            GameScriptRef = GameObject.Find("GameHandler").GetComponent<GameScript>();
        }
		if (isServer && other.gameObject.tag == "Enemy") {
			Destroy(other.gameObject);
			if (GameScriptRef.PlayerHealth != 0) {
                GameScriptRef.PlayerHealth -= 1;
                HealthSlider.value = GameScriptRef.PlayerHealth;
                HealthText.text = GameScriptRef.PlayerHealth.ToString();
                //GameScriptRef.PlayerHealth -= 1;
                if (GameScriptRef.PlayerHealth == 0) {
                    GameScriptRef.GameOver = true;
					HealthText.text = "GAME OVER";
				}
			}
		}
	}
	
}