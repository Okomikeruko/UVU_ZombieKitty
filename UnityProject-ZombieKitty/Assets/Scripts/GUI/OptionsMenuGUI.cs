using UnityEngine;
using System.Collections;

public class OptionsMenuGUI : MonoBehaviour {
	
	[SerializeField]
	public ButtonClass MainMenu;
	private GameSettings settings;
	private string[] modes = new string[] {"Highlight", "Direct"};

	void Start() {
		settings = GameObject.Find ("PlayerData").
			GetComponent<PlayerData>().
				CurrentPlayer.
				settings;
	}

	void OnGUI() {
		settings = GameObject.Find ("PlayerData").
			GetComponent<PlayerData>().
				CurrentPlayer.
				settings;

		if(GUI.Button (MainMenu.rect, MainMenu.title))
		{
			GameObject.Find ("PlayerData").GetComponent<PlayerData>().SaveData();
			MenuController.ChangeMenu(MainMenu.menuObject, this.gameObject);
		}

		settings.musicVolume = GUI.HorizontalSlider(new Rect (25,25,100,30), settings.musicVolume, 0.0f, 1.0f);
		GUI.Label(new Rect(25, 50, 100, 30), "Music Volume");
		settings.sfxVolume = GUI.HorizontalSlider(new Rect (25,75,100,30), settings.sfxVolume, 0.0f, 1.0f);
		GUI.Label(new Rect(25, 100, 100, 30), "SFX Volume");
		settings.playmode = GUILayout.SelectionGrid(settings.playmode, modes, modes.Length);
	}
}
