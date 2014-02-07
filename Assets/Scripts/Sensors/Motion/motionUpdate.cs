using UnityEngine;
using System.Collections;

public class motionUpdate : MonoBehaviour {

	public GameObject bouncychild;
	private valueOf value;
	private float lastSpawnTime;
	private AudioSource mic;
	private bool happendd;

	// Use this for initialization
	void Start () {
		value = GetComponent<valueOf> ();
		happendd = false;
		lastSpawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (value.value < 0.5 && happendd == false && lastSpawnTime < Time.time - 10) {
			happendd = true;
			GameObject childClone = (GameObject) Instantiate (bouncychild, transform.position, transform.rotation);
			childClone.transform.parent = transform;
			lastSpawnTime = Time.time;
		}
		if (value.value >= 0.5) {
			happendd = false;
		}
	}
}
