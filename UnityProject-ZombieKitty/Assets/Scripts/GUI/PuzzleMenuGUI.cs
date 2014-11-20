using UnityEngine;
using System.Collections;

public class PuzzleMenuGUI : MonoBehaviour {

	[SerializeField]
	public ButtonClass BackButton;

	public int levelNum { get; set; }

	private PuzzleParser puzzleParser;

	void Start() {
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
			ButtonClass b = new ButtonClass();
			b.title = puzzle.name;
			b.rect = new Rect(20 + 80*i, 50, 70, 50);
			if(GUI.Button (b.rect, b.title))
			{
				Debug.Log( b.title);
			}
			i++;
		}
	}
}
