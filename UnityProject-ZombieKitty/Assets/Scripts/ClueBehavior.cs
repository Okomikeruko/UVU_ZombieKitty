using UnityEngine;
using System.Collections;

public enum ClueType {
	column,
	row};

public class ClueBehavior : MonoBehaviour {

	public Material[] clueMaterials;
	public int clueValue, clueGroup;
	public GameObject crossOut;
	public ClueType clueType;
	public bool autoCrossOut = false;

	private GameGUI gameGUI;
	private PuzzleWatcher pw;
	
	void Start () {
		pw = GameObject.Find ("PuzzleBuilder").GetComponent<PuzzleWatcher>();
		gameGUI = GameObject.Find ("GameGUI").GetComponent<GameGUI>(); 
		this.renderer.material = clueMaterials[clueValue];
	}

	void OnMouseDown()
	{
		if(Input.GetMouseButton(0) && !gameGUI.paused && !pw.end && !autoCrossOut && pw.isOver){
			crossOut.SetActive(!crossOut.activeSelf);
		}
	}

	public void clearClues(bool makeClear) {
		if (makeClear){
			this.renderer.material.color = crossOut.renderer.material.color = Color.clear;
		}else{
			this.renderer.material.color = crossOut.renderer.material.color = Color.white;
		}
	}

	public void SetAutoCrossOut()
	{
		autoCrossOut = true;
		crossOut.SetActive (true);
	}

}
