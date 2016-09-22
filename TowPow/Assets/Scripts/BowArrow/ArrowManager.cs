using UnityEngine;
using System.Collections;

public class ArrowManager : MonoBehaviour {

	public static ArrowManager Instance;

    public SteamVR_TrackedObject trackedObj ;

    private GameObject currentArrow;

	public GameObject stringAttachPoint;
	public GameObject arrowStartPoint;
	public GameObject stringStartPoint;

	public AudioClip shootSound;

    public GameObject Arrow;

	private bool isAttached = false;

	private AudioSource source;
	private float volLowRange = .5f;
	private float volHighRange = 1.0f;

	// Use this for initialization
	void Awake () {
		source = GetComponent<AudioSource>();
		if (Instance == null)
			Instance = this;
	}

	void OnDestroy() {
		if (Instance == this)
			Instance = null;
	}
	
	// Update is called once per frame
	void Update () {
        AttachArrow();
		PullString ();
	}

	private void PullString (){
		if (isAttached) {
			float dist = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;
			if (dist <= 0.5f) {
				//If distance not greater than a certain value
				//Possibly have new Vector3 (5f*dist, 0f, dist);
				stringAttachPoint.transform.localPosition = stringStartPoint.transform.localPosition + new Vector3 (10f * dist, 0f, 0f);
				//Fix rotation of arrow
				if (dist * 100 % 5 == 0) {
					//Possibly use an interval in which there is a vibration
					SteamVR_Controller.Input ((int)trackedObj.index).TriggerHapticPulse(250);
				}
			}
			var device = SteamVR_Controller.Input ((int)trackedObj.index);
			if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
                if (dist >= 0.3f)
                    Fire();
                else
                {
                    Destroy(currentArrow);
                    currentArrow = null;
                    stringAttachPoint.transform.position = stringStartPoint.transform.position;
                    isAttached = false;
                }
			}
		}
			
	}

	private void Fire(){
		float dist = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;
        Debug.Log(dist);

        currentArrow.transform.parent = null;
		currentArrow.GetComponent<Arrow> ().Fired ();

		Rigidbody r = currentArrow.GetComponent<Rigidbody> ();
		r.velocity = currentArrow.transform.forward * 25f * dist;
		r.useGravity = true;

		currentArrow.GetComponent<Collider> ().isTrigger = false;

		stringAttachPoint.transform.position = stringStartPoint.transform.position;
		currentArrow = null;
		//Play sound
		float vol = Random.Range (volLowRange, volHighRange);
		source.PlayOneShot(shootSound,vol);

		isAttached = false;
	
	}

    public void AttachArrow()
    {
        if(currentArrow == null)
        {
            currentArrow = Instantiate(Arrow);
            currentArrow.transform.parent = trackedObj.transform;
            currentArrow.transform.localPosition = new Vector3(0f, 0f, .342f);
			currentArrow.transform.localRotation = Quaternion.identity;
        }

    }

	public void AttachBowToArrow() {
		currentArrow.transform.parent = stringAttachPoint.transform;
		currentArrow.transform.localPosition = arrowStartPoint.transform.localPosition;
        Vector3 localPos = currentArrow.transform.localPosition;
        localPos.x = 3.3f;
        currentArrow.transform.localPosition = localPos;
		currentArrow.transform.rotation = arrowStartPoint.transform.rotation;

		isAttached = true;
	}

}