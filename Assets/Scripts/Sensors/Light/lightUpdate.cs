using UnityEngine;
using System.Collections;

public class lightUpdate : MonoBehaviour {

	private valueOf value;
	private GUIText text_message;
	public Light ptLight;
	// Use this for initialization
	void Start () {
		value = GetComponent<valueOf> ();
		text_message = GetComponent<GUIText> ().guiText;
	}
	
	// Update is called once per frame
	void Update () {
		text_message.text = "Light Intensity Level = " + (value.value/10) + "%";
		ptLight.light.intensity = value.value / 800.0f;
	}
}
