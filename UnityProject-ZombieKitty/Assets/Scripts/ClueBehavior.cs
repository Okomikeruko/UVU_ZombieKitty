using UnityEngine;
using System.Collections;

public class ClueBehavior : MonoBehaviour {

	public Material[] clueMaterials;
	public int clueValue;

	private GameGUI gameGUI;

	void Start () {
		this.renderer.material = clueMaterials[clueValue];
		gameGUI = GameObject.Find ("GameGUI").GetComponent<GameGUI>();
	}

	public void clearClues(bool makeClear) {
		if (makeClear){
			this.renderer.material.color = Color.clear;
		}else{
			this.renderer.material.color = Color.white;
		}
	}

}
