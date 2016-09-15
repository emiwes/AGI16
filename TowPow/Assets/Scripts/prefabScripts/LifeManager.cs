using UnityEngine;
using System.Collections;

public class LifeManager : MonoBehaviour {

    public GameObject endPosition;
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

        if (transform.position == endPosition.transform.position)
        {
            //reached goal!
            //Debug.Log("reached end");
            //TODO; action when creep reaches goal
            Kill();
        }

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
