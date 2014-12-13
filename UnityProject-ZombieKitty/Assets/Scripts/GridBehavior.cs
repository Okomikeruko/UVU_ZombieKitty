using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridBehavior : MonoBehaviour {
	
	delegate void Watcher();
	Watcher watcher;

	public List<GameObject> gridParts;
	private PuzzleWatcher pw;
	private GameGUI gameGUI;
	private Vector3 newPos, newScale, originalPos, originalScale;
	public Vector2 gridCounter, gridIndex, growIndex;
	public float animationSpeed = 15;

	// Use this for initialization
	void Start () {
		gameGUI = GameObject.Find ("GameGUI").GetComponent<GameGUI>();
		pw = GameObject.Find ("PuzzleBuilder").GetComponent<PuzzleWatcher>();
		gridParts = new List<GameObject>();
		gridParts.AddRange(GameObject.FindGameObjectsWithTag("Grid"));
		watcher = empty;
		originalPos = transform.position;
		originalScale = transform.localScale;
		setGridsActive(GameObject.Find ("PlayerData").GetComponent<PlayerData>().CurrentLevel > 2);
	}
	
	// Update is called once per frame
	void Update () {
		watcher();
	}

	void OnMouseDown() {
		if (pw.OverRect.Contains(Input.mousePosition) && gameGUI.canZoom){
			gameGUI.zoomedIn = true;
			foreach (GameObject gridPart in gridParts)
			{
				GridBehavior g = gridPart.GetComponent<GridBehavior>();
				if (g != null){
					g.growIndex = gridIndex;
					g.setGridsActive(true);
					g.squish ();
				}
			}
		}
	}

	public void setGridsActive(bool a) {
		if(a){
			foreach(Transform child in transform) {
				Vector3 pos = child.position;
				pos.z = 1;
				child.position = pos;
			}
		} else {
			foreach(Transform child in transform){
				Vector3 pos = child.position;
				pos.z = -1;
				child.position = pos;
			}
		}
	}

	public void ZoomOut()
	{
		watcher = restore;
	}

	void restore()	{
		if (transform.localScale != originalScale && transform.position != originalPos) {
			transform.localScale = Vector3.Lerp (transform.localScale, originalScale, animationSpeed * Time.deltaTime);
			transform.position = Vector3.Lerp (transform.position, originalPos, animationSpeed * Time.deltaTime);
		} else {
			watcher = empty;
		}
	}

	public void squish()
	{
		float xScale, yScale, xPos, yPos;

		xScale = (growIndex.x == gridIndex.x) ? 1 + (4 * gridCounter.x) : 1 ;
		yScale = (growIndex.y == gridIndex.y) ? 1 + (4 * gridCounter.y) : 1 ;

		xPos = originalPos.x + squashOffset (Mathf.RoundToInt (growIndex.x), 
		                                     Mathf.RoundToInt (gridIndex.x), 
		                                     Mathf.RoundToInt (gridCounter.x));
		yPos = originalPos.y + squashOffset (Mathf.RoundToInt (growIndex.y), 
		                                     Mathf.RoundToInt (gridIndex.y), 
		                                     Mathf.RoundToInt (gridCounter.y));

		newScale = new Vector3(xScale, yScale, 1);
		newPos = new Vector3(xPos, yPos, transform.position.z);
		watcher = squash;
	}

	int squashOffset(int i, int j, int count)
	{
		int[,] fours =  new int[4,4] {
			{-6, -10, -6, -2},
			{ 2,  -2, -6, -2},
			{ 2,   6, -2, -2},
			{ 2,   6, 10, -6}
		}, threes = new int[3,3] {
			{-4, -6, -2},
			{ 2,  0, -2},
			{ 2,  6,  4}
		}, twos = new int[2,2] {
			{ 2,  2},
			{-2, -2}
		};
		int output = 0;
		switch (count)
		{
		case 2:
			output = twos[i,j];
			break;
		case 3:
			output = threes [i,j];
			break;
		case 4:
			output = fours [i,j];
			break;
		default:
			break;
		}
		return output;
	}

	void squash(){

		if (transform.localScale != newScale && transform.position != newPos)
		{
			transform.localScale = Vector3.Lerp (transform.localScale, newScale, animationSpeed * Time.deltaTime);
			transform.position = Vector3.Lerp (transform.position, newPos, animationSpeed * Time.deltaTime);
		}
		else
		{
			if (growIndex == gridIndex)
			{
				setGridsActive (false);
			}
			watcher = empty;
		}
	}

	public void empty(){}
}
