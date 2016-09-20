using UnityEngine;
using System.Collections;

public class TowerSpawn : MonoBehaviour {

	public bool isActive;

	// Use this for initialization
	void Start () {
		isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnAt(Vector3 position){
		Debug.Log (position.ToString());
		transform.position = position;
		isActive = true;
	}
}
