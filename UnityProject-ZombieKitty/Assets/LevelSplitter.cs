using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSplitter : MonoBehaviour {

	private delegate void Watcher();
	Watcher watcher;


	public ButtonClass exit, easy, hard, grid;
	public bool difficultySet = false, 
				isEasy = true, 
				canClose = false,
				checkMe = false ;
	public int sectionSelect;

	public SplitLevel splitLevel;
	public Puzzle currentPuzzle;
	public Texture2D[] minimap;

	void Awake() {
		checkMe = false;
		watcher = empty;
		sectionSelect = -1;
		currentPuzzle = GameObject.Find ("PuzzleGenerator").GetComponent<PuzzleParser>().currentPuzzle;
		if (currentPuzzle.rows.Count < 20)
		{
			GameObject.Find ("PuzzleBuilder").GetComponent <PuzzleBuilder>().BuildPuzzlePart(currentPuzzle, currentPuzzle);
			this.gameObject.SetActive(false);
		}
		else
		{
			checkMe = true;
			GameObject.Find ("GameGUI").GetComponent<GameGUI>().enabled = false;
			GameObject.Find ("PuzzleBuilder").GetComponent <PuzzleBuilder>().enabled = false;
			GameObject.Find ("PuzzleBuilder").GetComponent <PuzzleWatcher>().enabled = false;
		}
	}

	void SetDifficulty (bool d) {
		if (d) {
			splitLevel = new SplitLevel(currentPuzzle, currentPuzzle.rows.Count / 10);
		} else {
			splitLevel = new SplitLevel(currentPuzzle, currentPuzzle.rows.Count / 15);
		}
		difficultySet = true;
		minimap = splitLevel.printTextures();
		watcher = watching;
	}
	
	void Update () {
		watcher();
	}

	void watching() {
		if(allAreTrue (splitLevel)){
			GameObject.Find ("PuzzleBuilder").GetComponent <PuzzleWatcher>().recordVictory();
			watcher = empty;
			this.gameObject.SetActive(false);
		}
	}

	bool allAreTrue(SplitLevel splitLevel)
	{
		foreach (List<Section> sectionRow in splitLevel.sections){
			foreach (Section section in sectionRow) {
				foreach (List<SplitCell> splitCellRow in section.cells){
					foreach(SplitCell splitCell in splitCellRow){
						if (!splitCell.solved)
						{
							return false;
						}
					}
				}
			}
		}
		return true;
	}

	void OnGUI () {
		if(canClose){
			if(GUI.Button (exit.AnchoredRect(), exit.content, exit.style)) {
				GameObject.Find ("GameGUI").GetComponent<GameGUI>().paused = false;
				Time.timeScale = 1;
				this.gameObject.SetActive(false);
			}
		}
		if(!difficultySet) {
			if (GUI.Button (easy.AnchoredRect(), easy.content, easy.style)) {
				SetDifficulty(true);
			}
			if (GUI.Button (hard.AnchoredRect(), hard.content, hard.style)) {
				SetDifficulty(false);
			}
		}
		else
		{
			sectionSelect = (GUI.SelectionGrid(grid.AnchoredRect(), sectionSelect, minimap, splitLevel.sections.Count, grid.style));
		}
	}

	void OnMouseUp(){
		if (sectionSelect > -1)
		{

			SetSection(sectionSelect);
		}
	}

	void SetSection(int index){
		int c = splitLevel.sections.Count;
		Section section = splitLevel.sections[index/c][index%c];
		GameObject.Find ("GameGUI").GetComponent<GameGUI>().enabled = true;
		GameObject.Find ("GameGUI").GetComponent<GameGUI>().paused = false;
		Time.timeScale = 1;
		GameObject.Find ("PuzzleBuilder").GetComponent <PuzzleWatcher>().enabled = true;
		DestroyPuzzle();
		PuzzleBuilder b = GameObject.Find ("PuzzleBuilder").GetComponent <PuzzleBuilder>();
		b.enabled = true;
		b.BuildPuzzlePart(section, currentPuzzle);
		canClose=true;
		this.gameObject.SetActive(false);
	}

	void DestroyPuzzle()
	{
		string[] tags = new string[] {"Clue", "Cell", "Grid", "ClueGrid"};
		foreach (string tag in tags){
			foreach (GameObject g in GameObject.FindGameObjectsWithTag(tag)){
				GameObject.Destroy (g); 
			}
		}
	}

	void empty () {}
}

public class SplitLevel
{
	public List<List<Section>> sections { get; set; }

	public SplitLevel() {
		sections = new List<List<Section>>();
	}
	public SplitLevel(Puzzle p, int d)
	{
		sections = new List<List<Section>>();
		int length = p.rows.Count / d;
		for (int i = 0; i < d; i++) {
			List<Section> SectionRow = new List<Section>();
			for (int j = 0; j < d; j++) {
				List<List<Cell>> cellMap = new List<List<Cell>>();
				Puzzle subPuzzle = new Puzzle();
				for (int k = i*length; k < (i+1)*length; k++) {
					List<Cell> cellRow = new List<Cell>();
					Row subPuzzleRow = new Row();
					for (int l = j*length; l < (j+1)*length; l++) {
						Cell currentCell = p.rows[k].cells[l];
						cellRow.Add(currentCell);
						subPuzzleRow.cells.Add(currentCell);
					}
					cellMap.Add(cellRow);
					subPuzzle.rows.Add(subPuzzleRow);
				}
				subPuzzle.rowClues = subPuzzle.getClues ("row");
				subPuzzle.colClues = subPuzzle.getClues ("column");
				Section section = new Section(cellMap);
				section.puzzle = subPuzzle;
				SectionRow.Add (section); 
			} 
			sections.Add (SectionRow); 
		}
	} 

	public Texture2D[] printTextures() { 
		int c = 0;
		foreach (List<Section> sectionRow in sections){
			c += sectionRow.Count; 
		}

		Texture2D[] output = new Texture2D[c];
		int i = 0;
		foreach(List<Section> sectionRow in sections) {
			foreach (Section section in sectionRow) {
				section.refreshTexture();
				output[i++] = section.texture;
			}
		}
		return output;
	}
}

public class Section
{
	public List<List<SplitCell>> cells;
	public Puzzle puzzle;
	public Texture2D texture;
	public Section() {
		cells = new List<List<SplitCell>>();
	}
	public Section(List<List<Cell>> cellMap){
		cells = new List<List<SplitCell>>();
		foreach (List<Cell> cellRow in cellMap)	{
			List<SplitCell> splitCellRow = new List<SplitCell>();
			foreach (Cell cell in cellRow) {
				splitCellRow.Add (new SplitCell(false, cell.getColor()));
			}
			cells.Add (splitCellRow);
		}
	}
	public void refreshTexture(){
		texture = new Texture2D (cells.Count*50, cells[0].Count*50);
		Color[] colors = new Color[cells.Count*50 * cells[0].Count*50];
		int h = 0;
		for (int i = cells.Count; i > 0; i--){
			for (int l = 0; l < 50; l++){
				for (int j = 0; j < cells[0].Count; j++){
					for (int k = 0; k < 50; k++){
						SplitCell splitCell = cells[i-1][j];
						colors[h++] = (splitCell.solved) ? splitCell.color : Color.black; 
					}
				}
			}
		}
		texture.SetPixels(colors);
		texture.Apply();
	}
}

public class SplitCell
{
	public bool solved;
	public Color color;

	public SplitCell(){}

	public SplitCell(bool s, Color c) {
		solved = s;
		color = c;
	}
}