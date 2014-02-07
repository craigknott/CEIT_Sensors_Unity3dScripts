using UnityEngine;
using System.Collections;

#if UNITY_ANDROID

public class TouchRotate : TouchLogicV2 {
	
	public float rotateSpeed = 20.0f;
	public int invert = 1;
	public Transform me;

	private float verticle = 0.0f,
	horizontal = 0.0f;
	private Vector3 oRotation;

	// Use this for initialization
	void Start () {
		oRotation = Camera.main.transform.rotation.eulerAngles;
		verticle = oRotation.x;
		horizontal = oRotation.y;
	}

	public override void OnTouchBeganAnywhere() {
		touch2Watch = TouchLogicV2.currTouch;
	}

	public override void OnTouchMovedAnywhere() {
		verticle -= Input.GetTouch (touch2Watch).deltaPosition.y * rotateSpeed * invert * Time.deltaTime;
		horizontal += Input.GetTouch (touch2Watch).deltaPosition.x * rotateSpeed * invert * Time.deltaTime;

		verticle = Mathf.Clamp (verticle, -80, 80);
		oRotation = new Vector3 (verticle, horizontal, 0.0f);
		Camera.main.transform.eulerAngles = oRotation;
	}

	public override void OnTouchEndedAnywhere() {
		if (TouchLogicV2.currTouch == touch2Watch || Input.touches.Length <= 0) {
			touch2Watch = 64;
		}
	}

	void Update() {
		CheckTouches ();
	}
}

#endif