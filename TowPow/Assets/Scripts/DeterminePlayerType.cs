using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DeterminePlayerType : NetworkBehaviour {

	public GameObject pixelSenseComponents;
	public GameObject viveComponents;
	public GameObject pixelSenseButton;
	public GameObject viveButton;
	public GameObject hudCanvas;

	public GameObject topCamera;

	public static bool isVive;

	bool showUI = true;

	void Start() {
		topCamera.SetActive (true);
		viveComponents.SetActive (false);
		hudCanvas.SetActive (true);
	}

	void Update() {
		if (Input.GetKeyUp (KeyCode.U)) {
			toggleUiDisplay ();
		} 
		else if (Input.GetKeyUp(KeyCode.P)){
			SetDeviceEnvironment ("PixelSense");
        } 
		else if (Input.GetKeyUp(KeyCode.V)){
			SetDeviceEnvironment ("Vive");
        }
    }

	void toggleUiDisplay() {
		showUI = !showUI;
		pixelSenseButton.SetActive (showUI);
		viveButton.SetActive (showUI);
		GetComponent<NetworkManagerHUD> ().showGUI = showUI;

	}

	public void SetDeviceEnvironment(string type){
		if (type == "PixelSense") {
			isVive = false;
			viveComponents.SetActive(false);
			pixelSenseComponents.SetActive (true);
			hudCanvas.GetComponent<CanvasGroup> ().alpha = 1;
        }
        else if (type == "Vive") {
			isVive = true;
			hudCanvas.GetComponent<CanvasGroup> ().alpha = 0;
			viveComponents.SetActive(true);
			//hudCanvas.SetActive(false);
			pixelSenseComponents.SetActive(false);
        } else {
			Debug.LogError ("Invalid client type: "+type);
		}
	}
}