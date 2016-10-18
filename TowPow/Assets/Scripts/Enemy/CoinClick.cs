using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinClick : NetworkBehaviour {

	public void DestroyCoin(){
		Debug.Log ("Coin was clicked");
		GameObject localPlayer = ClientScene.FindLocalObject (GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId);
		localPlayer.GetComponent<CoinHandler> ().CmdIncrementMoney ();
		localPlayer.GetComponent<CoinHandler>().DestroyCoin (gameObject);
	}
}
