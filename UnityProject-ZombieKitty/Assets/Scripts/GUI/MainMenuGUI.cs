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
			Debug.Log ("Options");
		}

		if(GUI.Button (User.rect, User.title))
		{
			Debug.Log ("User");
		}
	}
}

