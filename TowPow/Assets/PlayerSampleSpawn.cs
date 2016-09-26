using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSampleSpawn : NetworkBehaviour {

	[Command]
	void CmdSpawnSample(GameObject sample){
		NetworkServer.Spawn(sample);
	}
}
