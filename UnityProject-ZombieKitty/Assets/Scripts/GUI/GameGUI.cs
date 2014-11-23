﻿using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {

	public ButtonClass shotgun, basket, life, timer, pause;
	public bool ShotgunMode = false, BasketMode = true, paused = false;
	private bool[] lifeCounter = new bool[4] {true, true, true, true};
	public int lifeCounterOffset;
	private PlayerData playerData;
	private PuzzleWatcher puzzleWatcher;

	void Start()
	{
		playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
		puzzleWatcher = GameObject.Find("PuzzleBuilder").GetComponent<PuzzleWatcher>();

	}

	void OnGUI()
	{
		GUI.enabled = !puzzleWatcher.end;
		GUI.enabled = !paused;

		if (GUI.Button(pause.rect, pause.content, pause.style))
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
		}else{}


		for (int i = 0; i < 4; i++)
		{
			lifeCounter[i] = (puzzleWatcher.lives > i);
			Rect offset = life.AnchoredRect();
			offset.y += i * lifeCounterOffset;
			GUI.Toggle (offset, lifeCounter[i], life.content, life.style);
		}

		GUI.Label (timer.AnchoredRect(), timer.content, timer.style);
	}
}
