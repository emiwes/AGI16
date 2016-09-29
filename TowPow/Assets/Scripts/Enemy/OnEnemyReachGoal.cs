using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnEnemyReachGoal : MonoBehaviour {

	public Slider HealthSlider;
	public Text HealthText;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			Destroy(other.gameObject);

			HealthSlider.value -= 1;
			HealthText.text = HealthSlider.value.ToString();
			if (HealthSlider.value <= 0) {
				GameObject.Find ("GameHandler").gameObject.GetComponent<GameScript>().GameOver = true;
				HealthText.text = "Game Over! :(";
				// Debug.Log ("GAME OVER");
			} 
			
		}
	}
}
