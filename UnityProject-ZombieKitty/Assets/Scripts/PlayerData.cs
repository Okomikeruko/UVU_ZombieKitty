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
	public Player CurrentPlayer;
	private string Data, _Location;
	[SerializeField]
	private string Filename;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
		_Location = Application.dataPath + "\\XML";
		LoadData();
		if(Data.ToString() != "")
		{
			playerData = (Players)DeserializeObject(Data);
			foreach (Player p in playerData.players)
			{
				if(p.isCurrent)
				{
					CurrentPlayer = p;
				}
			}
		}
		Application.LoadLevel("Menu");
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
		progress = new Progress();
		settings = new GameSettings();
	}
}

public class Progress
{
	[XmlElement("level")]
	public List<LevelProgress> Level { get; set; }

	public Progress()
	{
		Level = new List<LevelProgress>();
	}
}

public class LevelProgress
{
	[XmlElement("puzzle")]
	public List<PuzzleData> puzzle { get; set; }

	public LevelProgress()
	{
		puzzle = new List<PuzzleData>(); 
	}
}

public class PuzzleData
{
	[XmlAttribute("unlocked")]
	public bool unlocked { get; set; }

	[XmlAttribute("num")]
	public int num { get; set; }

	[XmlElement("highscore")]
	public int highscore { get; set; }

	[XmlElement("besttime")]
	public Time besttime { get; set; }

	[XmlElement("lives")]
	public int lives { get; set; }
}

public class GameSettings
{
	[XmlElement("music")]
	public float musicVolume { get; set; }

	[XmlElement("sfx")]
	public float sfxVolume { get; set;}

	[XmlElement("playmode")]
	public int playmode { get; set; }

	public GameSettings()
	{
		musicVolume = sfxVolume = 0.5F;
		playmode = 0;
	}
}