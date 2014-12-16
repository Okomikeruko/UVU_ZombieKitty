using UnityEngine;
using System.Collections;

public class LevelMenuGUI : MonoBehaviour {

	public int level, xOffset, yOffset;
	public bool live;

	[SerializeField]
	public ButtonClass MainMenu;

	[SerializeField]
	public ButtonClass[] PuzzleButtons;

	private Player currentPlayer;
	private PuzzleParser puzzleParser;
	private PlayerData playerData;


	void OnEnable() {
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
		puzzleParser = GameObject.Find("PuzzleGenerator").GetComponent<PuzzleParser>();
		currentPlayer = playerData.CurrentPlayer;
	}

	void OnGUI() {

		if(GUI.Button (MainMenu.AnchoredRect(), MainMenu.content, MainMenu.style))
		{
			audio.Play();
			MenuController.ChangeMenu(MainMenu.menuObject, this.gameObject);
		}

		for (int i = 0; i < 3 ; i++){
			int j = 0;
			foreach (PuzzleData puzzleData in currentPlayer.progress.Level[(3*level)+i].puzzle) {
				switch (level)
				{
				case 0:
					PuzzleButtons[0].scale = 0.5f;
					PuzzleButtons[0].rect.x = -0.36f;
					xOffset = 67;
					break;
				case 1:
					PuzzleButtons[0].scale = 0.48f;
					PuzzleButtons[0].rect.x = -0.42f;
					xOffset = 63;
					break;
				case 2:
					PuzzleButtons[0].scale = 0.38f;
					PuzzleButtons[0].rect.x = -0.42f;
					xOffset = 50;
					break;
				default:
					break;
				}

				Rect buttonRect = PuzzleButtons[0].AnchoredRect();
				buttonRect.x += j * xOffset;
				buttonRect.y += i * yOffset;

				GUIStyle buttonStyle = (live) ? PuzzleButtons[((i*3)+j)%5].style : PuzzleButtons[5].style;
				buttonStyle = (i==0 && j==0) ? PuzzleButtons[0].style : buttonStyle;

				if (GUI.Button (buttonRect, 
				                PuzzleButtons[0].content, 
				                buttonStyle)){
					audio.Play();
					puzzleParser.currentPuzzle = puzzleParser.allPuzzles.levels[(3*level)+i].puzzles[j];
					Application.LoadLevel("Game");
				}

				GUI.enabled = live = puzzleData.puzzleRuns.Count > 0;
				j++;
			}
		}
	}
}
