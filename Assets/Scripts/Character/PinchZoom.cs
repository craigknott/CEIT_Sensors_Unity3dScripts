using UnityEngine;
using System.Collections;

#if UNITY_ANDROID

public class PinchZoom : TouchLogicV2 {

	//ZOOM STUFF
	public float zoomSpeed = 20.0f;

	Vector2 currTouch1 = Vector2.zero,
	lastTouch1 = Vector2.zero,
	currTouch2 = Vector2.zero,
	lastTouch2 = Vector2.zero;

	private float currDist = 0.0f,
	lastDist = 0.0f,
	zoomFactor = 0.0f;

	public override void OnTouchMovedAnywhere() {
		Zoom ();
	}

	public override void OnTouchStayedAnywhere() {
		Zoom ();
	}

	void Zoom() {
		switch(TouchLogicV2.currTouch) {
			case 0:
				currTouch1 = Input.GetTouch(0).position;
				lastTouch1 = currTouch1 - Input.GetTouch(0).deltaPosition;
				break;
			case 1:
				currTouch2 = Input.GetTouch (1).position;
				lastTouch2 = currTouch2 - Input.GetTouch(1).deltaPosition;
				break;
		}

		if (TouchLogicV2.currTouch >= 1) {
			currDist = Vector2.Distance (currTouch1, currTouch2);
			lastDist = Vector2.Distance (lastTouch1, lastTouch2);
		} else {
			currDist = 0.0f;
			lastDist = 0.0f;
		}

		zoomFactor = Mathf.Clamp (lastDist - currDist, -50.0f, 50.0f);

		Camera.main.transform.Translate(Vector3.back * zoomFactor * zoomSpeed * Time.deltaTime);
	}

	//ROTATE STUFF
	public float rotateSpeed = 5.0f;
	public int invert = 1;
	public Transform me;
	
	private float verticle = 0.0f,
	horizontal = 0.0f;
	private Vector3 oRotation;
	
	// Use this for initialization
	void Start () {
		oRotation = me.eulerAngles;
		verticle = oRotation.x;
		horizontal = oRotation.y;
	}
	
	public override void OnTouchBegan() {
		touch2Watch = TouchLogicV2.currTouch;
	}
	
	public override void OnTouchMoved() {
		verticle -= Input.GetTouch (touch2Watch).deltaPosition.y * rotateSpeed * invert * Time.deltaTime;
		horizontal += Input.GetTouch (touch2Watch).deltaPosition.x * rotateSpeed * invert * Time.deltaTime;
		
		verticle = Mathf.Clamp (verticle, -89, 89);
		
		me.eulerAngles = new Vector3 (verticle, horizontal, 0.0f);
	}
	
	public override void OnTouchEndedAnywhere() {
		if (TouchLogicV2.currTouch == touch2Watch || Input.touches.Length <= 0) {
			touch2Watch = 64;
		}
	}

	void Update() {
		CheckTouches();
	}
}

#endif