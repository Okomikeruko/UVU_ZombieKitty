using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

public class PuzzleParser : MonoBehaviour {
	
	public Library allPuzzles;
	private string Data, _Location;
	[SerializeField]
	private string Filename;

	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		_Location = Application.dataPath + "\\XML";
		LoadPuzzles();
		if(Data.ToString() != "")
		{
			allPuzzles = (Library)DeserializeObject(Data);
			foreach(Level level in allPuzzles.levels)
			{
				foreach(Puzzle puzzle in level.puzzles)
				{
					puzzle.rowClues = puzzle.getClues ("row");
					puzzle.colClues = puzzle.getClues ("column");
				}
			}
			Application.LoadLevel("Menu");
		}
	}

	byte[] StringToUTF8ByteArray(string pXmlString)
	{
		UTF8Encoding encoding = new UTF8Encoding();
		byte[] byteArray = encoding.GetBytes(pXmlString);
		return byteArray;
	}

	object DeserializeObject (string pXmlizedString)
	{
		XmlSerializer xs = new XmlSerializer(typeof(Library));
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
		return xs.Deserialize(memoryStream);
	}

	void LoadPuzzles()
	{
		StreamReader r = File.OpenText(_Location + "\\" + Filename);
		string info = r.ReadToEnd();
		r.Close();
		Data = info;
	}
}

[XmlRoot("root")]
public class Library
{
	[XmlElement("level")]
	public List<Level> levels { get; set; }

	public Library()
	{
		levels = new List<Level>();
	}
}

public class Level
{
	[XmlAttribute("levelnum")]
	public int levelnum { get; set; }

	[XmlElement("puzzle")]
	public List<Puzzle> puzzles { get; set; }

	public bool unlocked { get; set; }

	public Level()
	{
		unlocked = false;
		puzzles = new List<Puzzle>();
	}
}

public class Puzzle
{
	[XmlAttribute("name")]
	public string name { get; set; }

	[XmlAttribute("puzzlenum")]
	public int puzzlenum { get; set; }

	[XmlElement("row")]
	public List<Row> rows { get; set; }

	public bool unlocked { get; set; }
	public List<List<int>> rowClues, colClues;

	public Puzzle()
	{
		unlocked = false;
		rows = new List<Row>();
	}

	public int getLongest (List<List<int>> list)
	{
		return list.OrderBy(x => x.Count).Last ().Count;
	}

	public List<List<int>> getClues(string type)
	{
		List<List<int>> clues = new List<List<int>>();
		switch(type)
		{
		case "row":
			foreach (Row row in rows)
			{
				List<int> clue = new List<int>();
				clue.Add (0);
				int c = 0;
				foreach (Cell cell in row.cells)
				{
					if(cell.isHealthy())
					{
						clue[c]++;
					}
					else if(clue[c] > 0)
					{
						clue.Add (0);
						c++;
					}
				}
				if (clue[c] == 0 && c > 0)
				{
					clue.RemoveAt(c);
				}
				clues.Add (clue);
			}
			break;
		case "column":
			for (int col = 0; col < rows[0].cells.Count; col++)
			{
				List<int> clue = new List<int>();
				clue.Add (0);
				int c = 0;
				for (int row = 0; row < rows.Count; row++)
				{
					if(rows[row].cells[col].isHealthy())
					{
						clue[c]++;
					}
					else if(clue[c] > 0)
					{
						clue.Add (0);
						c++;
					}
				}
				if (clue[c] == 0 && c > 0)
				{
					clue.RemoveAt(c);
				}
				clues.Add (clue);
			}
			break;
		default:
			break;
		}
		return clues;
	}
}

public class Row
{
	[XmlElement("cell")]
	public List<Cell> cells { get; set; }

	public Row()
	{
		cells = new List<Cell>();
	}
}

public class Cell
{
	[XmlAttribute("color")]
	public string color { get; set; }
	
	[XmlText]
	public string health { get; set; }

	public Cell()
	{
		color = "";
	}

	public bool isHealthy()
	{
		return (health == "true") ? true : false;
	}

	public Color getColor()
	{
		if(color.Contains ("rgb"))
		{
			MatchCollection colors = Regex.Matches (color, "[0-9]+");
			float r = float.Parse(colors[0].Value);
			float g = float.Parse(colors[1].Value);
			float b = float.Parse(colors[2].Value);
			Color c = new Color(r/255.0F,g/255.0F,b/255.0F,1);
			return c;
		}
		else
		{
			return Color.clear;
		}
	}
}