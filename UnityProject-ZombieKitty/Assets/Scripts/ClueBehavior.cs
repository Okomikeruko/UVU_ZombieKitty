using UnityEngine;
using System.Collections;

public class ClueBehavior : MonoBehaviour {

	public Material[] clueMaterials;
	public int clueValue;
//	public Color clueColor;

	void Start () {
		this.renderer.material = clueMaterials[clueValue];
//		this.renderer.material.color = clueColor;
	}
}
