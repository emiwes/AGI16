using UnityEngine;
using System.Collections;

public class SphereScript : MonoBehaviour {

	public float sphereRadius = 1;
	public GameObject spawnedByProjectile = null;

	public float damage;
	public float damageOverTimeSeconds;
	public float speedMultiplier;
	public float speedOverTimeSeconds;
	public Material morphEnemyToMaterial;
	public float areaOfEffectRadius;
	public float areaOfEffectExpandTime;
	public GameObject projectileAoePrefab;

	// Use this for initialization
	void Start () {
		// GetComponent<SphereCollider>().radius = sphereRadius;
		StartCoroutine(GrowProjectileColliderOverSeconds(spawnedByProjectile.GetComponent<ProjectileDamage>().areaOfEffectExpandTime, gameObject, sphereRadius));
	}
	
	void OnTriggerEnter (Collider other){
		if (other.gameObject.tag == "Enemy"){
			// Do the projectile damage/effects on the targets that collide with the AoE-collider
			other.gameObject.GetComponent<EnemyCombat> ().AffectByAoE(gameObject);
		}
	}


	IEnumerator GrowProjectileColliderOverSeconds(float time, GameObject go, float endRadius) {
		float elapsedTime = 0;
		GameObject growingSphere = go;

		while (elapsedTime < time) {
			growingSphere.GetComponent<SphereCollider>().radius = endRadius * (elapsedTime/time);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		// Set the radius to the final one without any weird decimals
		growingSphere.GetComponent<SphereCollider>().radius = endRadius;

		// Finally destroy the sphere when it has finished expanding
		Destroy(gameObject); // Comment out for eternal AoE (debugging)
	}
}
