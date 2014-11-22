using UnityEngine;
using System.Collections;

public class PuzzleMenuGUI : MonoBehaviour {

	[SerializeField]
	public ButtonClass BackButton;

	public int levelNum { get; set; }

	private Player currentPlayer;
	private PuzzleParser puzzleParser;

	void OnEnable() {
		currentPlayer = GameObject.Find ("PlayerData").GetComponent<PlayerData>().CurrentPlayer;
		puzzleParser = GameObject.Find("PuzzleGenerator").GetComponent<PuzzleParser>();
	}

	void OnGUI() {
		
		if(GUI.Button (BackButton.rect, BackButton.title))
		{
			MenuController.ChangeMenu(BackButton.menuObject, this.gameObject);
		}
		int i = 0;
		foreach (Puzzle puzzle in puzzleParser.allPuzzles.levels[levelNum].puzzles)
		{
			GUI.enabled = currentPlayer.progress.Level[levelNum].puzzle.Count >= i;
			ButtonClass b = new ButtonClass();
			b.title = puzzle.name;
			b.rect = new Rect(20 + 80*i, 50, 70, 50);
			if(GUI.Button (b.rect, b.title))
			{
				puzzleParser.currentPuzzle = puzzle;
				Application.LoadLevel("Game");
			}
			i++;
		}
	}
}
