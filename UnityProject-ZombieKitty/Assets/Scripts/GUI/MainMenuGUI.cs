using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {

	[SerializeField]
	public ButtonClass Play, Options, User;
	private Player currentPlayer;

	void OnEnable(){
		currentPlayer = GameObject.Find ("PlayerData").
			GetComponent<PlayerData>().
				CurrentPlayer;
	}

	void OnGUI() {
		GUI.Label (new Rect(50,50,200,30), "Current Player: " + currentPlayer.name);

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

