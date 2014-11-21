using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PuzzleBuilder : MonoBehaviour {

	public Puzzle puzzle;
	public GameObject Cell, Clue;

	// Use this for initialization
	void Start () {
		puzzle = GameObject.Find("PuzzleGenerator").GetComponent<PuzzleParser>().currentPuzzle;
		BuildPuzzle(puzzle);
	}
	
	void BuildPuzzle(Puzzle p)
	{
		int i = p.rows.Count;
		foreach(Row row in p.rows)
		{
			int j = 0;
			foreach(Cell cell in row.cells.Reverse<Cell>())
			{
				MakeCell(cell, j, i);
				j--;
			}
			foreach(int clue in p.rowClues[p.rows.Count - i].Reverse<int>())
			{
				MakeClue(clue, j, i);
				j--;
			}
			i--;
		}
		int k = 0;
		foreach (List<int> clues in p.colClues.Reverse<List<int>>())
		{
			int l = 1;
			foreach (int clue in clues.Reverse<int>())
			{
				MakeClue (clue, k, l + p.rows.Count);
				l++;
			}
			k--;
		}

	}

	void MakeCell(Cell cell, int x, int y)
	{
		GameObject c = Instantiate(Cell, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
		BoxBehaviour b = c.GetComponent<BoxBehaviour>();
		b.openColor = cell.getColor();
		b.kitty = cell.isHealthy();
	}

	void MakeClue(int value, int x, int y)
	{
		GameObject c = Instantiate (Clue, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
		ClueBehavior b = c.GetComponent<ClueBehavior>();
		b.clueValue = value;
	}
}
