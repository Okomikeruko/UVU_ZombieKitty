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
			GUI.enabled = currentPlayer.progress.Level[levelNum].puzzle.Count > i;
			ButtonClass b = new ButtonClass();
			b.title = puzzle.name;
			b.rect = new Rect(20 + 80*i, 50, 70, 50);
			if(GUI.Button (b.rect, b.title))
			{
				if (currentPlayer.progress.Level[levelNum].puzzle.Count == puzzleParser.allPuzzles.levels[levelNum].puzzles.Count &&
				    levelNum < puzzleParser.allPuzzles.levels.Count - 1)
				{
					currentPlayer.progress.Level.Add(new LevelProgress());
					currentPlayer.progress.Level[levelNum + 1].puzzle.Add (new PuzzleData());
				}
				else if(currentPlayer.progress.Level[levelNum].puzzle.Count == i + 1)
				{
					currentPlayer.progress.Level[levelNum].puzzle.Add(new PuzzleData());
				}
				GameObject.Find ("PlayerData").GetComponent<PlayerData>().SaveData();
			}
			i++;
		}
	}
}
