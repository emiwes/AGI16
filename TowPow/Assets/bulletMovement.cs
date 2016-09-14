using UnityEngine;
using System.Collections;

public class bulletMovement : MonoBehaviour {


    public Vector3 dir = new Vector3(0,0,0);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(dir);
        transform.Translate(dir);
	}

    public void setDir(Vector3 v)
    {
        Debug.Log(dir);
        dir = v;
        Debug.Log(dir);
    }
}
