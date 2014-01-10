using UnityEngine;
using System.Collections;

public class ShowText : MonoBehaviour {

	new public GameObject guiText = null;
	private bool showText = false;
	private ParticleSystem ps;

	void Start()
	{
		ps = GetComponent<ParticleSystem>();
	}

	void Update()
	{
		if (float.Parse (guiText.guiText.text) < 21.0) {
			ps.particleSystem.startColor = Color.blue;
			if (ps.particleSystem.isStopped)
				ps.particleSystem.Play();
		} else if (float.Parse (guiText.guiText.text) > 23.0) {
			ps.particleSystem.startColor = Color.red;
			if (ps.particleSystem.isStopped)
				ps.particleSystem.Play();
		} else {
			ps.particleSystem.Stop();
		}
	}

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
