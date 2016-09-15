using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	public GameObject toBeFollowed;
	float yOffset;

	// Use this for initialization
	void Start () {
		yOffset = gameObject.transform.position.y - toBeFollowed.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = toBeFollowed.transform.position;
		transform.Translate(Vector3.up * yOffset, Space.World);
		gameObject.transform.rotation = toBeFollowed.transform.rotation;
	}
}
