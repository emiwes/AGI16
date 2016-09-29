using UnityEngine;
using System.Collections;

namespace VRTK
{
    public class GrenadeSprintScript : VRTK_InteractableObject
    {
        public bool grenadeActive = false;

        // Update is called once per frame
        void Update () {
            if (base.IsGrabbed())
            {
                grenadeActive = true;
                Destroy(GetComponent<HingeJoint>());
            }
	        
	    }
    }
}
