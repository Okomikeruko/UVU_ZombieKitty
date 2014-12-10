using UnityEngine;
using System.Collections;

public class VictoryScreenGUI : MonoBehaviour {

	public ButtonClass restart, menu, next;
	private PuzzleParser puzzleParser;

	void Start()
	{
		puzzleParser = GameObject.Find ("PuzzleGenerator").GetComponent<PuzzleParser>();
	}

	void OnGUI()
	{
		GUI.depth = 1;

		if(GUI.Button (restart.AnchoredRect(), restart.content, restart.style)){
			Application.LoadLevel(Application.loadedLevel);
		}

		if(GUI.Button (menu.AnchoredRect(), menu.content, menu.style)){
			puzzleParser.currentPuzzle = puzzleParser.getNextPuzzle(puzzleParser.currentPuzzle);
			Application.LoadLevel ("Menu");
		}

		if(GUI.Button (next.AnchoredRect(), next.content, next.style)){
			puzzleParser.currentPuzzle = puzzleParser.getNextPuzzle(puzzleParser.currentPuzzle);
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
