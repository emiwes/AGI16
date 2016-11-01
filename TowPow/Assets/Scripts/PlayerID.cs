using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour {

	public NetworkInstanceId myNetId;

	public override void OnStartLocalPlayer(){
		myNetId = GetComponent<NetworkIdentity> ().netId;
		GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId = myNetId;
		// Debug.Log ("OnStart Player net ID is: " + GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId.ToString());
	}
}
