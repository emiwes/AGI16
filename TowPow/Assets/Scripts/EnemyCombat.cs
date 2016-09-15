using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyCombat : MonoBehaviour {
	public float health = 100;
	public GameObject textObject;

	TextMesh textMesh; 

	public void takeDamage (float damage) {
		health -= damage;
		if (health <= 0)
			die ();
	}

	void die(){
		Destroy (gameObject.transform.parent.gameObject);
	}
}