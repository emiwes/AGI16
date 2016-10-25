using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TowerHandler : NetworkBehaviour {

	[Command]
	public void CmdLevelUp(string towerTag){
		GameObject.Find ("GameHandler").GetComponent<TowerLevelHandler> ().LevelUp(towerTag);
	}
}
