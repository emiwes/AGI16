using UnityEngine;
using System.Collections;

public class Float : MonoBehaviour {

	public float waterLevel = 10f;
	public float floatHeight = 0.3f;
	public float bounceDamp = 0.5f;

	public Vector3 buoyancyCentreOffset;

	private float forceFactor;
	private Vector3 actionPoint;
	private Vector3 upLift;
	private Rigidbody rigidbody;
	
	// Update is called once per frame

	void Start(){
		rigidbody = GetComponent<Rigidbody> ();	
	}

	void Update () {
		//removeMomentum();
		actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
		forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);

		if(forceFactor > 0f){
			
			//upLift = -Physics.gravity * (forceFactor - rigidbody.velocity.y * bounceDamp);
			upLift = -Physics.gravity * (forceFactor - rigidbody.velocity.y * bounceDamp) * rigidbody.mass;
			rigidbody.AddForceAtPosition(upLift, actionPoint);
		}
	}



	// UNTESTED. Will remove momentum?
	void removeMomentum(){
		if(transform.position.y + rigidbody.centerOfMass.y < 17){
			rigidbody.AddForce (-rigidbody.velocity.x * Time.deltaTime * 100,700 * Time.deltaTime + (-rigidbody.velocity.y * Time.deltaTime * 140),-rigidbody.velocity.z * Time.deltaTime * 100);

			if(transform.position.y > 16.8f){
				rigidbody.AddForceAtPosition (new Vector3(0,50 * Time.deltaTime * -transform.up.y,0),10 * transform.up + transform.position);
			}

			rigidbody.AddTorque(-rigidbody.angularVelocity * 100 * Time.deltaTime);
		}
	}
}