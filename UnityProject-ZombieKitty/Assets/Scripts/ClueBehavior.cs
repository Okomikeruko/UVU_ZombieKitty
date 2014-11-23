using UnityEngine;
using System.Collections;

public class ClueBehavior : MonoBehaviour {

	public Material[] clueMaterials;
	public int clueValue;

	void Start () {
		this.renderer.material = clueMaterials[clueValue];
	}

	public void clearClues(bool makeClear) {
		if (makeClear){
			this.renderer.material.color = Color.clear;
		}else{
			this.renderer.material.color = Color.white;
		}
	}

}
