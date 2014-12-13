using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class PuzzleBuilder : MonoBehaviour {

	public Puzzle puzzle;
	public GameObject Cell, Clue, Grid, ClueGrid;
	private PuzzleWatcher pw;
	private PlayerData playerData;

	public void BuildPuzzlePart () {
		pw = this.gameObject.GetComponent<PuzzleWatcher>();
		puzzle = GameObject.Find("PuzzleGenerator").GetComponent<PuzzleParser>().currentPuzzle;
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
		BuildPuzzle(puzzle);
		CenterCamera();
		pw.puzzleIndex = puzzle.puzzlenum - 1;
	}

	public void BuildPuzzlePart(Puzzle p, Puzzle original)
	{
		pw = this.gameObject.GetComponent<PuzzleWatcher>();
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
		puzzle = p;
		BuildPuzzle(p);
		CenterCamera();
		pw.puzzleIndex = original.puzzlenum - 1; 
	}

	public void BuildPuzzlePart(Section p, Puzzle original)
	{
		pw = this.gameObject.GetComponent<PuzzleWatcher>();
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
		puzzle = p.puzzle;
		BuildPuzzle(p);
		CenterCamera();
		pw.puzzleIndex = original.puzzlenum - 1;
	}

	void BuildPuzzle(Section s){
		int i = s.puzzle.rows.Count;
		foreach(Row row in s.puzzle.rows)
		{
			int j = 0;
			foreach(Cell cell in row.cells.Reverse<Cell>())
			{
				MakeCell(cell, j, i, s.cells[s.puzzle.rows.Count - i][(row.cells.Count - 1) + j]);
				j--;
			}
			foreach(int clue in s.puzzle.rowClues[s.puzzle.rows.Count - i].Reverse<int>())
			{
				MakeClue(clue, j, i, ClueType.row, i);
				j--;
			}
			i--;
		}
		int k = 0;
		foreach (List<int> clues in s.puzzle.colClues.Reverse<List<int>>())
		{
			int l = 1;
			foreach (int clue in clues.Reverse<int>())
			{
				MakeClue (clue, k, l + s.puzzle.rows.Count, ClueType.column, k);
				l++;
			}
			k--;
		}
		Vector2 gridCount = new Vector2(s.puzzle.rows[0].cells.Count, s.puzzle.rows.Count);
		gridCount /= 5;
		
		for (int m = 0; m < gridCount.y; m++)
		{
			for (int n = 0; n > -gridCount.x; n--)
			{
				MakeGrid ((n*5)-2, (m*5)+3, gridCount, new Vector2(Mathf.Abs (n), m));
			}
		}
		
		for (int o = 0; o < gridCount.y; o++)
		{
			MakeClueGrid (0, o, (o*5)+3);
		}
		for (int o = 0; o > -gridCount.x; o--)
		{
			MakeClueGrid (1, o, (o*5)-2);
		}
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
				MakeClue(clue, j, i, ClueType.row, i);
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
				MakeClue (clue, k, l + p.rows.Count, ClueType.column, k);
				l++;
			}
			k--;
		}
		Vector2 gridCount = new Vector2(p.rows[0].cells.Count, p.rows.Count);
		gridCount /= 5;

		for (int m = 0; m < gridCount.y; m++)
		{
			for (int n = 0; n > -gridCount.x; n--)
			{
				MakeGrid ((n*5)-2, (m*5)+3, gridCount, new Vector2(Mathf.Abs (n), m));
			}
		}

		for (int o = 0; o < gridCount.y; o++)
		{
			MakeClueGrid (0, o, (o*5)+3);
		}
		for (int o = 0; o > -gridCount.x; o--)
		{
			MakeClueGrid (1, o, (o*5)-2);
		}
	}

	void MakeCell(Cell cell, int x, int y)
	{
		GameObject c = Instantiate(Cell, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
		BoxBehaviour b = c.GetComponent<BoxBehaviour>();
		b.openColor = cell.getColor();
		b.kitty = cell.isHealthy();
		b.mode = playerData.CurrentPlayer.settings.playmode;
		pw.puzzleCount++;
	}

	void MakeCell(Cell cell, int x, int y, SplitCell s)
	{
		GameObject c = Instantiate(Cell, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
		BoxBehaviour b = c.GetComponent<BoxBehaviour>();
		b.openColor = cell.getColor();
		b.kitty = cell.isHealthy();
		b.splitCell = s;
		if(s.solved) b.openOnStart();
		b.mode = playerData.CurrentPlayer.settings.playmode;
		pw.puzzleCount++;
	}

	void MakeClue(int value, int x, int y, ClueType t, int group)
	{
		GameObject c = Instantiate (Clue, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
		ClueBehavior b = c.GetComponent<ClueBehavior>();
		b.clueValue = value;
		b.clueType = t;
		b.clueGroup = group;
	}

	void MakeGrid (int x, int y, Vector2 gridCounter, Vector2 gridIndex)
	{
		GameObject g = Instantiate (Grid, new Vector3(x,y,0), Quaternion.identity) as GameObject;
		foreach (GameObject c in GameObject.FindGameObjectsWithTag("Cell")){
			if (Mathf.Abs (c.transform.position.x - g.transform.position.x) < 3 && Mathf.Abs (c.transform.position.y - g.transform.position.y) < 3){
				c.transform.parent = g.transform;
			}
		}

		GridBehavior gridBehavior = g.GetComponent<GridBehavior>();
		gridBehavior.gridCounter = gridCounter; 
		gridBehavior.gridIndex = gridIndex;
		gridBehavior.setGridsActive(true);
	}

	void MakeClueGrid(int type, int index, int pos)
	{
		GameObject g = Instantiate(ClueGrid, Vector3.zero, Quaternion.identity) as GameObject;
		ClueGridBehavior cg = g.GetComponent<ClueGridBehavior>();
		cg.type = type;
		cg.index = index;
		cg.pos = pos;

		switch (type) {
		case 0:
			g.transform.position = new Vector3 (-2, pos, 0);
			foreach(GameObject c in GameObject.FindGameObjectsWithTag ("Clue")) {
				if (Mathf.Abs(c.transform.position.y - pos) < 3) {
					c.transform.parent = g.transform;
				}
			}
			break;
		case 1:
			g.transform.position = new Vector3 (pos, 3, 0);
			foreach(GameObject c in GameObject.FindGameObjectsWithTag ("Clue")) {
				if (Mathf.Abs(c.transform.position.x - pos) < 3) {
					c.transform.parent = g.transform;
				}
			}
			break;
		default:
			break;
		}
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
