using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OnEnemyReachGoal : NetworkBehaviour {

	public Slider HealthSlider;
	public Text HealthText;
	[SyncVar (hook = "OnChangeHealth")]
	public int HealthSliderValue = 10;

	void OnTriggerEnter(Collider other) {
		if (isServer && other.gameObject.tag == "Enemy") {
			Destroy(other.gameObject);
			if (HealthSliderValue == 0) {
				GameObject.Find ("GameHandler").gameObject.GetComponent<GameScript>().GameOver = true;
				Debug.Log ("GAME OVER");
			} 
			else {
				HealthSliderValue -= 1;
			}
		}
	}
	void OnChangeHealth(int health) {
		HealthSlider.value = health;
		HealthText.text = health.ToString();
	}
}