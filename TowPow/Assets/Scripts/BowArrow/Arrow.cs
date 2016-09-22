using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	private bool isAttached = false;
    private bool isFired = false;
    public bool inCollider = false;


    void OnTriggerEnter() {
        //Debug.Log("Arrow Trigger Enter");
		//AttachArrowToBow();
	}

	void OnTriggerStay() {
		//AttachArrowToBow();
	}

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
		if (inCollider && !isAttached && device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
			ArrowManager.Instance.AttachBowToArrow();
			isAttached = true;
		}
	}
}
