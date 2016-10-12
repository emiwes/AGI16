using UnityEngine;
using System.Collections;

public class CoinClick : MonoBehaviour {


	public void DestroyCoin(){
		Debug.Log ("Coin was clicked");
		Destroy (this.transform.gameObject);
		GameObject.Find ("GameHandler").GetComponent<GameScript> ().moneyCounter += 10;
	}
}
