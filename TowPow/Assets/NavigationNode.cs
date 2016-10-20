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
        //change to one of the childs of the node

        //changes target to a random of the childs
        int RandomChild = Random.Range(0, childs.Count);
        col.gameObject.GetComponent<EnemyMovement>().target = childs[RandomChild];
    }
}
