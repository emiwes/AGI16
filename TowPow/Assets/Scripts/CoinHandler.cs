﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinHandler : NetworkBehaviour {

	public Camera topCamera;
	public GameObject coinPrefab;


	[Command]
	public void CmdIncrementMoney(){
		GameObject.Find ("GameHandler").GetComponent<GameScript> ().moneyCounter += 10;
	}

	public void DestroyCoin (GameObject coin){
		Destroy (coin);
	}
}
