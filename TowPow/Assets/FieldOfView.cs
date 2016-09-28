using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FieldOfView : MonoBehaviour {

    public float viewAngleDegree;
    public int length;
    public GameObject canvas;
    public GameObject segmentPrefab;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            instatiateCirclesegment(true);
            instatiateCirclesegment(false);
        }
	
	}

    void instatiateCirclesegment(bool isClockwise)
    {
        Vector3 position = new Vector3(0, 0, 0);
        //translates worldposition to position on the canvas
       
        Debug.Log(rt);
        Vector3 p1 = RectTransformUtility.WorldToScreenPoint(Camera.main, position);

        Vector3 rotation = new Vector3(0,0,0);
        rotation.z = this.transform.rotation.y - 100; // why -100?!

        
        GameObject segment = Instantiate(segmentPrefab, p1, Quaternion.Euler(rotation)) as GameObject;

        //retrivee component Image script
        Image imageScript = segment.GetComponent<Image>();
        imageScript.fillAmount = (viewAngleDegree / 2) / 360;
        imageScript.fillClockwise = isClockwise;
        Debug.Log("sets view angle to :" + (viewAngleDegree / 2) / 360);

        //set the object parent to the canvas
        segment.transform.SetParent(canvas.transform);
    }


}
