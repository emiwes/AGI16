using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CoinClick : MonoBehaviour {
	private float destroyTimer = 5.0f;
	GameObject localPlayer;

	private bool coinBlinking = false;
	private float coinAlpha = .7f;
	private float blinkTime = 0.3f;

	void Start() {
		localPlayer = ClientScene.FindLocalObject (GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId);
	}

	public void DestroyCoin(){
		localPlayer.GetComponent<CoinHandler> ().CmdIncrementMoney ();
		localPlayer.GetComponent<CoinHandler>().DestroyCoin (gameObject);
	}

	void coinBlinkAnimation() {
		//Blinks coin on set interval
		Color coinColor = gameObject.GetComponent<Image>().color;
		coinColor.a = coinAlpha;
		gameObject.GetComponent<Image> ().color = coinColor;
		//Change coinAlpha value
		if (coinAlpha == 1f) {
			coinAlpha = .7f;
		}
		else {
			coinAlpha = 1f;
		}
	}

	void Update() {
		destroyTimer -= Time.deltaTime;
		if (destroyTimer <= 0) {
			localPlayer.GetComponent<CoinHandler>().DestroyCoin (gameObject);
		}
		else if (destroyTimer <= 2.5f && !coinBlinking) {
			//Coin sprite blinking
			//gameObject.GetComponent<Image>().
			coinBlinking = true;
			InvokeRepeating("coinBlinkAnimation", 0f, blinkTime);
		}
	}
}
