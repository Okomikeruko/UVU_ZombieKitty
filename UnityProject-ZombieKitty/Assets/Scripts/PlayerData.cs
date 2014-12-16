using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

public class PlayerData : MonoBehaviour {

	public Players playerData;
	public Library allPuzzles;
	public Player CurrentPlayer;
	public int CurrentLevel;
	private string Data, _Location;
	[SerializeField]
	private string Filename;
	
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		_Location = Application.dataPath + "\\XML";
		LoadData();
		if(Data.ToString() != "") {
			playerData = (Players)DeserializeObject(Data);
			foreach (Player p in playerData.players) {
				if(p.isCurrent) {
					CurrentPlayer = p;
					CurrentLevel = getCurrentLevel(p);
				}
			}
		}
		Application.LoadLevel("Menu");
	}

	void OnLevelWasLoaded(int level){
		if (level > 1){
			foreach(GameObject music in GameObject.FindGameObjectsWithTag("Music")){
				music.audio.volume *= CurrentPlayer.settings.musicVolume;
			}

			foreach(GameObject soundFX in GameObject.FindGameObjectsWithTag("SFX")){
				soundFX.audio.volume *= CurrentPlayer.settings.sfxVolume;
			}
		}
	}

	int getCurrentLevel(Player p) {
		foreach (LevelProgress l in p.progress.Level){
			foreach(PuzzleData puzzle in l.puzzle){
				if (puzzle.puzzleRuns.Count == 0){
					return l.levelNum - 1;
				}
			}
		}
		return 8;
	}

	byte[] StringToUTF8ByteArray(string pXmlString)
	{
		UTF8Encoding encoding = new UTF8Encoding();
		byte[] byteArray = encoding.GetBytes(pXmlString);
		return byteArray;
	}

	string UTF8ByteArrayToString( byte[] characters )
	{
		UTF8Encoding encoding = new UTF8Encoding();
		string constructedString = encoding.GetString (characters);
		return constructedString;
	}
	
	object DeserializeObject (string pXmlizedString)
	{
		XmlSerializer xs = new XmlSerializer(typeof(Players));
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
		return xs.Deserialize(memoryStream);
	}

	string SerializeObject (object pObject)
	{
		string XmlizedString = null;
		MemoryStream memoryStream = new MemoryStream();
		XmlSerializer xs = new XmlSerializer(typeof(Players));
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xs.Serialize (xmlTextWriter, pObject);
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
		return XmlizedString;
	}

	public void LoadData()
	{
		StreamReader r = File.OpenText(_Location + "\\" + Filename);
		string info = r.ReadToEnd();
		r.Close();
		Data = info;
	}

	public void SaveData()
	{
		string saveData = SerializeObject(playerData);
		StreamWriter writer;
		FileInfo t = new FileInfo(_Location + "\\" + Filename);
		if(!t.Exists)
		{
			writer = t.CreateText();
		}
		else
		{
			t.Delete ();
			writer = t.CreateText ();
		}
		writer.Write (saveData);
		writer.Close ();
	}

	public void ResetLevels()
	{
		CurrentPlayer.progress = new Progress("new");
		SaveData();
	}
}

[XmlRoot("root")]
public class Players
{
	[XmlElement("player")]
	public List<Player> players { get; set; }

	public Players()
	{
		players = new List<Player>();
	}

	public void setCurrentPlayer(string playerName)
	{
		foreach (Player player in players)
		{
			player.isCurrent = (player.name == playerName);
		}

	}
}

public class Player
{
	[XmlElement("name")]
	public string name { get; set; } 

	[XmlAttribute("current")]
	public bool isCurrent { get; set; }

	[XmlElement("progress")]
	public Progress progress { get; set; }

	[XmlElement("settings")]
	public GameSettings settings { get; set; }

	public Player() {}

	public Player(string newName)
	{
		name = newName;
		progress = new Progress("new");
		progress.Level.Add(new LevelProgress());
		settings = new GameSettings();
	}
}

public class Progress
{
	[XmlElement("level")]
	public List<LevelProgress> Level { get; set; }

	public Progress ()
	{
		Level = new List<LevelProgress>();
	}

	public Progress(string n)
	{
		Level = new List<LevelProgress>();
		foreach (Level level in GameObject.Find ("PuzzleGenerator").GetComponent<PuzzleParser>().allPuzzles.levels)
		{
			Level.Add (new LevelProgress(level));
		}
	}
}

public class LevelProgress
{
	[XmlElement("puzzle")]
	public List<PuzzleData> puzzle { get; set; }

	[XmlElement("levelNum")]
	public int levelNum;

	public LevelProgress()
	{
		puzzle = new List<PuzzleData>(); 
	}
	public LevelProgress(Level l)
	{
		levelNum = l.levelnum;
		puzzle = new List<PuzzleData>();
		foreach(Puzzle p in l.puzzles){
			puzzle.Add (new PuzzleData(p));
		}
	}
}

public class PuzzleData
{
	[XmlElement("puzzleName")]
	public string puzzleName { get; set; }

	[XmlElement("puzzleNum")]
	public int puzzleNum { get; set; }

	[XmlElement("puzzleRuns")]
	public List<PuzzleRun> puzzleRuns { get; set; }

	public PuzzleData()
	{
		puzzleRuns = new List<PuzzleRun>(); 
	}
	public PuzzleData(Puzzle p)
	{
		puzzleName = p.name;
		puzzleNum = p.puzzlenum; 
		puzzleRuns = new List<PuzzleRun>();
	}
}

public class PuzzleRun
{
	[XmlElement("highscore")]
	public int highscore { get; set; }
	
	[XmlElement("besttime")]
	public int besttime { get; set; }
	
	[XmlElement("lives")]
	public int lives { get; set; }
	
	public PuzzleRun()
	{
		highscore = lives = besttime = 0;
	}
}

public class GameSettings
{
	[XmlElement("music")]
	public float musicVolume { get; set; }

	[XmlElement("sfx")]
	public float sfxVolume { get; set; }

	[XmlElement("scopeState")]
	public string scopeState { get; set; }

	[XmlElement("toolState")]
	public string toolState { get; set; }

	[XmlElement("playmode")]
	public int playmode { get; set; }

	public GameSettings()
	{
		musicVolume = sfxVolume = 0.5F;
		playmode = 0;
	}
}