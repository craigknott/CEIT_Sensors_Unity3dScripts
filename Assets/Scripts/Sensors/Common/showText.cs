using UnityEngine;
using System.Collections;

public class showText : MonoBehaviour {

	public GUIText[] Text;
	private bool show = false;
	
	void OnMouseDown()
	{
		show = !show;
		// If you clicked the object, set showText to true
	}
	
	void OnGUI()
	{
		if(show)
		{
			foreach (GUIText g in Text) {
				g.guiText.enabled = true;
			}
		}
		else
		{
			foreach (GUIText g in Text) {
				g.guiText.enabled = false;
			}
		}
	}
}
