using UnityEngine;
using System.Collections;

public class PalmTree: MonoBehaviour {

	Renderer renderer;

	Vector3 speed;
	float acc = 0.05f;

	Vector4 wind;
	float windForce;

	// Use this for initialization
	void Start () {
		speed = new Vector3();

		renderer = GetComponent<Renderer> ();
		float offset = Random.Range (0, 10);
		for (int i = 0; i < renderer.materials.Length; i++) {
			renderer.materials[i].SetFloat ("_Offset", offset);
		}

		// Get all the wind defaultvalues
		//windForce = renderer.material.GetFloat ("_WindForce");
		//wind = renderer.material.GetVector ("_Wind");
	}

	void Update() {
		//float windForceOffset = renderer.material.GetFloat ("_WindForceOffset");
		Vector4 windOffset = renderer.material.GetVector ("_WindOffset");
		windOffset = UpdateTreeOffset (windOffset);
		//renderer.material.SetFloat ("_WindForceOffset", windForceOffset / 1.1f);
		for (int i = 0; i < renderer.materials.Length; i++) {
			renderer.materials [i].SetVector ("_WindOffset", windOffset);
		}
	}

	Vector4 UpdateTreeOffset(Vector4 windOffset) {
		for (int i = 0; i < 3; i++) {
			float a = -(windOffset [i] - speed [i]);
			speed [i] += a;
			speed [i] *= 0.3f;
			windOffset [i] += speed [i];
		}
		//windOffset = windOffset * 0.8f;
		// Debug.Log (speed);
		return windOffset;
	}

	public void SetSpeed(Vector3 newSpeed) {
		speed = newSpeed;
	}
}
