using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HandleFOV : NetworkBehaviour {

	[SyncVar (hook = "OnYAngleChange")]
	public float yAngle;

	[SyncVar (hook = "OnVRPosChange")]
	public Vector3 VRPosition;

	private GameObject FOV;

	void Start() {
		FOV = GameObject.Find ("FOV");
		//Set server started active in CalcVRAngle

		if (DeterminePlayerType.isVive) {
			CalcVRAngle AngleScript = GameObject.Find("Camera (eye)").GetComponent<CalcVRAngle>();
			AngleScript.GetFOVhandler ();
			AngleScript.serverStarted = true;
		}
	}

	void OnYAngleChange(float yAngle) {
		//Update rotation of FOV
		FOV.transform.eulerAngles = new Vector3(0f, 0f, yAngle);
	}

	void OnVRPosChange(Vector3 VRPosition) {
		//Sets new position
		if(!DeterminePlayerType.isVive){
			Camera topCamera = GameObject.Find("TopCamera").GetComponent<Camera>();
			FOV.transform.position = topCamera.WorldToScreenPoint(VRPosition);
		}
	}
}
