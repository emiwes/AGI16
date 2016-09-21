using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	private bool isAttached = false;

	private bool isFired = false;

	void OnTriggerEnter() {
		AttachArrow ();
	}

	void OnTriggerStay() {
		AttachArrow ();
	}

	void Update() {
		if (isFired) {
			transform.LookAt (transform.position + transform.GetComponent<Rigidbody>().velocity);
		}
	}

	public void Fired() {
		isFired = true;
	}

	private void AttachArrow(){
		var device = SteamVR_Controller.Input ((int)ArrowManager.Instance.trackedObj.index);
		if (!isAttached && device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
			ArrowManager.Instance.AttachBowToArrow();
			isAttached = true;
		}
	}
}
