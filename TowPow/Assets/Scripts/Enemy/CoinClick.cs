using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinClick : NetworkBehaviour {

	public void DestroyCoin(){
		Debug.Log ("Coin was clicked");
		CmdDestroyCoin ();
//		CmdIncrementMoney ();

	}


	[Command]
	void CmdDestroyCoin(){
		Debug.Log ("Destroying coin");
		Destroy (this.transform.gameObject);

		Debug.Log ("Money is being incremented");
		GameObject.Find ("GameHandler").GetComponent<GameScript> ().moneyCounter += 10;
	}

//	[Command]
//	void CmdIncrementMoney(){
//		Debug.Log ("Money is being incremented");
//		GameObject.Find ("GameHandler").GetComponent<GameScript> ().moneyCounter += 10;
//	}
}
