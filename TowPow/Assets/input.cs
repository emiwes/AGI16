using UnityEngine;
using System.Collections;

public class input : MonoBehaviour {


    public GameObject bullet;



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 v3 = Input.mousePosition;
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);

            Debug.Log("mouse key was pressed");
            //GameObject newBullet = Instantiate<GameObject>(bullet);//, transform.position, Quaternion.identity);
            // = Instantiate(bullet, Camera.main.gameObject.transform.position, Quaternion.identity) as GameObject;

            GameObject obj = ((GameObject)Instantiate(bullet, transform.position, transform.rotation)).GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 1);
            obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 1);

            //newBullet.GetComponent<bulletMovement>().setDir(v3);

            //v3.Normalize();
            //Debug.Log(newBullet.name);
            //newBullet.GetComponent<Rigidbody>().velocity = v3;
            //newBullet.GetComponent<Rigidbody>().AddForce(v3);
            // bulletMovement otherScript = newBullet.GetComponent<bulletMovement>();
            //otherScript.dir = v3; 
            //Debug.Log("Kills gameobj:  " + gameObject.name);
            //Destroy(gameObject);
        }

    }
}
