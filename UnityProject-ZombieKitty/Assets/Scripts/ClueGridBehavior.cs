using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClueGridBehavior : MonoBehaviour {

	public int index, pos, type;
	public GameObject grid;
	
	// Use this for initialization
	void Start () {
		switch(type)
		{
		case 0:
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("Grid")) {
				if (g.GetComponent<GridBehavior>().gridIndex == new Vector2(0,index))
				{
					grid = g;
				}
			}
			break;
		case 1:
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("Grid")) {
				if (g.GetComponent<GridBehavior>().gridIndex == new Vector2(Mathf.Abs(index),0))
				{
					grid = g;
				}
			}
			break;
		default:
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (type)
		{
		case 0:
			transform.position = new Vector3 (transform.position.x, grid.transform.position.y, transform.position.z);
			transform.localScale = new Vector3 (transform.localScale.x, grid.transform.localScale.y/5, transform.localScale.z);
			break;
		case 1:
			transform.position = new Vector3 (grid.transform.position.x, transform.position.y, transform.position.z);
			transform.localScale = new Vector3 (grid.transform.localScale.x/5, transform.localScale.y, transform.localScale.z);
			break;
		default:
			break;
		}
	}
}
