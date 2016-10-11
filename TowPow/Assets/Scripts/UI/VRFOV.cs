using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VRFOV : MonoBehaviour {

	public Camera topCamera;

	void Awake() {
		setFOVPosition ();
	}

	public void setFOVPosition() {
		//Set position of FOV image
		//Used when position of VR player changes
		Transform VRHead = GameObject.Find("Camera (eye)").transform;
		transform.position = topCamera.WorldToScreenPoint(VRHead.position);
	}
	
	// Update is called once per frame
	void Update () {
		//Get new transform for VR Headset
		Transform VRHead = GameObject.Find("Camera (eye)").transform;
		//Get transform.eulerAngles.y (for world position)
		transform.eulerAngles = new Vector3(0f, 0f, VRHead.eulerAngles.y);
	}
}
