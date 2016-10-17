using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour {

	public NetworkInstanceId myNetId;
	[SyncVar] public string playerUniqueIdentity;

	public override void OnStartLocalPlayer(){
		myNetId = GetComponent<NetworkIdentity> ().netId;
		GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId = myNetId;
		Debug.Log ("OnStart Player net ID is: " + GameObject.Find ("LocalPlayerNetId").GetComponent<LocalPlayerNetId> ().netId.ToString());

//		GetNetIdentity ();
//
//		SetIdentity ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
//	void Update () {
//		if(transform.name == "" || transform.name == "PlayerObject(Clone)"){
//			SetIdentity ();
//		}
//	}

	void SetIdentity(){
		if (!isLocalPlayer) {
			transform.name = playerUniqueIdentity;
		} 
		else {
			transform.name = MakeUniqueIdentity ();
		}
	}


	void GetNetIdentity(){
		myNetId = GetComponent<NetworkIdentity> ().netId;
		CmdTellServerMyNetId (MakeUniqueIdentity());
	}

	string MakeUniqueIdentity(){
		string uniqueName = "Player " + myNetId.ToString ();
		return uniqueName;
	}

	[Command]
	void CmdTellServerMyNetId(string name){
		playerUniqueIdentity = name;
	}
}
