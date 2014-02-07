using UnityEngine;
using System.Collections;

public class glow : MonoBehaviour {

	public GameObject obj;

	public Material 	cold, 
						hot;
	
	private MeshRenderer m;

	private float 	value = 22.0f,
					speed = 2.0f,
					startTime,
					journeyLength;

	private Transform 	xform;
						
	private Vector3 	start, 
						stop;

	private Light ptLight;

	private int current = 1;

	// Use this for initialization
	void Start () {
		ptLight = GetComponent<Light>();
		m = GetComponent<MeshRenderer> ();
		xform = GetComponent<Transform> ();
		start = new Vector3 (1.3f, 1.3f, 1.3f);
		stop = new Vector3 (4.0f, 4.0f, 4.0f);
		journeyLength = Vector3.Distance (start, stop);
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		value = obj.GetComponent<valueOf> ().value;

		//CHANGE DEM MATERIALS
		if (value < 21.0 && current != 0) {
			ptLight.color = new Color(0, 0, 255, 0);
			m.material = cold;
			m.enabled = true;
			current = 0;
		} else if (value > 23.0 && current != 2) {
			ptLight.color = new Color(255, 0, 0, 0);
			m.material = hot;
			m.enabled = true;
			current = 2;
		} else if (value <= 23.0 && value >= 21.0 && current != 1) {
			ptLight.color = new Color(0, 0, 0, 0);
			m.enabled = false;
			current = 1;
		}

		//LERP DAT PULSATING SPHERE
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		xform.localScale = Vector3.Lerp (start, stop, fracJourney);

		//LERP DAT LIGHT INTENSITY
		ptLight.intensity = Mathf.Lerp (2.0f*start.x, 2.0f*stop.x, fracJourney);

		//SWITCH DAT LERP DIRECTION
		if (fracJourney >= 0.95) {
			startTime = Time.time;
			Vector3 temp = stop;
			stop = start;
			start = temp;
		}
	}
}
