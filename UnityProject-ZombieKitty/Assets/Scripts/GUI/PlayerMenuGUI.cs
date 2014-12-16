using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerMenuGUI : MonoBehaviour {

	private PlayerData playerData;
	private int PlayerCount = 6,
				down = 3;
	public int guiDepth = 2;
	[SerializeField]
	public ButtonClass MainMenu, player, delete;
	public GameObject newPlayer;
	private TouchScreenKeyboard keyboard;

	void Start () {
		playerData = GameObject.Find ("PlayerData").GetComponent <PlayerData>();
	}
	
	void OnGUI() {
		
		GUI.depth = guiDepth;
		player.style.fontSize = Mathf.RoundToInt(24.0F * Screen.height / 458.0F);
		for ( int i = 0; i < PlayerCount; i++ )
		{
			Rect cur = player.AnchoredRect();
			cur.y += cur.height * (i % down);
			cur.x += cur.width * (i / down);

			if(playerData.playerData != null && i < playerData.playerData.players.Count)
			{
				GUI.depth = 0;

				if(GUI.Toggle (cur, playerData.playerData.players[i].isCurrent, playerData.playerData.players[i].name, player.style))
				{
					audio.Play();
					playerData.playerData.setCurrentPlayer(playerData.playerData.players[i].name);
					playerData.CurrentPlayer = playerData.playerData.players[i];
					foreach (LevelProgress l in playerData.CurrentPlayer.progress.Level){
						if(l.puzzle.Count > 0) {
							if (l.puzzle[0].puzzleRuns.Count > 0){
								playerData.CurrentLevel = l.levelNum -1;
							}
						}
					}
					playerData.SaveData();
				}

			}
			else
			{
				if(GUI.Button(cur, "New Player", player.style))
				{
					audio.Play();
					MenuController.ChangeMenu(newPlayer, this.gameObject);
				}
				GUI.enabled = false;
			}
		}
		GUI.enabled = true;
		if(GUI.Button (MainMenu.AnchoredRect(), MainMenu.content, MainMenu.style))
		{
			audio.Play();
			MenuController.ChangeMenu(MainMenu.menuObject, this.gameObject);
		}
	}
}
