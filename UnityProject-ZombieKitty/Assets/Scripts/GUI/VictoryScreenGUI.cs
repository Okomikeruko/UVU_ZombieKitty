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
		if(GUI.Button (restart.AnchoredRect(), restart.title)){
			Application.LoadLevel(Application.loadedLevel);
		}

		if(GUI.Button (menu.AnchoredRect(), menu.title)){
			Application.LoadLevel ("Menu");
		}

		if(GUI.Button (next.AnchoredRect(), next.title)){
			puzzleParser.currentPuzzle = getNextPuzzle(puzzleParser.currentPuzzle);
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	Puzzle getNextPuzzle(Puzzle p)
	{
		Level currentLevel = null;
		foreach (Level level in puzzleParser.allPuzzles.levels){
			foreach (Puzzle puzzle in level.puzzles){
				if (puzzle == p){
					currentLevel = level;
				}
			}
		}

		if (p.puzzlenum == currentLevel.puzzles.Count){
			if(currentLevel.levelnum != puzzleParser.allPuzzles.levels.Count){
				return puzzleParser.allPuzzles.levels[currentLevel.levelnum].puzzles[0];
			}else{
				return p;
			}
		}else{
			return currentLevel.puzzles[p.puzzlenum];
		}
	}
}
