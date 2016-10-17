using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class VRFOV : NetworkBehaviour {

	public Camera topCamera;

    [SyncVar (hook = "OnYAngleChange")]
    public float yAngle;

	[SyncVar (hook = "OnVRPosChange")]
	public Vector3 VRposition;

    void OnYAngleChange(float yAngle) {
		//Update rotation of FOV
        transform.eulerAngles = new Vector3(0f, 0f, yAngle);
    }

	void OnVRPosChange(Vector3 VRposition) {
		//Sets new position
		transform.position = topCamera.WorldToScreenPoint(VRposition);
	}
}
