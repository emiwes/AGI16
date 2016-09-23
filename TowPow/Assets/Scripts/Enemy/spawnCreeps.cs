using UnityEngine;
using System.Collections;

public class spawnCreeps : MonoBehaviour {

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
            spawnZombie();
        }

    }

    void spawnZombie()
    {
        
        //Debug.Log("spwn!!");

        //Instatiate path that creep should go
        //GameObject temp_path = Instantiate(pathPrefab) as GameObject;

        //Get startPosition
        //Transform startPosition = temp_path.transform.GetChild(0);

        //Instatiate Creep
        GameObject temp_enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity) as GameObject;
        //set path as a child to spawner Gameobject.
        temp_enemy.GetComponent<EnemyMovement>().target = target;
        temp_enemy.transform.SetParent(this.transform);

        
        //set path as a child to Creep
        //temp_path.transform.parent = temp_enemy.transform;

        //attack path to creep
        //temp_enemy.GetComponent<SplineController>().SplineRoot = temp_path;
        

    }
}
