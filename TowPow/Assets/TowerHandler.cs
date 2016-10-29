using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TowerHandler : NetworkBehaviour {

	[Command]
	public void CmdLevelUp(string towerTag){
		TowerLevelSynchronize towerLevelSynchronize = GameObject.Find ("GameHandler").GetComponent<TowerLevelSynchronize> ();
		Debug.Log ("CmdLevelUp called");
		if (towerTag == "red") {
			towerLevelSynchronize.towerRedLevel += 1;
		} else if (towerTag == "blue") {
			towerLevelSynchronize.towerBlueLevel += 1;
		} else if (towerTag == "white") {
			towerLevelSynchronize.towerWhiteLevel += 1;
		} else if (towerTag == "black") {
			towerLevelSynchronize.towerBlackLevel += 1;
		} else {
			Debug.Log ("Invalid tower upgrade name " + towerTag);
		}
	}
}
