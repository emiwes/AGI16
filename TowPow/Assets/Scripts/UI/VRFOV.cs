using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class VRFOV : NetworkBehaviour {

    [SyncVar (hook = "OnYAngleChange")]
    public float yAngle;

	[SyncVar (hook = "OnVRPosChange")]
	public Vector3 VRPosition;

    void OnYAngleChange(float yAngle) {
		//Update rotation of FOV
        transform.eulerAngles = new Vector3(0f, 0f, yAngle);
    }

	void OnVRPosChange(Vector3 VRPosition) {
		//Sets new position
		Camera topCamera = GameObject.Find("TopCamera").GetComponent<Camera>();
		transform.position = topCamera.WorldToScreenPoint(VRPosition);
	}
}
