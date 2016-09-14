using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour
{

    // Use this for initialization


    //TODO
        //detect end
        //finished script?


    public int startHealthPoints;
    int currentHealthPoints;
    //float speed = 1f;
   

    void Awake()
    {
        currentHealthPoints = startHealthPoints;
    }
    void Start()
    {
        //find zoombie
        //set lives speed etc.
        //healthPoints = 2;
        

    }

    // Update is called once per frame
    void Update()
    {


    }

    //void CollisionDetection 

    void OnCollisionEnter (Collision col)
    {
        //Debug.Log(col.gameObject.name);
        if(col.gameObject.GetComponent<bulletType>() != null)
        {
            currentHealthPoints -= col.gameObject.GetComponent<bulletType>().Damage;
            Debug.Log("collision hp:" + currentHealthPoints);
            Destroy(col.gameObject);

            if (currentHealthPoints <= 0)
            {
                Destroy(gameObject);
            }
        }
        

        
    }

    public void TakeDamage (int Damage)
    {// To be called if a bullet collides with a creep
        currentHealthPoints -= Damage;
        if (currentHealthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
