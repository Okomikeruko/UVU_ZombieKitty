using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewUserGUI : MonoBehaviour {

	private string newName = "";
	private PlayerData playerData;

	[SerializeField]
	public ButtonClass Submit;

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
		newName = GUI.TextField (new Rect(10,10,200,20), newName, 25);

		GUI.enabled = newName.Length > 2;

		if(GUI.Button (Submit.rect, Submit.title))
		{
			Player newPlayer = new Player(newName);
			playerData.playerData.players.Add(newPlayer);
			playerData.playerData.setCurrentPlayer(newName);
			playerData.CurrentPlayer = playerData.playerData.players[playerData.playerData.players.Count - 1];
			playerData.SaveData();
			MenuController.ChangeMenu(Submit.menuObject, this.gameObject);
		}
	}
}
