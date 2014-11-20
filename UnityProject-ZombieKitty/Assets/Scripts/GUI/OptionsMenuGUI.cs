using UnityEngine;
using System.Collections;

public class OptionsMenuGUI : MonoBehaviour {
	
	[SerializeField]
	public ButtonClass MainMenu;

	void OnGUI() {
		
		if(GUI.Button (MainMenu.rect, MainMenu.title))
		{
			MenuController.ChangeMenu(MainMenu.menuObject, this.gameObject);
		}

	}
}
