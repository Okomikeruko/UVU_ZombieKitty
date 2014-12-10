using UnityEngine;
using System.Collections;

public class BoxBehaviour : MonoBehaviour {
	
	public delegate void myDelegate();
	myDelegate clickEvent, watch;

	public Material openBox;
	public Color openColor;
	public bool kitty, isOpen = false;
	public string state = "hand";
	public int mode = 1, kittyScore = 50, zombieScore = 50;
	public GameObject highlightPlane;

	private PuzzleWatcher pw;
	private GameGUI gameGUI;

	void Start()
	{
		pw = GameObject.Find ("PuzzleBuilder").GetComponent<PuzzleWatcher>();
		gameGUI = GameObject.Find ("GameGUI").GetComponent<GameGUI>();
		switch (mode)
		{
		case 1:
			clickEvent = direct;
			watch = directWatch;
			break;
		case 0:
			clickEvent = highlight;
			watch = empty;
			break;
		default:
			break;
		}
	}

	void Update()
	{
		watch();
	}

	void OnMouseDown()
	{
		pw.isHighlighting = !highlightPlane.activeSelf;
	}

	void OnMouseOver()
	{
		if(Input.GetMouseButton(0) && !gameGUI.paused && !pw.end && pw.isOver)
			clickEvent();
	}

	void direct()
	{
		highlightEvent(state);
	}

	public void highlightEvent(string s)
	{
		switch (s)
		{
		case "hand":
			if (kitty)
			{
				pw.score += kittyScore;
			}
			else
			{
				pw.bites++;
				pw.TimeRemaining -= 30 * pw.bites;
			}
			break;
		case "shotgun":
			if (kitty)
			{
				pw.lives--;
			}
			else
			{
				pw.score += zombieScore;
			}
			break;
		default:
			break;
		}

		open ();
	}

	public void open()
	{
		isOpen = true;
		this.renderer.material = openBox;
		this.renderer.material.color = openColor;
		pw.solveCount++;
		clickEvent = empty;
	}

	void directWatch()
	{
		state = (gameGUI.BasketMode) ? "hand" : "shotgun";
		if(pw.end){
			clickEvent = empty;
		}
	}

	void highlight()
	{
		highlightPlane.SetActive(pw.isHighlighting);
	}

	void empty (){}
}
