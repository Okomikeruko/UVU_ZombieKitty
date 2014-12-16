using UnityEngine;
using System.Collections;

public class BoxBehaviour : MonoBehaviour {
	
	public delegate void myDelegate();
	myDelegate clickEvent, watch;

	public Material openBox;
	public Color openColor;
	public SplitCell splitCell;
	public bool kitty, isOpen = false;
	public string state = "hand";
	public int mode = 1, kittyScore = 50, zombieScore = 50;
	public GameObject highlightPlane;
	public AudioClip KittyHappy, KittyShot, ZombieBite, ZombieShot;
	public ParticleSystem KittyHappyFX, KittyShotFX, ZombieBiteFX, ZombieShotFX; 

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
				audio.clip = KittyHappy;
//				KittyHappyFX.Play ();
				pw.score += kittyScore;
			}
			else
			{
				audio.clip = ZombieBite;
//				ZombieBiteFX.Play ();
				pw.bites++;
				pw.TimeRemaining -= 30 * pw.bites;
			}
			break;
		case "shotgun":
			if (kitty)
			{
				audio.clip = KittyShot;
//				KittyShotFX();
				pw.lives--;
			}
			else
			{
				audio.clip = ZombieShot;
//				ZombieShotFX.Play();
				pw.score += zombieScore;
			}
			break;
		default:
			break;
		}
		audio.Play();
		open ();
	}

	public void open()
	{
		isOpen = true;
		if(splitCell != null)
			splitCell.solved = true;
		this.renderer.material = openBox;
		this.renderer.material.color = openColor;
		pw.solveCount++;
		clickEvent = empty;
	}

	public void openOnStart()
	{
		isOpen = true;
		this.renderer.material = openBox;
		this.renderer.material.color = openColor;
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
