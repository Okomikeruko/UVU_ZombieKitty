using UnityEngine;
using System.Collections;

public class PuzzleWatcher : MonoBehaviour {

	public delegate void Watcher();
	Watcher watcher;

	public int solveCount { get; set; } 
	public int puzzleCount { get; set; }

	public GameObject VictoryScreen;
	public GameObject DefeatScreen;

	void Awake()
	{
		solveCount = 0;
		puzzleCount = 0;
		watcher = victory;
	}

	void Update()
	{
		watcher();
	}

	void victory()
	{
		if (solveCount == puzzleCount)
		{
			VictoryScreen.SetActive(true);
			watcher = empty;
		}
	}

	void empty () {}
}
