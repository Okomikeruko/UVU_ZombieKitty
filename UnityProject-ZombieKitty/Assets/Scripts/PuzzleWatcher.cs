using UnityEngine;
using System.Collections;

public class PuzzleWatcher : MonoBehaviour {

	public delegate void Watcher();
	Watcher watcher;

	public int solveCount { get; set; } 
	public int puzzleCount { get; set; }
	public int puzzleIndex { get; set; }
	public int lives = 4, bites = 0, score = 0;
	public float TimeRemaining = 301;
	public bool end = false, isOver = false, isHighlighting = false;
	public int levelIndex;

	public GameObject VictoryScreen, DefeatScreen, GameGUIScreen;

	private GameGUI gameGUI;
	private PlayerData playerData;
	private PuzzleParser puzzleParser;
	public Rect OverRect;

	void Awake()
	{
		solveCount = 0;
		puzzleCount = 0;
		puzzleIndex = 0;
		watcher += victory;
		watcher += defeat;
		watcher += countdown;
		watcher += overDetect;
	}
	void Start()
	{
		OverRect = new Rect(Screen.width*0.2f,Screen.height*0.1f, Screen.width*0.6f, Screen.height*0.8f);

		gameGUI = GameObject.Find ("GameGUI").GetComponent<GameGUI>();
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
		puzzleParser = GameObject.Find("PuzzleGenerator").GetComponent<PuzzleParser>();
		levelIndex = playerData.CurrentLevel;
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
		if (solveCount == puzzleCount && (lives > 0 || TimeRemaining > 0)
		    ||
		    Input.GetKeyDown(KeyCode.Q))
		{
			Player currentPlayer = playerData.CurrentPlayer;

			PuzzleRun finish = new PuzzleRun();
			finish.lives = lives;
			finish.besttime = Mathf.FloorToInt(TimeRemaining);
			finish.highscore = score;

			currentPlayer.progress.Level[levelIndex].puzzle[puzzleIndex].puzzleRuns.Add (finish);

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
		GameGUIScreen.SetActive (false);
		end = true;
	}

	void overDetect()
	{
		isOver = OverRect.Contains(Input.mousePosition);
	}

	void empty () {}
}
