using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGUI : MonoBehaviour {

	public ButtonClass shotgun, basket, life, timer, pause, help, helpIcon;
	public bool ShotgunMode = false, BasketMode = true, paused = false;
	private bool[] lifeCounter = new bool[4] {true, true, true, true},
				   helpCounter = new bool[4] {true, true, true, true};
	private int helpCount;
	public int lifeCounterOffset, helpCounterOffset;
	private PlayerData playerData;
	private PuzzleWatcher puzzleWatcher;

	void Start()
	{
		playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
		puzzleWatcher = GameObject.Find("PuzzleBuilder").GetComponent<PuzzleWatcher>();
		helpCount = helpCounter.Length;
	}

	void OnGUI()
	{
		GUI.depth = 5;
		GUI.enabled = !puzzleWatcher.end && !paused;

		if (GUI.Button(pause.AnchoredRect (), pause.content, pause.style))
		{
			paused = true;
			Time.timeScale = 0;
			pause.menuObject.SetActive(true);
			GameObject[] clues = GameObject.FindGameObjectsWithTag("Clue");
			foreach(GameObject clue in clues)
			{
				clue.GetComponent<ClueBehavior>().clearClues(true);
			}
		}

		if (GUI.Button(help.AnchoredRect(), help.content, help.style) && anyTrue (helpCounter))
		{
			GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
			List<GameObject> cellList = new List<GameObject>();
			foreach(GameObject cell in cells)
			{
				if(!cell.GetComponent<BoxBehaviour>().isOpen)
				{
					cellList.Add(cell);
				}
			}
			cellList[Random.Range (0, cellList.Count -1)].GetComponent<BoxBehaviour>().open();
			helpCount--;
		}

		for (int i = 0; i < helpCounter.Length; i++)
		{
			helpCounter[i] = (helpCount > i);
			Rect offset = helpIcon.AnchoredRect();
			offset.y += i * helpCounterOffset;
			GUI.Toggle (offset, helpCounter[i], helpIcon.content, helpIcon.style);
		}

		if (playerData.CurrentPlayer.settings.playmode == 1)
		{
			if(GUI.Toggle(shotgun.AnchoredRect(), ShotgunMode, shotgun.content, shotgun.style))
			{
				ShotgunMode = true;
				BasketMode = false;
			}
			if(GUI.Toggle(basket.AnchoredRect(), BasketMode, basket.content, basket.style))
			{
				ShotgunMode = false;
				BasketMode = true;
			}
		}else if(playerData.CurrentPlayer.settings.playmode == 0){
			if(GUI.Button(shotgun.AnchoredRect(), shotgun.content, shotgun.style))
			{
				groupClick ("shotgun");
			}
			if(GUI.Button(basket.AnchoredRect(), basket.content, basket.style))
			{
				groupClick ("hand");
			}
		}


		for (int i = 0; i < lifeCounter.Length; i++)
		{
			lifeCounter[i] = (puzzleWatcher.lives > i);
			Rect offset = life.AnchoredRect();
			offset.y += i * lifeCounterOffset;
			GUI.Toggle (offset, lifeCounter[i], life.content, life.style);
		}

		GUI.Label (timer.AnchoredRect(), timer.content, timer.style);
	}

	void groupClick(string s)
	{
		GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
		foreach (GameObject cell in cells)
		{
			BoxBehaviour b = cell.GetComponent<BoxBehaviour>();
			if(b.highlightPlane.activeSelf){
				b.highlightEvent(s);
				b.highlightPlane.SetActive(false);
			}
		}
	}

	bool anyTrue(bool[] a)
	{
		bool output = false;
		foreach (bool item in a){
			if (item)
				output = true;
		}
		return output;
	}
}
