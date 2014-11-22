using UnityEngine;
using System.Collections;

public class DefeatScreenGUI : MonoBehaviour {
	
	public ButtonClass restart, menu;

	void OnGUI()
	{
		if(GUI.Button (restart.AnchoredRect(), restart.title)){
			Application.LoadLevel(Application.loadedLevel);
		}
		
		if(GUI.Button (menu.AnchoredRect(), menu.title)){
			Application.LoadLevel ("Menu");
		}
	}
}
