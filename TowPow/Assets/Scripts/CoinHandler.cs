using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinHandler : NetworkBehaviour {
	// TODO ta bort kanske när allt funkar lol
	public Camera topCamera;
	public GameObject coinPrefab;


	[Command]
	public void CmdIncrementMoney(){
		GameObject.Find ("GameHandler").GetComponent<GameScript> ().moneyCounter += 10;
	}

	[Command]
	public void CmdDecrementMoney(int cash){
		GameObject.Find ("GameHandler").GetComponent<GameScript> ().moneyCounter -= cash;
	}

	public void DestroyCoin (GameObject coin){
		Destroy (coin);
	}
}
