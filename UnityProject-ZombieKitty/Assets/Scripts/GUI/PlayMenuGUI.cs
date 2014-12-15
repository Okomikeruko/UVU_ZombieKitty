using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PlayMenuGUI : MonoBehaviour {
	
	[SerializeField]
	public ButtonClass BeginnerLevels, IntermediateLevels, AdvancedLevels, Back, Resume, NewGame;
	private PlayerData playerData;
	private PuzzleParser puzzleParser;
	public GameObject LevelMenuObject;
	private LevelMenuGUI levelMenuGUI;

	void Start(){
		levelMenuGUI = LevelMenuObject.GetComponent<LevelMenuGUI>();
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
		puzzleParser = GameObject.Find ("PuzzleGenerator").GetComponent<PuzzleParser>();
	}
	
	void OnGUI(){
		bool noob = playerData.CurrentPlayer.progress.Level[0].puzzle[0].puzzleRuns.Count == 0;
		if(GUI.Button (NewGame.AnchoredRect(), NewGame.content, NewGame.style))
		{
			if (!noob)
			{
				if(EditorUtility.DisplayDialog(
					"Warning!",
					"This will erase ALL your progress! This cannot be undone!",
					"Confirm",
					"Cancel")
					)
				{
					playerData.ResetLevels();
					puzzleParser.currentPuzzle = puzzleParser.allPuzzles.levels[0].puzzles[0];
					Application.LoadLevel("Convo");
				}
			}
			else
			{
				puzzleParser.currentPuzzle = puzzleParser.allPuzzles.levels[0].puzzles[0];
				Application.LoadLevel("Convo");
			}
		}
		GUI.enabled = !noob;
		if(GUI.Button (Resume.AnchoredRect(), Resume.content, Resume.style))
		{
			puzzleParser.currentPuzzle = NextPuzzle();
			Application.LoadLevel("Game");
		}
		GUI.enabled = true;
		if(GUI.Button (Back.AnchoredRect(), Back.content, Back.style))
		{
			MenuController.ChangeMenu(Back.menuObject, this.gameObject);
		}
		if(GUI.Button (BeginnerLevels.AnchoredRect(), BeginnerLevels.content, BeginnerLevels.style))
		{
			levelMenuGUI.level = 0;
			MenuController.ChangeMenu(LevelMenuObject, this.gameObject);
		}
		GUI.enabled = playerData.CurrentPlayer.progress.Level[2].puzzle[9].puzzleRuns.Count > 0;
		if(GUI.Button (IntermediateLevels.AnchoredRect (), IntermediateLevels.content, IntermediateLevels.style))
		{
			levelMenuGUI.level = 1;
			MenuController.ChangeMenu(LevelMenuObject, this.gameObject);
		}
		GUI.enabled = playerData.CurrentPlayer.progress.Level[5].puzzle[11].puzzleRuns.Count > 0;
		if(GUI.Button (AdvancedLevels.AnchoredRect(), AdvancedLevels.content, AdvancedLevels.style))
		{
			levelMenuGUI.level = 2;
			MenuController.ChangeMenu(LevelMenuObject, this.gameObject);
		}
	}

	Puzzle NextPuzzle()
	{
		foreach(LevelProgress level in playerData.CurrentPlayer.progress.Level) {
			foreach(PuzzleData puzzle in level.puzzle) {
				if (puzzle.puzzleRuns.Count == 0){
					return puzzleParser.allPuzzles.levels[level.levelNum-1].puzzles[puzzle.puzzleNum-1];
				}
			}
		}
		return puzzleParser.allPuzzles.levels[8].puzzles[14];
	}
}
