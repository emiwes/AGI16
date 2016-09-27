using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj;

    private bool isAttached = false;
    private bool isFired = false;

	void Update() {
		if (isFired && transform.GetComponent<Rigidbody>().velocity.magnitude > 1f) {
			transform.LookAt (transform.position + transform.GetComponent<Rigidbody>().velocity);
		}
	}

	public void Fired() {
		isFired = true;
        Destroy (gameObject, 8f);
	}

	public void AttachArrowToBow(){
		var device = SteamVR_Controller.Input ((int)ArrowManager.Instance.trackedObj.index);
		if (!isAttached && device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
			ArrowManager.Instance.AttachBowToArrow();
			isAttached = true;
		}
	}
}
