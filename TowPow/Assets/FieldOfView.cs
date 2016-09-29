using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FieldOfView : MonoBehaviour {

    public float viewAngleDegree;
    public int length;
    public GameObject canvas;
    public GameObject segmentPrefab;
    private bool isInstatiated = false;
    private GameObject circleSegment;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("left"))
        {
            Vector3 newRot = transform.rotation.eulerAngles;
            newRot.y += 10;
            this.transform.eulerAngles = newRot;
        }else if (Input.GetKeyDown("right"))
        {
            Vector3 newRot = transform.rotation.eulerAngles;
            newRot.y -= 10;
            this.transform.eulerAngles = newRot;
        }
        if (!isInstatiated)
        {
            circleSegment =  instatiateCirclesegment(true);
            isInstatiated = true;
        }
        else
        {
            Vector3 segmentEuler = circleSegment.transform.rotation.eulerAngles;
            //Vector3 boxEuler = this.transform.rotation.eulerAngles;
            //Debug.Log(boxEuler.y + (viewAngleDegree / 2) - 100);
            if (segmentEuler.z != (this.transform.rotation.y + (viewAngleDegree / 2) - 100))
            {
                segmentEuler.x = 0;
                segmentEuler.z = (this.transform.rotation.y + (viewAngleDegree / 2) - 100);


                //update
                circleSegment.transform.rotation = Quaternion.Euler(segmentEuler);
                Debug.Log("updates field of view :" + segmentEuler.ToString());//update circle segment
            }
        }
	
	}

    GameObject instatiateCirclesegment(bool isClockwise)
    {
        Vector3 position = new Vector3(0, 0, 0);
        //translates worldposition to position on the canvas
       
        Vector3 p1 = RectTransformUtility.WorldToScreenPoint(Camera.main, position);

        Vector3 rotation = new Vector3(0,0,0);
        rotation.z = this.transform.rotation.y + (viewAngleDegree/2) - 100; // why -100?!
        Debug.Log("rotation z: " + rotation.ToString());

        
        GameObject segment = Instantiate(segmentPrefab, p1, Quaternion.Euler(rotation)) as GameObject;

        //retrivee component Image script
        Image imageScript = segment.GetComponent<Image>();
        imageScript.fillAmount = viewAngleDegree / 360;
        imageScript.fillClockwise = isClockwise;

        //set the object parent to the canvas
        segment.transform.SetParent(canvas.transform);

        return gameObject;
    }


}
