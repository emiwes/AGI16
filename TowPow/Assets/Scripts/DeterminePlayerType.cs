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

	 private Color activeButtonColor = new Color (0.1f, 0.8f, 0.2f, 1.0f);
	 private Color defaultButtonColor = new Color (1.0f, 1.0f, 1.0f, 0.5f);

	void Start() {
		topCamera.SetActive (true);
		hudCanvas.SetActive (true);
		viveComponents.SetActive (false);
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

			viveButton.gameObject.GetComponent<Image> ().color = defaultButtonColor;
			pixelSenseButton.gameObject.GetComponent<Image> ().color = activeButtonColor;
        }
        else if (type == "Vive") {
			isVive = true;
			viveComponents.SetActive(true);
			pixelSenseComponents.SetActive(false);
			hudCanvas.GetComponent<CanvasGroup> ().alpha = 0;

			viveButton.gameObject.GetComponent<Image> ().color = activeButtonColor;
			pixelSenseButton.gameObject.GetComponent<Image> ().color = defaultButtonColor;
        } 
		else {
			Debug.LogError ("Invalid client type: "+type);
		}
	}
}