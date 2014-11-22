using UnityEngine;
using System.Collections;

public class BoxBehaviour : MonoBehaviour {
	
	public delegate void myDelegate();
	myDelegate clickEvent, watch;

	public Material openBox;
	public Color openColor;
	public bool kitty;
	public string state = "hand";
	public int mode = 1;

	private PuzzleWatcher pw;

	void Start()
	{
		pw = GameObject.Find ("PuzzleBuilder").GetComponent<PuzzleWatcher>();

		switch (mode)
		{
		case 1:
			clickEvent = direct;
			break;
		case 0:
			clickEvent = highlight;
			break;
		default:
			break;
		}
		watch = empty;
	}

	void Update()
	{
		watch();
	}

	void OnMouseOver()
	{
		if(Input.GetMouseButton(0))
			clickEvent();
	}

	void direct()
	{
		switch (state)
		{
		case "hand":
			// Hand events
			break;
		case "shotgun":
			// Shotgun events
			break;
		default:
			break;
		}

		this.renderer.material = openBox;
		this.renderer.material.color = openColor;
		pw.solveCount++;
		clickEvent = empty;
	}
	void highlight()
	{
	
	}

	void empty (){}
}
