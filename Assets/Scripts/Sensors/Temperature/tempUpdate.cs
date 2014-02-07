using UnityEngine;
using System.Collections;

public class tempUpdate : MonoBehaviour {
	
	private valueOf value;
	private GUIText text_message;
	// Use this for initialization
	void Start () {
		value = GetComponent<valueOf> ();
		text_message = GetComponent<GUIText> ().guiText;
	}
	
	// Update is called once per frame
	void Update () {
		text_message.text = "Temperature = " + value.value + "C";
	}
}
