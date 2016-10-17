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

	public GameObject ViveCamera;
	public GameObject ViveController_Left;
	public GameObject ViveController_Right;

	public static bool isVive;

	bool showUI = true;

	void Start() {
		topCamera.SetActive (true);
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
			hudCanvas.SetActive(true);
            setHostInGameScript();

        }
        else if (type == "Vive") {
			isVive = true;
			viveComponents.SetActive(true);
			hudCanvas.SetActive(false);
			pixelSenseComponents.SetActive(false);
			setHostInGameScript();

        } else {
			Debug.LogError ("Invalid client type: "+type);
		}
	}

    void setHostInGameScript()
    {
        //NetworkServer.active is a state that determines if it is a server running on this client
        // isServer dosen't work on objects without networkIdentity like where this script is placed.
        GameObject.Find("GameHandler").GetComponent<GameScript>().isHost = NetworkServer.active;
		//Update host status on enemy spawner
		GameObject.Find ("spawner").GetComponent<spawnEnemy> ().updateHostStatus (NetworkServer.active);
    }
}