using UnityEngine;
using System.Collections;

public class bounce : MonoBehaviour {

	private Transform 	xform;

	private float 		startTime,
						startTimer,
						speed = 1.0f,
						journeyLength;

	private Vector3		start,
						stop;

	public GameObject 	prefab;
	// Use this for initialization
	void Start () {
		xform = GetComponent<Transform> ();
		float x = Random.Range (-3.0F, 3.0F);
		float y = Random.Range (-3.0F, 3.0F);
		start = new Vector3 (x, y, -1.0f);
		stop = new Vector3 (x, y, 1.0f);
		journeyLength = Vector3.Distance (start, stop);
		startTime = Time.time;
		startTimer = startTime;
	}
	
	// Update is called once per frame
	void Update () {

		if (startTimer < Time.time - 300.0f) {
			DestroyObject(xform.gameObject);
		}
		//LERP DAT BOUNCE
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		xform.localPosition = Vector3.Lerp (start, stop, fracJourney);

		//SWITCH DAT LERP DIRECTION
		if (fracJourney >= 0.95) {
			startTime = Time.time;
			Vector3 temp = stop;
			stop = start;
			start = temp;
		}
	}
}
