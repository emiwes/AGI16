using UnityEngine;
using System.Collections;

public class ProjectileDamage : MonoBehaviour {

	public AudioClip hitSound;
	private AudioSource source;
	public float damage = 30;
	public float damageOverTimeSeconds = 0;
	public float speedMultiplier = 1;
	public float speedOverTimeSeconds = 0;
	public Material morphEnemyToMaterial;
	public float areaOfEffectRadius = 0;
	public float areaOfEffectExpandTime = 1;
	public GameObject projectileAoePrefab;

	void Awake(){
		source = gameObject.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			// Do the projectile damage/effects on the target
			// other.gameObject.GetComponent<EnemyCombat> ().HandleIncomingProjectile(this);

			// Create area of effect if we have a defined radius
			if(areaOfEffectRadius > 0){
				// Instantiate the AoE prefab (it grows by itself)
				GameObject projectileAoe = (GameObject)Instantiate(projectileAoePrefab, other.transform.position, Quaternion.identity);
				projectileAoe.GetComponent<SphereScript> ().sphereRadius = areaOfEffectRadius;
				projectileAoe.GetComponent<SphereScript>().spawnedByProjectile = gameObject;
			} else{
				// Otherwise, do the projectile damage/effects on the target
				other.gameObject.GetComponent<EnemyCombat> ().HandleIncomingProjectile(this);
			}
            
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

			// Destroy the projectile
			Destroy (gameObject);
		}
	}
}
