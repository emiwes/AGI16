using UnityEngine;
using System.Collections;

public class InteractableItem : MonoBehaviour {

	public Rigidbody rigidbody;

	private bool currentlyInteracting;
	private HandController attachedHand;
	private Transform interactionPoint;

	private Vector3 positionDelta;
	private Quaternion rotationDelta;
	private float angle;
	private Vector3 axis;

	private float velocityFactor = 20000f;
	private float rotationFactor = 500f;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		interactionPoint = new GameObject().transform;
		velocityFactor /= rigidbody.mass;
		rotationFactor /= rigidbody.mass;
	
	}
	
	// Update is called once per frame
	//TODO: Use FixedUpdate for rigidbody manipulation
	void Update () {
		if( attachedHand && currentlyInteracting){
			positionDelta = attachedHand.transform.position - interactionPoint.position;
			this.rigidbody.velocity = positionDelta * velocityFactor * Time.fixedDeltaTime;

			rotationDelta = attachedHand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
			rotationDelta.ToAngleAxis (out angle, out axis);

			if (angle > 180) {
				angle -= 360;
			}

			this.rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
		}
	
	}

	public void BeginInteraction(HandController hand){
		attachedHand = hand;
		interactionPoint.transform.position = hand.transform.position;
		interactionPoint.transform.rotation = hand.transform.rotation;
		interactionPoint.SetParent(transform, true);

		currentlyInteracting = true;

	}

	public void EndInteraction(HandController hand){
		if(hand == attachedHand){
			attachedHand = null;
			currentlyInteracting = false;
		}
	}

	public bool IsInteracting(){
		return currentlyInteracting;
	}
}