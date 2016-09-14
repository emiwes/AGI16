using UnityEngine;
using System.Collections;

public class bulletType : MonoBehaviour {

    public int Damage;
 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log(col.gameObject.name);
        if (col.gameObject.GetComponent<HealthScript>() != null)
        {
            HealthScript otherScript = GetComponent<HealthScript>();
            otherScript.TakeDamage(Damage);
            Destroy(gameObject);
        }



    }


}
