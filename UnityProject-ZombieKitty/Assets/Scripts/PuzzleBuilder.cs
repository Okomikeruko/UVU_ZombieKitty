using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PuzzleBuilder : MonoBehaviour {

	public Puzzle puzzle;
	public GameObject Cell, Clue;
	private PuzzleWatcher pw;

	// Use this for initialization
	void Start () {
		pw = this.gameObject.GetComponent<PuzzleWatcher>();
		puzzle = GameObject.Find("PuzzleGenerator").GetComponent<PuzzleParser>().currentPuzzle;
		BuildPuzzle(puzzle);
		CenterCamera();
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
		pw.puzzleCount++;
	}

	void MakeClue(int value, int x, int y)
	{
		GameObject c = Instantiate (Clue, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
		ClueBehavior b = c.GetComponent<ClueBehavior>();
		b.clueValue = value;
	}

	void CenterCamera()
	{
		float w = puzzle.getLongest (puzzle.rowClues) + puzzle.rows[0].cells.Count;
		float h = puzzle.getLongest (puzzle.colClues) + puzzle.rows.Count;
		Vector3 pos = new Vector3((-w/2.0F)+0.5F, (h/2.0F)+0.5F, -10);
		GameObject camera = GameObject.Find ("Main Camera");
		camera.transform.position = pos;
		float l = (w > h) ? w : h;
		camera.camera.orthographicSize = (l * 0.6F);
	}
}
