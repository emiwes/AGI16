using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationNode : MonoBehaviour {

    public List<Transform> childs = new List<Transform>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {

		if (col.gameObject.tag == "Enemy") {
			
			//change to one of the childs of the node
			// Get EnemyMovmenetScript
			EnemyMovement em = col.gameObject.GetComponent<EnemyMovement> ();
			//check if it is the target Node of the enemy
			if (transform == em.target) {
				//Correct Node
				//changes target to a random of the childs
				int RandomChild = Random.Range (0, childs.Count);
				em.target = childs [RandomChild];
				//Debug.Log("entered rigth node");
			}
			/*else
	        {
	            Debug.Log("entered wrong node");
	        }*/
		}

        
    }
}
