using UnityEngine;
using System.Collections;

public class PirateAnimationController : MonoBehaviour {
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space"))
			anim.Play ("Forgot I was a pirate");
		
		if (Input.GetKeyDown ("v"))
			anim.Play ("Victory Jump");
	}
}
