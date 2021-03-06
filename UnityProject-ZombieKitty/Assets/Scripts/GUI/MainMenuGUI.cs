﻿using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {

	[SerializeField]
	public ButtonClass Play, Options, User, Player;
	private Player currentPlayer;

	void OnEnable(){
		currentPlayer = GameObject.Find ("PlayerData").
			GetComponent<PlayerData>().
				CurrentPlayer;
	}

	void OnGUI() {
		if(GUI.Button (Play.AnchoredRect(), Play.content, Play.style))
		{
			audio.Play();
			MenuController.ChangeMenu(Play.menuObject, this.gameObject);
		}

		if(GUI.Button (Options.AnchoredRect(), Options.content, Options.style))
		{
			audio.Play();
			MenuController.ChangeMenu(Options.menuObject, this.gameObject);
		}

		if(GUI.Button (User.AnchoredRect(), User.content, User.style))
		{
			audio.Play();
			MenuController.ChangeMenu (User.menuObject, this.gameObject);
		}
		Player.style.fontSize = Mathf.RoundToInt(24.0F * Screen.height / 458.0F);
		GUI.Label (Player.AnchoredRect(), currentPlayer.name, Player.style);
	}
}

