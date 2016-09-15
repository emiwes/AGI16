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
        if (col.gameObject.tag == "enemy" && col.gameObject.GetComponent<LifeManager>())
        {
            LifeManager lm = col.gameObject.GetComponent<LifeManager>();
            lm.TakeDamage(Damage);
            Destroy(gameObject);
        }



    }


}
