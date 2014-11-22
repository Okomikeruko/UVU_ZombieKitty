using UnityEngine;
using System.Collections;

public class LevelMenuGUI : MonoBehaviour {

	[SerializeField]
	public ButtonClass MainMenu;

	[SerializeField]
	private GameObject PuzzleMenuObject;

	private Player currentPlayer;
	private PuzzleParser puzzleParser;
	private PuzzleMenuGUI puzzleMenuGUI;
	private PlayerData playerData;

	void OnEnable() {
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
		puzzleParser = GameObject.Find("PuzzleGenerator").GetComponent<PuzzleParser>();
		puzzleMenuGUI = PuzzleMenuObject.GetComponent<PuzzleMenuGUI>();
		currentPlayer = playerData.CurrentPlayer;
	}

	void OnGUI() {

		if(GUI.Button (MainMenu.rect, MainMenu.title))
		{
			MenuController.ChangeMenu(MainMenu.menuObject, this.gameObject);
		}

		int i = 0;
		foreach(Level level in puzzleParser.allPuzzles.levels)
		{
			GUI.enabled = currentPlayer.progress.Level.Count > i;
			if(GUI.Button (new Rect (10 + (130*i), 60 ,120,40), level.levelnum.ToString()))
			{
				playerData.CurrentLevel = puzzleMenuGUI.levelNum = i;
				MenuController.ChangeMenu(PuzzleMenuObject, this.gameObject);
			}
			i++;
		}

	}
}
