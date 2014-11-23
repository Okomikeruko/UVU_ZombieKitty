using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AutoCrossOut : MonoBehaviour {

	public delegate void Watch();
	Watch watch;

	private List<List<GameObject>> ColumnClueGroup, RowClueGroup, ColumnCellGroup, RowCellGroup;
	private List<PuzzleLine> puzzleLines;
	private Puzzle puzzle;


	void Start () {
		ColumnClueGroup = new List<List<GameObject>>();
		RowClueGroup = new List<List<GameObject>>();
		ColumnCellGroup = new List<List<GameObject>>();
		RowCellGroup = new List<List<GameObject>>();
		puzzleLines = new List<PuzzleLine>();
		puzzle = GameObject.Find("PuzzleGenerator").GetComponent<PuzzleParser>().currentPuzzle;
		watch = empty;
		watch += createGroups;
	}
	
	void Update () {
		watch();
	}

	void createGroups()
	{
		int colKey = 0 - puzzle.rows[0].cells.Count;
		for (int i = 0; i > colKey; i--){
			ColumnClueGroup.Add (createGroup (ClueType.column, i, "Clue"));
			ColumnCellGroup.Add (createGroup (ClueType.column, i, "Cell"));
		}
		int rowKey = puzzle.rows.Count;
		for (int i = rowKey; i > 0; i--){
			RowClueGroup.Add (createGroup (ClueType.row, i, "Clue"));
			RowCellGroup.Add (createGroup (ClueType.row, i, "Cell"));
		}

		watch -= createGroups;
		watch += watchLines;
	}

	List<GameObject> createGroup (ClueType clueType, int groupNum, string tag)
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
		List<GameObject> output = new List<GameObject>();
		foreach (GameObject obj in objects)
		{
			switch (clueType)
			{
			case ClueType.column:
				if (obj.transform.position.x == groupNum)
				{
					output.Add(obj);
					output = output.OrderBy(x=>x.transform.position.y).ToList();
				}
				break;
			case ClueType.row:
				if (obj.transform.position.y == groupNum)
				{
					output.Add(obj);
					output = output.OrderBy(x=>x.transform.position.x).ToList();
				}
				break;
			default:
				break;
			}
		}
		return output;
	}

	void watchLines()
	{
		createLines ();

		foreach (PuzzleLine line in puzzleLines)
		{
			if ( line.clues.Count == 1 && line.clueValues[0] == 0) crossout (line.clues[0]);

			int openBoxes, clueCount = 0;
			bool last = false;
			for (openBoxes = 0; openBoxes < line.open.Count && line.open[openBoxes]; openBoxes++)
			{
				bool current = line.kitties[openBoxes];
				if (last && !current)
					clueCount++;
				last = current;

				if (openBoxes == line.open.Count - 1)
				{
					clueCount = line.clues.Count;
				}
			}
			for (int i = 0; i < clueCount; i++)
			{
				crossout (line.clues[i]);
			}

			clueCount = line.clues.Count;
			last = false;

			for (openBoxes = line.open.Count; openBoxes > 0 && line.open[openBoxes-1]; openBoxes--)
			{
				bool current = line.kitties[openBoxes-1];
				if (last && !current)
					clueCount--;
				last = current;
			}

			for (int i = line.clues.Count; i > clueCount; i--)
			{
				crossout (line.clues[clueCount]);
			}
		}
	}

	void crossout(GameObject g)
	{
		ClueBehavior c = g.GetComponent<ClueBehavior>();
		if(!c.autoCrossOut) c.SetAutoCrossOut();
	}

	void createLines()
	{

		for (int i = 0; i < ColumnClueGroup.Count; i++)
		{
			puzzleLines.Add (createLine (ColumnClueGroup[i], ColumnCellGroup[i]));
		}
		for (int i = 0; i < RowClueGroup.Count; i++)
		{
			puzzleLines.Add (createLine (RowClueGroup[i], RowCellGroup[i]));
		}
	}

	PuzzleLine createLine(List<GameObject> clues, List<GameObject> cells)
	{
		List<bool> open =new List<bool>();
		List<bool> kitties = new List<bool>();
		List<int> clueValues = new List<int>();
		foreach (GameObject cell in cells)
		{
			BoxBehaviour b = cell.GetComponent<BoxBehaviour>();
			open.Add(b.isOpen);
			kitties.Add(b.kitty);
		}
		foreach (GameObject clue in clues)
		{
			ClueBehavior c = clue.GetComponent<ClueBehavior>();
			clueValues.Add (c.clueValue);
		}

		return new PuzzleLine(open, kitties, clueValues, clues);
	}

	void empty() {}
}

public class PuzzleLine
{
	public List<bool> open, kitties;
	public List<int> clueValues;
	public List<GameObject> clues;

	public PuzzleLine() {
		open = kitties = new List<bool>();
		clueValues = new List<int>();
		clues = new List<GameObject>();
	}

	public PuzzleLine(List<bool> o, List<bool> k, List<int> v, List<GameObject> c)
	{
		open = kitties = new List<bool>();
		clueValues = new List<int>();
		clues = new List<GameObject>();

		open = o;
		kitties = k;
		clueValues = v;
		clues = c;
	}
}