using UnityEngine;
using System.Collections;

public class TurnOnDepthBuffer : MonoBehaviour {
	[ExecuteInEditMode]
	void OnEnable() {
		GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
	}
}