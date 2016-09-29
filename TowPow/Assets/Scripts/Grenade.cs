using UnityEngine;
using System.Collections;

namespace VRTK
{
    public class Grenade : VRTK_InteractableObject
    {
	    // Update is called once per frame
	    void Update () {
            if (GetComponentInChildren<GrenadeSprintScript>().grenadeActive)
            {
                Debug.Log("BOOM");
            }
	
	    }
    }
}
