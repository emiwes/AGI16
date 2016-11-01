using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ProjectileDamage : NetworkBehaviour {

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
    public GameObject projectileHitEffectPrefab;

	void Awake(){
		source = gameObject.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.tag == "noCollision") {
			return;
		}

        // Create are of effect if we have a defined radius
        if (areaOfEffectRadius > 0) {
            AOE();
        } else if (other.gameObject.tag == "Enemy") {
            other.gameObject.GetComponent<EnemyCombat>().HandleIncomingProjectile(this);
        }

        //if it has trailing smoke remove it after the removal of the projectile
        if (gameObject.transform.FindChild("movingSmoke")) {
            Transform smoke = gameObject.transform.FindChild("movingSmoke");
            smoke.parent = null;
            smoke.GetComponent<ParticleSystem>().Stop(true);

            Destroy(smoke.gameObject, 2.0f);
        }

        //Play sound if arrow (== tag friendly)
        if (gameObject.tag == "Friendly") {
            source.PlayOneShot(hitSound, .7f);
        }

        // Destroy the projectile
        Destroy (gameObject);
	}

    void AOE() {
        // Instantiate the AoE prefab (it grows by itself)
        GameObject projectileAoe = (GameObject)Instantiate(projectileAoePrefab, transform.position, Quaternion.identity);
        SphereScript ss = projectileAoe.GetComponent<SphereScript>();
        ss.sphereRadius = areaOfEffectRadius;
        ss.spawnedByProjectile = gameObject;
        ss.damage = damage;
        ss.damageOverTimeSeconds = damageOverTimeSeconds;
        ss.speedMultiplier = speedMultiplier;
        ss.speedOverTimeSeconds = speedOverTimeSeconds;
        ss.morphEnemyToMaterial = morphEnemyToMaterial;
        ss.areaOfEffectRadius = areaOfEffectRadius;
        ss.areaOfEffectExpandTime = areaOfEffectExpandTime;
        ss.projectileAoePrefab = projectileAoePrefab;

        // If there's an explosion effect instantiate it locally and on the server
        if (projectileHitEffectPrefab) {
            GameObject explosion = (GameObject)Instantiate(projectileHitEffectPrefab, transform.position, Quaternion.identity);
			NetworkServer.Spawn (explosion);
        }
    }
}
