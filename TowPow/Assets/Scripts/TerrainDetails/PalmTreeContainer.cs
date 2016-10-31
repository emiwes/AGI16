using UnityEngine;
using System.Collections;

public class PalmTreeContainer : MonoBehaviour {

	public GameObject container;
	private PalmTree[] palmTrees;

	void Start() {
		palmTrees = container.GetComponentsInChildren<PalmTree> ();
	}

	public PalmTree[] GetPalmTrees() {
		return palmTrees;
	}
}
