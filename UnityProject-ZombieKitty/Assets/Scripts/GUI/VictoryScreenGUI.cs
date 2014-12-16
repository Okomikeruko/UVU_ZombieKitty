using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class VictoryScreenGUI : MonoBehaviour {

	public ButtonClass restart, menu, next;
	private PuzzleParser puzzleParser;
	private ConvoParser convoParser;

	void Start()
	{
		puzzleParser = GameObject.Find ("PuzzleGenerator").GetComponent<PuzzleParser>();
		convoParser = GameObject.Find ("Convo Parser").GetComponent<ConvoParser>();
	}

	void OnGUI()
	{
		GUI.depth = 1;

		if(GUI.Button (restart.AnchoredRect(), restart.content, restart.style)){
			audio.Play();
			Application.LoadLevel(Application.loadedLevel);
		}

		if(GUI.Button (menu.AnchoredRect(), menu.content, menu.style)){
			audio.Play();
			puzzleParser.currentPuzzle = puzzleParser.getNextPuzzle(puzzleParser.currentPuzzle); 
			Application.LoadLevel ("Menu");
		}

		if(GUI.Button (next.AnchoredRect(), next.content, next.style)){
			audio.Play();
			bool isLast = puzzleParser.IsLast(puzzleParser.currentPuzzle);
			if(puzzleParser.isLastPuzzle)
			{
				convoParser.currentScene = convoParser.script.scenes.Last ();
				Application.LoadLevel("Convo");
			}
			else 
			{
				puzzleParser.currentPuzzle = puzzleParser.getNextPuzzle(puzzleParser.currentPuzzle);
			}
			if(isLast){
				convoParser.currentScene = convoParser.script.scenes[puzzleParser.currentLevelIndex];
				Application.LoadLevel("Convo");
			} else {
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}
}
