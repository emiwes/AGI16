using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkSetup : MonoBehaviour {
	public NetworkManagerHUD networkHUD;

	public Dropdown clientTypeDropdown;
	public Button showHideUiButton;

	public Camera overviewCamera;
	public Camera firstPersonCamera;

	static public string clientType;

	void Start () {
		// Handler for selection of dropdown item
		clientTypeDropdown.onValueChanged.AddListener(
			delegate 
			{
				clientTypeChangeHandler(clientTypeDropdown);
			}
		);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.U)) {
			toggleUI ();
		}
	}

	public void toggleUI(){
		if (networkHUD.showGUI) {
			networkHUD.showGUI = false;
			clientTypeDropdown.gameObject.SetActive (false);
			showHideUiButton.gameObject.SetActive (false);	
		} else {
			networkHUD.showGUI = true;
			clientTypeDropdown.gameObject.SetActive (true);
			showHideUiButton.gameObject.SetActive (true);
		}

	}

	void clientTypeChangeHandler(Dropdown target){
		string cType = getClientType (target.value);
		clientType = cType;

		if (clientType == "Vive") {
			overviewCamera.enabled = false;
			firstPersonCamera.enabled = true;
		} else {
			overviewCamera.enabled = true;
			firstPersonCamera.enabled = false;
		}
	}

	string getClientType(int dropdownValue)
	{
		if (dropdownValue == 1)
			return "Vive";
		
		if (dropdownValue == 2)
			return "PixelSense";

		return "Undefined";
	}
}
