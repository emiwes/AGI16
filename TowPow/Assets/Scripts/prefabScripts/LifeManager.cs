using UnityEngine;
using System.Collections;

public class LifeManager : MonoBehaviour {

    Vector3 endPosition;
    public int startHealthPoints;
    int currentHealthPoints;


    void Awake()
    {
        currentHealthPoints = startHealthPoints;

        
        
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        /*if ((endPosition.magnitude <= 0) && (this.GetComponent<SplineController>().SplineRoot != null))
        {
            SplineController h = this.GetComponent<SplineController>();
            GameObject sr = h.SplineRoot;

            int nrOfEndP = sr.transform.childCount;


            //Debug.Log(nrOfEndP);
            endPosition = this.GetComponent<SplineController>().SplineRoot.transform.GetChild(nrOfEndP - 1).transform.position;
        }

        if ((transform.position-endPosition).magnitude <= 1)
        {
            //reached goal!
            Debug.Log("reached end");
            //TODO; action when creep reaches goal
            Kill();
        }*/

    }

    public void TakeDamage(int Damage)
    {
        // To be called if a bullet collides with a creep
        currentHealthPoints -= Damage;
        if (currentHealthPoints <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        //probably we would like to do something

        //Destroy the actual creep
        Destroy(gameObject);
    }
}
