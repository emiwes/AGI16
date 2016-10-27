using UnityEngine;
using System.Collections;

public class SphereScript : MonoBehaviour {

	public float sphereRadius = 1;

	// Use this for initialization
	void Start () {
		// GetComponent<SphereCollider>().radius = sphereRadius;
		StartCoroutine(GrowProjectileColliderOverSeconds(1.0f, gameObject, 10));
	}
	
	// Update is called once per frame
	void Update () {
		// GetComponent<SphereCollider>().radius = sphereRadius;
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
		
	}
}
