using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OnEnemyReachGoal : NetworkBehaviour {

	public Slider HealthSlider;
	public Text HealthText;
	[SyncVar]
	public int HealthSliderValue = 10;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			Destroy(other.gameObject.transform.parent.gameObject);
			if (HealthSliderValue == 0) {
				GameObject.Find ("GameHandler").gameObject.GetComponent<GameScript>().GameOver = true;
				Debug.Log ("GAME OVER");
			} 
			else {
				HealthSliderValue -= 1;
				HealthSlider.value = HealthSliderValue;
				HealthText.text = HealthSliderValue.ToString();
			}
		}
	}
}
