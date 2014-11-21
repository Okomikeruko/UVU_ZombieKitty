using UnityEngine;
using System.Collections;

public class PlayMenuGUI : MonoBehaviour {
	
	[SerializeField]
	public ButtonClass MainMenu, PuzzleMode;

	
	void OnGUI(){
		if(GUI.Button (MainMenu.rect, MainMenu.title))
		{
			MenuController.ChangeMenu(MainMenu.menuObject, this.gameObject);
		}
		if(GUI.Button (PuzzleMode.rect, PuzzleMode.title))
		{
			MenuController.ChangeMenu(PuzzleMode.menuObject, this.gameObject);
		}
	}

}
