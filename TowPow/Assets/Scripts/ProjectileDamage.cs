using UnityEngine;
using System.Collections;

public class ProjectileDamage : MonoBehaviour {
	public float damage;
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			other.gameObject.GetComponent<EnemyCombat> ().takeDamage(damage);
            if (gameObject.transform.FindChild("movingSmoke"))
            {
                Transform smoke = gameObject.transform.FindChild("movingSmoke");
                smoke.parent = null;
                smoke.GetComponent<ParticleSystem>().Stop(true);

                Destroy(smoke.gameObject, 2.0f);
            }
          

            Destroy(gameObject);
		}
	}
}
