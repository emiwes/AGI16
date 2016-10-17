using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class VRFOV : NetworkBehaviour {

    [SyncVar (hook = "OnYAngleChange")]
    public float yAngle;

	[SyncVar (hook = "OnVRPosChange")]
	public Transform VRposition;

    void OnYAngleChange(float yAngle) {
		//Update rotation of FOV
        transform.eulerAngles = new Vector3(0f, 0f, yAngle);
    }

	void OnVRPosChange(Transform position) {
		//Sets new position

	}
}
