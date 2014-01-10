using UnityEngine;
using System;
using System.Collections;

public class hide : MonoBehaviour {

	private TextMesh me;
	private bool gumball = false;
	private float t;
	// Use this for initialization
	void Start () {
		me = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (me.renderer.enabled == true && gumball == false) {
			gumball = true;
			t = Time.time + 10;
		}
		if (gumball == true && Time.time > t) {
			me.renderer.enabled = false;
			gumball = false;
		}
	}
}
