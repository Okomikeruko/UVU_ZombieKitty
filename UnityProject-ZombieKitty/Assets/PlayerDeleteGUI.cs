using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerDeleteGUI : MonoBehaviour {
	
	private PlayerData playerData;
	private PlayerMenuGUI playerMenuGUI;
	private int PlayerCount = 6,
	down = 3;
	public int guiDepth = 1;
	
	void Start () {
		playerData = GameObject.Find ("PlayerData").GetComponent <PlayerData>();
		playerMenuGUI = this.gameObject.GetComponent<PlayerMenuGUI>();
	}
	
	void OnGUI() {
		GUI.depth = guiDepth;
		for ( int i = 0; i < PlayerCount; i++ )
		{
			Rect cur = playerMenuGUI.player.AnchoredRect();
			cur.y += cur.height * (i % down);
			cur.x += cur.width * (i / down);
			
			Rect del = playerMenuGUI.delete.AnchoredRect();
			del.y += cur.height * (i % down);
			del.x += cur.width * (i / down);
			
			if(playerData.playerData != null && i < playerData.playerData.players.Count)
			{
				if(GUI.Button (del, playerMenuGUI.delete.content, playerMenuGUI.delete.style )){
					if (!playerData.playerData.players[i].isCurrent)
					{
						if(EditorUtility.DisplayDialog (
							"Warning!",
							"You are about to delete a player. This cannot be undone.",
							"Confirm",
							"Cancel")){
							playerData.playerData.players.RemoveAt (i);
							playerData.SaveData();
						}
					}
					else
					{
						if(EditorUtility.DisplayDialog(
							"Notice:",
							"You cannot delete the current player.",
							"OK")){}
					}
					
				}
/*
				if(GUI.Toggle (cur, playerData.playerData.players[i].isCurrent, playerData.playerData.players[i].name, player.style))
				{
					playerData.playerData.setCurrentPlayer(playerData.playerData.players[i].name);
					playerData.CurrentPlayer = playerData.playerData.players[i];
					playerData.SaveData();
				}
				
			*/}/*
			else
			{
				if(GUI.Button(cur, "New Player", player.style))
				{
					MenuController.ChangeMenu(newPlayer, this.gameObject);
				}
				GUI.enabled = false;
			}
		}
		GUI.enabled = true;
		if(GUI.Button (MainMenu.AnchoredRect(), MainMenu.content, MainMenu.style))
		{
			MenuController.ChangeMenu(MainMenu.menuObject, this.gameObject);*/
		}
	}
}
