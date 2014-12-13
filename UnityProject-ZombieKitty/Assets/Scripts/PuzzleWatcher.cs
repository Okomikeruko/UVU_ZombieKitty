using UnityEngine;
using System.Collections;

public class PuzzleWatcher : MonoBehaviour {

	public delegate void Watcher();
	Watcher watcher;

	public int solveCount { get; set; } 
	public int puzzleCount { get; set; }
	public int puzzleIndex { get; set; }
	public int lives = 4, bites = 0, score = 0;
	public float TimeRemaining;
	public bool end = false, isOver = false, isHighlighting = false;
	public int levelIndex;

	public GameObject VictoryScreen, DefeatScreen, GameGUIScreen, LevelSplitter;

	private GameGUI gameGUI;
	private PlayerData playerData;
	private PuzzleParser puzzleParser;
	public Rect OverRect;
	public LevelSplitter levelSplitter;

	void Awake()
	{
		solveCount = 0;

		OverRect = new Rect(Screen.width*0.2f,Screen.height*0.1f, Screen.width*0.6f, Screen.height*0.8f);

		gameGUI = GameObject.Find ("GameGUI").GetComponent<GameGUI>();
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
		puzzleParser = GameObject.Find("PuzzleGenerator").GetComponent<PuzzleParser>();
		levelSplitter = LevelSplitter.GetComponent<LevelSplitter>();
		TimeRemaining = puzzleParser.currentPuzzle.rows.Count * 
			puzzleParser.currentPuzzle.rows[0].cells.Count * 12;

		watcher += victory;
		watcher += defeat;
		watcher += countdown;
		watcher += overDetect;
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
				" : " +
					Mathf.FloorToInt(Mathf.Repeat (TimeRemaining, 60)).ToString("D2")
						: "0 : 00";

		gameGUI.timer.content.text = printTime;
	}

	void victory()
	{
		if (solveCount == puzzleCount && puzzleCount > 0 && (lives > 0 || TimeRemaining > 0)
		    ||
		    Input.GetKeyDown(KeyCode.Q))
			{
				if (levelSplitter.checkMe){
					LevelSplitter.SetActive(true);
					LevelSplitter ls = LevelSplitter.GetComponent<LevelSplitter>();
					ls.minimap = ls.splitLevel.printTextures();
				}else{
					recordVictory();
				}
			}
	}

	public void recordVictory()
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

	void defeat()
	{
		if (lives <= 0 || TimeRemaining <= 0)
		{
			stop ();
			DefeatScreen.SetActive(true);
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
		levelIndex = playerData.CurrentLevel;
	}

	void empty () {}
}
