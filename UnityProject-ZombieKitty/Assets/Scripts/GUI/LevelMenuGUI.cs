using UnityEngine;
using System.Collections;

public class LevelMenuGUI : MonoBehaviour {

	[SerializeField]
	public ButtonClass MainMenu;

	private PuzzleParser puzzleParser;

	void Start() {
		puzzleParser = GameObject.Find("PuzzleGenerator").GetComponent<PuzzleParser>();
	}

	void OnGUI() {

		if(GUI.Button (MainMenu.rect, MainMenu.title))
		{
			MenuController.ChangeMenu(MainMenu.menuObject, this.gameObject);
		}

		int i = 0;
		foreach(Level level in puzzleParser.allPuzzles.levels)
		{
			if(GUI.Button (new Rect (10 + (130*i), 60 ,120,40), level.levelnum.ToString()))
			{
				Debug.Log("Level " + level.levelnum);
			}
			i++;
		}

	}
}
