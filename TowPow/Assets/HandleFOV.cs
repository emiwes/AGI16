using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HandleFOV : NetworkBehaviour {

	[SyncVar (hook = "OnYAngleChange")]
	public float yAngle;

	[SyncVar (hook = "OnVRPosChange")]
	public Vector3 VRPosition;

	private GameObject FOV;
	private float targetYAngle;

	void Start() {
		FOV = GameObject.Find ("FOV");
		//Set server started active in CalcVRAngle

		if (DeterminePlayerType.isVive) {
			CalcVRAngle AngleScript = GameObject.Find("Camera (eye)").GetComponent<CalcVRAngle>();
			AngleScript.GetFOVhandler ();
			AngleScript.serverStarted = true;
		}
	}

	void Update() {
		//On update lerp rotation
		FOV.transform.eulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(FOV.transform.eulerAngles.z, targetYAngle, 0.5f));
	}

	void OnYAngleChange(float yAngle) {
		//Update target rotation
		targetYAngle = yAngle;
		//Mathf.Lerp(FOV.transform.eulerAngles, new Vector3(0f, 0f, yAngle), Time.deltaTime);
		//FOV.transform.eulerAngles = new Vector3(0f, 0f, yAngle);
	}

	void OnVRPosChange(Vector3 VRPosition) {
		//Sets new position
		if(!DeterminePlayerType.isVive){
			Camera topCamera = GameObject.Find("TopCamera").GetComponent<Camera>();
			FOV.transform.position = topCamera.WorldToScreenPoint(VRPosition);
		}
	}
}
