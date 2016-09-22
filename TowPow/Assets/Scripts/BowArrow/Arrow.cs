﻿using UnityEngine;
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
		if (isFired && transform.GetComponent<Rigidbody>().velocity.magnitude > 1f) {
			transform.LookAt (transform.position + transform.GetComponent<Rigidbody>().velocity);
		}
	}

	public void Fired() {
		isFired = true;
		Destroy (gameObject, 8f);
	}

	private void AttachArrow(){
		var device = SteamVR_Controller.Input ((int)ArrowManager.Instance.trackedObj.index);
		if (!isAttached && device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
			ArrowManager.Instance.AttachBowToArrow();
			isAttached = true;
		}
	}
}
