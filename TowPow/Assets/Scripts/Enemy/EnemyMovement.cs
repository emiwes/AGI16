using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
    // [HideInInspector]
    public Transform target;
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if(NetworkServer.active)
        agent.SetDestination(target.position);
	
	}
}
