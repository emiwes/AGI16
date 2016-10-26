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
	public float areaOfEffectRadius = 0;
	public GameObject projectileAoePrefab;

	void Awake(){
		source = gameObject.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			// Do the projectile damage/effects on the target
			other.gameObject.GetComponent<EnemyCombat> ().HandleIncomingProjectile(this);

			// Create area of effect if we have a defined radius
			if(areaOfEffectRadius > 0){

				// Instantiate the AoE prefab
				GameObject projectileAoe = (GameObject)Instantiate(projectileAoePrefab, other.transform.position, Quaternion.identity);

				// Create a SphereCollider
				// SphereCollider aoeCollider = projectileAoe.AddComponent<SphereCollider>() as SphereCollider;
				
				// Get the SphereCollider of the AoE Prefab
				SphereCollider aoeCollider = projectileAoe.GetComponent<SphereCollider>();
				
				// Set the initial radius of the collider
				aoeCollider.radius = 0;

				// Grow the sphere over a set time
				StartCoroutine(GrowProjectileColliderOverSeconds(1.0f, projectileAoe, areaOfEffectRadius));
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


	IEnumerator GrowProjectileColliderOverSeconds(float time, GameObject go, float endRadius) {
		float elapsedTime = 0;

		while (elapsedTime < time) {
			go.GetComponent<SphereCollider>().radius = endRadius * (elapsedTime/time);

			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		// Set the radius to the final one without any weird decimals
		go.GetComponent<SphereCollider>().radius = endRadius;
		
	}
}
