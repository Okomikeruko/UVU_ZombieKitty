using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayMenuGUI : MonoBehaviour {
	
	[SerializeField]
	public ButtonClass BeginnerLevels, IntermediateLevels, AdvancedLevels, Back, Resume, NewGame;
	private PlayerData playerData;
	private PuzzleParser puzzleParser;

	void Start(){
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
		puzzleParser = GameObject.Find ("PuzzleGenerator").GetComponent<PuzzleParser>();
	}
	
	void OnGUI(){
		if(GUI.Button (NewGame.AnchoredRect(), NewGame.content, NewGame.style))
		{
			if(EditorUtility.DisplayDialog(
				"Warning!",
				"This will erase ALL your progress! This cannot be undone!",
				"Confirm",
				"Cancel")
				)
			{
				playerData.ResetLevels();
			}
			puzzleParser.currentPuzzle = puzzleParser.allPuzzles.levels[0].puzzles[0];
			Application.LoadLevel("Game");
		}
		if(GUI.Button (Resume.AnchoredRect(), Resume.content, Resume.style))
		{
			int l = playerData.CurrentPlayer.progress.Level.Count - 1;
			int p = playerData.CurrentPlayer.progress.Level[l].puzzle.Count;
			puzzleParser.currentPuzzle = puzzleParser.allPuzzles.levels[l].puzzles[p];
			Application.LoadLevel("Game");
		}
		if(GUI.Button (Back.AnchoredRect(), Back.content, Back.style))
		{
			MenuController.ChangeMenu(Back.menuObject, this.gameObject);
		}
		if(GUI.Button (BeginnerLevels.AnchoredRect(), BeginnerLevels.content, BeginnerLevels.style))
		{

		}
		GUI.enabled = playerData.CurrentPlayer.progress.Level.Count > 3;
		if(GUI.Button (IntermediateLevels.AnchoredRect (), IntermediateLevels.content, IntermediateLevels.style))
		{

		}
		GUI.enabled = playerData.CurrentPlayer.progress.Level.Count > 6;
		if(GUI.Button (AdvancedLevels.AnchoredRect(), AdvancedLevels.content, AdvancedLevels.style))
		{

		}
	}
}
