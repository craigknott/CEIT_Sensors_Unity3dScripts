using UnityEngine;
using System.Collections;

public class ShowText : MonoBehaviour {

	public GameObject guiText;
	private bool showText = false;

	void OnMouseDown()
	{
		showText = !showText;
		// If you clicked the object, set showText to true
	}
	
	void OnGUI()
	{
		if(showText)
		{
			guiText.guiText.enabled = true;
		}
		else
		{
			guiText.guiText.enabled = false;
		}
	}
}
