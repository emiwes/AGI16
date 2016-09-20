using UnityEngine;
using System.Collections;

public class TowerSpawn : MonoBehaviour {

	public bool isActive;

	public float spawnMovementSpeed;
	public float spawnMovementDistance;

	private float spawnStartTime;

	private bool moving = false;
	private Vector3 endPos;

	private IEnumerator coroutine;

	// Use this for initialization
	void Start () {
		// Start in despawned state
		isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Despawn() {
		if (moving)
			return;
		isActive = false;
		// Sink into the ground;
		StartCoroutine (MoveOverSeconds (new Vector3 (transform.position.x,  -spawnMovementDistance, transform.position.z), 3f));
		moving = true;
	}

	public void Spawn(Vector3 position) {
		if (moving)
			return;
		// TODO Set active after 5 seconds
		isActive = true;
		Vector3 startPos = new Vector3 (position.x, -spawnMovementDistance, position.z);
		transform.position = startPos;
		StartCoroutine (MoveOverSeconds (position, 3f));
		moving = true;
	}

	public IEnumerator MoveOverSpeed (GameObject objectToMove, Vector3 end, float speed){
		// speed should be 1 unit per second
		while (objectToMove.transform.position != end)
		{
			objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}
	}

	public IEnumerator MoveOverSeconds (Vector3 end, float seconds)
	{
		float elapsedTime = 0;
		Vector3 startingPos = transform.position;
		while (elapsedTime < seconds)
		{
			transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		transform.position = end;
		moving = false;
	}
}
