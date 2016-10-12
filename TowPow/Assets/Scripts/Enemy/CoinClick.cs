using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinClick : NetworkBehaviour {

	public void DestroyCoin(){
		Debug.Log ("Coin was clicked");
		Destroy (this.transform.gameObject);
		CmdIncrementMoney ();

	}

	[Command]
	void CmdIncrementMoney(){
		Debug.Log ("Money is being incremented");
		GameObject.Find ("GameHandler").GetComponent<GameScript> ().moneyCounter += 10;
	}
}
