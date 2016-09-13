using UnityEngine;
using System.Collections;

public class Hit : MonoBehaviour
{

    // Use this for initialization


    //TODO
        //detect end
        //finished script?


    public int startHealthPoints;
    int currentHealthPoints;
    float speed = 1f;

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
        if (Input.GetKeyDown("space")) {
            Debug.Log("space key was pressed");
            Debug.Log("Kills gameobj:  " + gameObject.name);
            Destroy(gameObject);
        }


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
}
