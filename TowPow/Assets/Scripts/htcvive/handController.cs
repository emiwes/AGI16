using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandController : MonoBehaviour {
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input ((int) trackedObj.index);} }


	HashSet<InteractableItem> objectsHoveringOver = new HashSet<InteractableItem>();

	private InteractableItem closestItem;
	private InteractableItem interactingItem;

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
			float minDistance = float.MaxValue;
			float distance;

			foreach(InteractableItem item in objectsHoveringOver){
				distance = (item.transform.position - transform.position).sqrMagnitude;
				if(distance < minDistance){
					minDistance = distance;
					closestItem = item;
				}
			}

			interactingItem = closestItem;

			closestItem = null;

			// check if item is interacting when switching hands.
			if(interactingItem){
				if(interactingItem.IsInteracting()){
					interactingItem.EndInteraction(this);
				}

				interactingItem.BeginInteraction(this);
			}
		}

		if (controller.GetPressUp(gripButton) && interactingItem != null) {
			Debug.Log ("Grip up!");
			interactingItem.EndInteraction(this);

		}

		if (controller.GetPressDown(triggerButton)) {
			Debug.Log ("trigger down!");
            Weapon weapon;
            if (weapon = interactingItem.GetComponent<Weapon>()) {
                Debug.Log("Weapon");
                weapon.Fire();
            }
        }
		if (controller.GetPressUp(triggerButton)) {
			Debug.Log ("trigger up!");
		}
	}

	private void OnTriggerEnter(Collider collider){
		Debug.Log ("Trigger entered!");
		InteractableItem collidedItem = collider.GetComponent<InteractableItem> ();
		if (collidedItem) {
			objectsHoveringOver.Add(collidedItem);
		}
	}

	private void OnTriggerExit(Collider collider){
		Debug.Log ("Trigger exited!");
		InteractableItem collidedItem = collider.GetComponent<InteractableItem> ();
		if (collidedItem) {
			objectsHoveringOver.Remove(collidedItem);
		}
	}
}
