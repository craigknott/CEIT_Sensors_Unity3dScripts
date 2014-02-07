using UnityEngine;
using System.Collections;

public class micUpdate : MonoBehaviour {
	
	private valueOf value;
	private GUIText text_message;
	private AudioSource mic;
	// Use this for initialization
	void Start () {
		value = GetComponent<valueOf> ();
		text_message = GetComponent<GUIText> ().guiText;
		mic = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		text_message.text = "Sound Pressure Level = " + value.value/7 + "%";
		mic.volume = value.value / 700.0f;
	}
}
