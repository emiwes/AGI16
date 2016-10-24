using UnityEngine;
using System.Collections;



public class UpgradeTower : MonoBehaviour {

	public GameObject tower;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Something(GameObject attr){
		Debug.Log("mjao");
	}

	public void Upgrade(){
		//
		TowerCombat tc = tower.GetComponent<TowerCombat>();
		tc.level += 1;
		Debug.Log(tower.name);
	}


}
