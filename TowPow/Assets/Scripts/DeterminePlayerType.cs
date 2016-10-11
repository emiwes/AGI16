using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DeterminePlayerType : NetworkBehaviour {

	GameObject pixelSenseComponents;
	GameObject viveComponents;
	GameObject pixelSenseButton;
	GameObject viveButton;
	GameObject hudCanvas;

	bool showUI = true;

	void Start() {
		pixelSenseComponents = GameObject.Find ("PixelSenseComponents");
		viveComponents = GameObject.Find ("ViveComponents");
		viveButton = GameObject.Find ("ViveButton");
		pixelSenseButton = GameObject.Find ("PixelSenseButton");
		hudCanvas = GameObject.Find ("HUDCanvas");
	}

	void Update() {
		if (Input.GetKeyUp (KeyCode.U)) {
			toggleUiDisplay ();
		} else if (Input.GetKeyUp(KeyCode.P))
        {
            pixelSenseComponents.SetActive(true);
			pixelSenseComponents.transform.Find("TopCamera").gameObject.SetActive(true);
            //viveComponents.SetActive(false);
            //viveComponents.SetActive(true);
            //viveComponents.transform.Find("Camera (eye)").gameObject.SetActive(true);
            //viveComponents.transform.Find("Controller (left)").gameObject.SetActive(false);
            //viveComponents.transform.Find("Controller (right)").gameObject.SetActive(false);
            hudCanvas.SetActive(true);
            setHostInGameScript();

        }
        else if (Input.GetKeyUp(KeyCode.V))
        {
            viveComponents.SetActive(true);
			hudCanvas.SetActive(false);
            pixelSenseComponents.SetActive(false);
            setHostInGameScript();

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
            pixelSenseComponents.transform.Find("TopCamera").gameObject.SetActive(true);

            //viveComponents.SetActive (false);
            viveComponents.SetActive(true);
            viveComponents.transform.Find("Camera (eye)").gameObject.SetActive(true);
            setHostInGameScript();

        }
        else if (type == "Vive") {
			viveComponents.SetActive (true);
			pixelSenseComponents.SetActive (false);
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