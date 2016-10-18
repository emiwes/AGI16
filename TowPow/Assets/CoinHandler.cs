using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinHandler : NetworkBehaviour {

	public Camera topCamera;
	public GameObject coinPrefab;

	[Command]
	public void CmdSpawnCoin (Vector3 piratePosition){
		GameObject coin = (GameObject)Instantiate(coinPrefab, topCamera.WorldToScreenPoint(piratePosition), Quaternion.identity);
		coin.transform.SetParent(GameObject.Find("HUDCanvas").transform);
		NetworkServer.Spawn (coin);
	}


	[Command]
	public void CmdDestroyCoin(GameObject coin){
		Debug.Log ("Destroying coin");
		Destroy (coin);

		Debug.Log ("Money is being incremented");
		GameObject.Find ("GameHandler").GetComponent<GameScript> ().moneyCounter += 10;
	}
}
