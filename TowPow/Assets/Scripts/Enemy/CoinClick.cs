using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinClick : MonoBehaviour {

	public void DestroyCoin(){
		GameObject localPlayer = ClientScene.FindLocalObject (GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId);
		localPlayer.GetComponent<CoinHandler> ().CmdIncrementMoney ();
		localPlayer.GetComponent<CoinHandler>().DestroyCoin (gameObject);
	}
}
