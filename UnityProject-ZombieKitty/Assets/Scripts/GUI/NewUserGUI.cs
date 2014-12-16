using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewUserGUI : MonoBehaviour {

	private string newName = "";
	private PlayerData playerData;

	[SerializeField]
	public ButtonClass Submit, Name, Back;

	void Start()
	{
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
		if(playerData.playerData != null && playerData.playerData.players.Count > 0)
		{
			this.gameObject.SetActive(false);
		}else{
			playerData.playerData = new Players();
			GameObject.Find("MainMenu").SetActive(false);
		}
	}

	void OnGUI()
	{
		GUI.skin.settings.cursorColor = Color.black;
		newName = GUI.TextField (Name.AnchoredRect(), newName, 25, Name.style);

		if(GUI.Button (Back.AnchoredRect(), Back.content, Back.style))
		{
			audio.Play();
			MenuController.ChangeMenu (Back.menuObject, this.gameObject);
		}

		GUI.enabled = newName.Length > 2;

		if(GUI.Button (Submit.AnchoredRect (), Submit.content, Submit.style))
		{
			audio.Play();
			Player newPlayer = new Player(newName);
			playerData.playerData.players.Add(newPlayer);
			playerData.playerData.setCurrentPlayer(newName);
			playerData.CurrentLevel = 0;
			playerData.CurrentPlayer = playerData.playerData.players[playerData.playerData.players.Count - 1];
			playerData.SaveData();
			newName = "";
			MenuController.ChangeMenu(Submit.menuObject, this.gameObject);
		}
	}
}
