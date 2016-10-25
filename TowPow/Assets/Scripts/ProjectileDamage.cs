using UnityEngine;
using System.Collections;

public class ProjectileDamage : MonoBehaviour {

	public AudioClip hitSound;
	private AudioSource source;
	public float damage = 30;
	public float damageOverTime = 0;
	public float speedMultiplier = 1;
	public float speedOverTime = 0;
	public Material morphEnemyToMaterial;

	void Awake(){
		source = gameObject.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			// other.gameObject.GetComponent<EnemyCombat> ().takeDamage(damage);
			other.gameObject.GetComponent<EnemyCombat> ().HandleIncomingProjectile(this);
            
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
