using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public GUIStyle style;
	public GUIStyle stylebg;

	void OnGUI() {
		GUI.contentColor = Color.black;
		GUILayout.BeginArea (new Rect((Screen.width/2)-350, (Screen.height/2) -200, 700, 400), stylebg);
		GUILayout.Label ("");
		GUILayout.Label ("University of Queensland", style);
		GUILayout.Label ("");
		GUILayout.Label ("Centre for Educational Innovation and Technology", style);
		GUILayout.Label ("");
		GUILayout.Label ("3D Data Visualisation", style);
		GUILayout.Label ("");
		GUILayout.Label ("Developed by Craig Knott", style);
		GUILayout.Label ("");
		if (GUILayout.Button ("Start Visualisation")) {
			Application.LoadLevel(1);
		}

		GUILayout.EndArea ();
	}
}
