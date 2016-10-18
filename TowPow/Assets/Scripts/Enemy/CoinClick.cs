using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinClick : NetworkBehaviour {

	private GameObject localPlayer;
	void Awake(){
		localPlayer = ClientScene.FindLocalObject (GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId);
	}

	public void DestroyCoin(){
		Debug.Log ("Coin was clicked");
		localPlayer.GetComponent<CoinHandler>().CmdDestroyCoin (gameObject);
	}
}
