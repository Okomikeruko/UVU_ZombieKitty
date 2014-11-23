using UnityEngine;
using System.Collections;

public class PuzzleWatcher : MonoBehaviour {

	public delegate void Watcher();
	Watcher watcher;

	public int solveCount { get; set; } 
	public int puzzleCount { get; set; }
	public int lives = 4, bites = 0, score = 0;
	public float TimeRemaining = 301;
	public bool end = false, isHighlighting = false;
	public int levelNum;

	public GameObject VictoryScreen, DefeatScreen;

	private GameGUI gameGUI;
	private PlayerData playerData;
	private PuzzleParser puzzleParser;


	void Awake()
	{
		solveCount = 0;
		puzzleCount = 0;
		watcher += victory;
		watcher += defeat;
		watcher += countdown;
	}
	void Start()
	{
		gameGUI = GameObject.Find ("GameGUI").GetComponent<GameGUI>();
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
		puzzleParser = GameObject.Find("PuzzleGenerator").GetComponent<PuzzleParser>();
		levelNum = playerData.CurrentLevel;
	}
	void Update()
	{
		watcher();
	}

	void countdown()
	{
		TimeRemaining -= Time.deltaTime;
		string printTime = (TimeRemaining > 0) ? 
			Mathf.FloorToInt(TimeRemaining / 60).ToString() +
				":" +
					Mathf.FloorToInt(Mathf.Repeat (TimeRemaining, 60)).ToString("D2")
						: "0:00";

		gameGUI.timer.content.text = printTime;
	}

	void victory()
	{
		if (solveCount == puzzleCount)
		{
			Player currentPlayer = playerData.CurrentPlayer;

			PuzzleData finish = new PuzzleData();
			finish.lives = lives;
			finish.besttime = Mathf.FloorToInt(TimeRemaining);
			finish.highscore = score;


			// If the number of saved puzzles is greater than or equal to the puzzle number you just solved, save/overwrite data;
			if (currentPlayer.progress.Level[levelNum].puzzle.Count >= puzzleParser.currentPuzzle.puzzlenum)
			{
				currentPlayer.progress.Level[levelNum].puzzle[puzzleParser.currentPuzzle.puzzlenum - 1] = finish;
			}
			// Otherwise, if the number of saved puzzles is less than the total number of puzzles in that level, add new save data.
			else if (currentPlayer.progress.Level[levelNum].puzzle.Count < puzzleParser.allPuzzles.levels[levelNum].puzzles.Count)
			{
				currentPlayer.progress.Level[levelNum].puzzle.Add (finish);
				if (currentPlayer.progress.Level[levelNum].puzzle.Count == puzzleParser.allPuzzles.levels[levelNum].puzzles.Count)
				{
					currentPlayer.progress.Level.Add (new LevelProgress());
				}
			}
			playerData.SaveData();

			VictoryScreen.SetActive(true);
			stop ();
		}
	}

	void defeat()
	{
		if (lives <= 0 || TimeRemaining <= 0)
		{
			DefeatScreen.SetActive(true);
			stop ();
		}
	}

	void stop()
	{
		watcher = empty;
		end = true;
	}

	void empty () {}
}
