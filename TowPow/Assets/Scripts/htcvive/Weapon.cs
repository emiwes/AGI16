using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public GameObject bulletPrefab;
    public float bulletSpeed;

    public Transform bulletSpawnPointTransform;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public void Fire() {
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPointTransform.position, bulletSpawnPointTransform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        Destroy(bullet, 5.0f);
    }
}
