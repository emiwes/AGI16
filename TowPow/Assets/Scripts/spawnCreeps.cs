using UnityEngine;
using System.Collections;

public class spawnCreeps : MonoBehaviour {

    public GameObject enemyPrefab;
    public GameObject pathPrefab;
    public GameObject startPosition;

    // Use this for initialization
    void Start () {
        
       
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
        //Instatiate Creep
        GameObject temp_enemy = Instantiate(enemyPrefab, startPosition.transform.position, Quaternion.identity) as GameObject;
        //set path as a child to creepSpawner
        temp_enemy.transform.parent = GameObject.Find("enemySpawner").transform;
        //Instatiate path that creep should go
        GameObject temp_path = Instantiate(pathPrefab) as GameObject;
        //set path as a child to Creep
        temp_path.transform.parent = temp_enemy.transform;

        //attack path to creep
        temp_enemy.GetComponent<SplineController>().SplineRoot = temp_path;
        

    }
}
