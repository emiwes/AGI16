using UnityEngine;
using System.Collections;

public class CalcVRAngle : MonoBehaviour {

	private float yAngle;

	private HandleFOV FOVhandler;
	public bool serverStarted = false;

	public void GetFOVhandler() {
		FOVhandler = GameObject.Find ("GameHandler").GetComponent<HandleFOV>();
	}

	public void setFOVPosition(HandleFOV FOV) {
		//Set position of FOV image
		//Used when position of VR player changes
		FOV.VRPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (serverStarted) {
			setFOVPosition (FOVhandler);
			//Get new transform for VR Headset
			//Get transform.eulerAngles.y (for world position)
			yAngle = -(transform.eulerAngles.y);
			FOVhandler.yAngle = yAngle;
		}
	}
}
