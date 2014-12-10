using UnityEngine;
using System.Collections;

public class DefeatScreenGUI : MonoBehaviour {
	
	public ButtonClass restart, menu;

	void OnGUI()
	{
		if(GUI.Button (restart.AnchoredRect(), restart.content, restart.style)){
			Application.LoadLevel(Application.loadedLevel);
		}
		
		if(GUI.Button (menu.AnchoredRect(), menu.content, menu.style)){
			Application.LoadLevel ("Menu");
		}
	}
}
