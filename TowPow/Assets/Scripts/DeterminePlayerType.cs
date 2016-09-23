using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DeterminePlayerType : MonoBehaviour {

	GameObject pixelSenseComponents;
	GameObject viveComponents;
	GameObject pixelSenseButton;
	GameObject viveButton;

	bool showUI = true;

	void Start() {
		pixelSenseComponents = GameObject.Find ("PixelSenseComponents");
		viveComponents = GameObject.Find ("ViveComponents");
		viveButton = GameObject.Find ("ViveButton");
		pixelSenseButton = GameObject.Find ("PixelSenseButton");
	}

	void Update() {
		if (Input.GetKeyUp (KeyCode.U)) {
			toggleUiDisplay ();
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
			pixelSenseComponents.SetActive (true);
			viveComponents.SetActive (false);
            setHostInGameScript(false);

        }
        else if (type == "Vive") {
			viveComponents.SetActive (true);
			pixelSenseComponents.SetActive (false);
            setHostInGameScript(true);

        } else {
			Debug.LogError ("Invalid client type: "+type);
		}
	}

    void setHostInGameScript(bool isHost)
    {
        Debug.Log("sets host to: " + isHost);
        GameObject.Find("GameHandler").GetComponent<GameScript>().isHost = isHost;
    }
}