using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class VRFOV : NetworkBehaviour {

	public Camera topCamera;

    [SyncVar (hook = "OnYAngleChange")]
    public float yAngle;

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
        yAngle = -VRHead.eulerAngles.y;
        //transform.eulerAngles = new Vector3(0f, 0f, -VRHead.eulerAngles.y);
	}
    void OnYAngleChange(float yAngle) {
        transform.eulerAngles = new Vector3(0f, 0f, yAngle);
    }
}
