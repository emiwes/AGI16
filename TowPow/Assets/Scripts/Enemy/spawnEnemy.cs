using UnityEngine;
using System.Collections;

public class spawnEnemy : MonoBehaviour {

    public GameObject enemyPrefab;
    [HideInInspector]
    public Transform target;
    //public GameObject pathPrefab;
    //public GameObject startPosition;
    //public GameObject spawObj;

    // Use this for initialization
    void Start () {
        target = this.transform.Find("target");
		}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("space"))
        {
            spawnSingleEnemy();
        }

    }

    public void spawnSingleEnemy()
    {

        //Instatiate Enemy
        GameObject temp_enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity) as GameObject;
        //set path as a child to spawner Gameobject.
        temp_enemy.GetComponent<EnemyMovement>().target = target;
        temp_enemy.transform.SetParent(this.transform);

    }
}
