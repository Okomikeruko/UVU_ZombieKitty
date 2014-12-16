using UnityEngine;
using System.Collections;

public class DefeatScreenGUI : MonoBehaviour {
	
	public ButtonClass restart, menu;

	void OnGUI()
	{
		if(GUI.Button (restart.AnchoredRect(), restart.content, restart.style)){
			audio.Play();
			Application.LoadLevel(Application.loadedLevel);
		}
		
		if(GUI.Button (menu.AnchoredRect(), menu.content, menu.style)){
			audio.Play();
			Application.LoadLevel ("Menu");
		}
	}
}
