using UnityEngine;
using System.Collections;

public class ProjectileDamage : MonoBehaviour {

	public AudioClip hitSound;
	private AudioSource source;
	public float damage;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			other.gameObject.GetComponent<EnemyCombat> ().takeDamage(damage);
            
            //if it has trailing smoke remove it after the removal of the projectile
            if (gameObject.transform.FindChild("movingSmoke"))
            {
                Transform smoke = gameObject.transform.FindChild("movingSmoke");
                smoke.parent = null;
                smoke.GetComponent<ParticleSystem>().Stop(true);

                Destroy(smoke.gameObject, 2.0f);
            }
          
			//Play sound if arrow (== tag friendly)
			if (gameObject.tag == "Friendly") {
				source.PlayOneShot (hitSound, .7f);
			}
			Destroy (gameObject);
		}
	}
}
