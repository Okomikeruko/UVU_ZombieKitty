using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {

	[SerializeField]
	public ButtonClass Play, Options, User;

	void OnGUI() {

		if(GUI.Button (Play.rect, Play.title))
		{
			MenuController.ChangeMenu(Play.menuObject, this.gameObject);
		}

		if(GUI.Button (Options.rect, Options.title))
		{
			MenuController.ChangeMenu(Options.menuObject, this.gameObject);
		}

		if(GUI.Button (User.rect, User.title))
		{
			MenuController.ChangeMenu (User.menuObject, this.gameObject);
		}

	}
}

