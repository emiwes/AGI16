using UnityEngine;
using System.Collections;

public class ArrowManager : MonoBehaviour {

    public SteamVR_TrackedObject trackedObj ;

    private GameObject currentArrow;

    public GameObject Arrow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        AttachArrow();
	}

    public void AttachArrow()
    {
        if(currentArrow == null)
        {
            currentArrow = Instantiate(Arrow);
            currentArrow.transform.parent = trackedObj.transform;
            currentArrow.transform.localPosition = new Vector3(0f, 0f, .342f);
        }

    }

}