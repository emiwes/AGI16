using UnityEngine;
using System.Collections;

public class MonitorHP : MonoBehaviour {
	void Start () {
		GetComponent<TextMesh>().text = "";
	}

	void Update() {
		
		GetComponent<TextMesh> ().text = gameObject.transform.parent.gameObject.GetComponent<EnemyCombat> ().health.ToString();
	}
}