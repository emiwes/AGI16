using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OnEnemyReachGoal : NetworkBehaviour {

	public Slider HealthSlider;
	public Text HealthText;
	[SyncVar (hook = "OnChangeHealth")]
	public int HealthSliderValue = 0;

	void Awake() {
		HealthSliderValue = GameObject.Find ("GameHandler").GetComponent<GameScript> ().PlayerHealth;
	}

	void OnTriggerEnter(Collider other) {
		if (isServer && other.gameObject.tag == "Enemy") {
			Destroy(other.gameObject);
			if (HealthSliderValue != 0) {
				HealthSliderValue -= 1;
				GameObject.Find ("GameHandler").GetComponent<GameScript> ().PlayerHealth -= 1;
				if (HealthSliderValue == 0) {
					GameObject.Find ("GameHandler").GetComponent<GameScript> ().GameOver = true;
					HealthText.text = "GAME OVER";
				}
			}
		}
	}
	void OnChangeHealth(int health) {
		HealthSlider.value = health;
		HealthText.text = health.ToString();
	}
}