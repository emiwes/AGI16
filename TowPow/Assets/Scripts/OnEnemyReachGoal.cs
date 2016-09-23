using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnEnemyReachGoal : MonoBehaviour {

	public Slider HealthSlider;
	public Text HealthText;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			Destroy(other.gameObject.transform.parent.gameObject);
			if (HealthSlider.value == 0) {
				Debug.Log ("GAME OVER");
			} 
			else {
				HealthSlider.value -= 1;
				HealthText.text = HealthSlider.value.ToString();
			}
		}
	}
}
