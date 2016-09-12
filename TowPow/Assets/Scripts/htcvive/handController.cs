using UnityEngine;
using System.Collections;

public class handController : MonoBehaviour {
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;

	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input ((int) trackedObj.index);} }

	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (controller == null) {
			Debug.Log ("Controller not initalized.");
			return;
		}

		if (controller.GetPressDown(gripButton)) {
			Debug.Log ("Grip down!");
		}
		if (controller.GetPressUp(gripButton)) {
			Debug.Log ("Grip up!");
		}

		if (controller.GetPressDown(triggerButton)) {
			Debug.Log ("trigger down!");
		}
		if (controller.GetPressUp(triggerButton)) {
			Debug.Log ("trigger up!");
		}
	}

	private void OnTriggerEnter(Collider collider){
		Debug.Log ("Trigger entered!");
	}

	private void OnTriggerExit(Collider collider){
		Debug.Log ("Trigger exited!");
	}
}
