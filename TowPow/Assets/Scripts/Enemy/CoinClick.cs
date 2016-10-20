using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinClick : MonoBehaviour {
	private float destroyTimer = 5.0f;
	GameObject localPlayer;

	void Start() {
		localPlayer = ClientScene.FindLocalObject (GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId);
	}

	public void DestroyCoin(){
		localPlayer.GetComponent<CoinHandler> ().CmdIncrementMoney ();
		localPlayer.GetComponent<CoinHandler>().DestroyCoin (gameObject);
	}

	void Update() {
		destroyTimer -= Time.deltaTime;
		if (destroyTimer <= 0) {
			localPlayer.GetComponent<CoinHandler>().DestroyCoin (gameObject);
		}
	}
}
