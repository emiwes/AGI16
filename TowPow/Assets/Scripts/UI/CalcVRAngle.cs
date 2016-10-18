using UnityEngine;
using System.Collections;

public class CalcVRAngle : MonoBehaviour {

	//public Camera topCamera;
	public GameObject FOV;
	private float yAngle;

	void Awake() {
		setFOVPosition ();
	}

	public void setFOVPosition() {
		//Set position of FOV image
		//Used when position of VR player changes
		//Transform VRHead = GameObject.Find("Camera (eye)").transform;
		//transform.position = topCamera.WorldToScreenPoint(VRHead.position);

		FOV.GetComponent<VRFOV> ().VRposition = transform;
	}

	// Update is called once per frame
	void Update () {
		setFOVPosition ();
		//Get new transform for VR Headset
		//Transform VRHead = GameObject.Find("Camera (eye)").transform;
		//Get transform.eulerAngles.y (for world position)
		yAngle = -(transform.eulerAngles.y);
		FOV.GetComponent<VRFOV> ().yAngle = yAngle;
		//transform.eulerAngles = new Vector3(0f, 0f, -VRHead.eulerAngles.y);
	}
}
