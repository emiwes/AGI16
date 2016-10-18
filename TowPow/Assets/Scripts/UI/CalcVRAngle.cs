using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CalcVRAngle : NetworkBehaviour {

	//public Camera topCamera;
	//public GameObject FOV;
	private float yAngle;

//	void Awake() {
//		setFOVPosition ();
//	}

	public void setFOVPosition(GameObject fieldView) {
		//Set position of FOV image
		//Used when position of VR player changes
		//Transform VRHead = GameObject.Find("Camera (eye)").transform;
		//transform.position = topCamera.WorldToScreenPoint(VRHead.position);
		fieldView.GetComponent<VRFOV> ().VRPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		GameObject FOV = GameObject.Find ("FOV");
		setFOVPosition (FOV);
		//Get new transform for VR Headset
		//Transform VRHead = GameObject.Find("Camera (eye)").transform;
		//Get transform.eulerAngles.y (for world position)
		yAngle = -(transform.eulerAngles.y);
		FOV.GetComponent<VRFOV> ().yAngle = yAngle;
		//transform.eulerAngles = new Vector3(0f, 0f, -VRHead.eulerAngles.y);
	}
}
