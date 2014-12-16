using UnityEngine;
using System.Collections;

public class PauseScreenGUI : MonoBehaviour {

	public ButtonClass resume, restart, mainMenu, background;
	private GameGUI gameGUI;

	void OnEnable()
	{
		gameGUI = GameObject.Find ("GameGUI").GetComponent<GameGUI>();
	}

	void OnGUI()
	{
		GUI.Box(background.AnchoredRect(), background.content, background.style);

		if (GUI.Button (resume.AnchoredRect(), resume.content, resume.style))
		{
			audio.Play();
			restoreGame();
		}

		if (GUI.Button (restart.AnchoredRect(), restart.content, restart.style))
		{
			audio.Play();
			restoreGame ();
			Application.LoadLevel(Application.loadedLevel);
		}

		if (GUI.Button (mainMenu.AnchoredRect(), mainMenu.content, mainMenu.style))
		{
			audio.Play();
			restoreGame ();
			Application.LoadLevel ("Menu");
		}

	}

	void restoreGame()
	{
		gameGUI.paused = false;
		Time.timeScale = 1;
		this.gameObject.SetActive(false);
		GameObject[] clues = GameObject.FindGameObjectsWithTag("Clue");
		foreach(GameObject clue in clues)
		{
			clue.GetComponent<ClueBehavior>().clearClues(false);
		}
	}
}
