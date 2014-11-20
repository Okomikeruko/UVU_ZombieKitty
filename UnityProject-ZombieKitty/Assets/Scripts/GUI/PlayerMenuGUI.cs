using UnityEngine;
using System.Collections;

public class PlayerMenuGUI : MonoBehaviour {

	private PlayerData playerData;
	public int PlayerCount = 6;
	[SerializeField]
	public ButtonClass MainMenu;
	public GameObject newPlayer;
	private TouchScreenKeyboard keyboard;

	void Start () {
		playerData = GameObject.Find ("PlayerData").GetComponent <PlayerData>();
	}
	
	void OnGUI() {
		for ( int i = 0; i < PlayerCount; i++ )
		{
			Rect cur = new Rect (30, 30+(40*i), 120, 30);
			Rect del = new Rect (160, 30+(40*i), 50, 30);
			Rect chx = new Rect (10, 40+(40*i), 10, 10);
			if(playerData.playerData != null && i < playerData.playerData.players.Count)
			{
				if(GUI.Button (cur, playerData.playerData.players[i].name))
				{
					playerData.playerData.setCurrentPlayer(playerData.playerData.players[i].name);
					playerData.CurrentPlayer = playerData.playerData.players[i];
					playerData.SaveData();
				}
				if(GUI.Button (del, "Delete" ) && !playerData.playerData.players[i].isCurrent)
				{
					playerData.playerData.players.RemoveAt (i);
					playerData.SaveData();
				}
				if(playerData.playerData.players[i].isCurrent)
				{
					GUI.Box(chx, "");
				}
			}
			else
			{
				if(GUI.Button(cur, "New User"))
				{
					MenuController.ChangeMenu(newPlayer, this.gameObject);
				}
				GUI.enabled = false;
			}
		}
		GUI.enabled = true;
		if(GUI.Button (MainMenu.rect, MainMenu.title))
		{
			MenuController.ChangeMenu(MainMenu.menuObject, this.gameObject);
		}
	}
}
