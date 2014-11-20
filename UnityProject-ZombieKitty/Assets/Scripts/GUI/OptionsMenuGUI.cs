using UnityEngine;
using System.Collections;

public class OptionsMenuGUI : MonoBehaviour {
	
	[SerializeField]
	public ButtonClass MainMenu;
	private GameSettings settings;
	private string[] modes = new string[] {"Highlight", "Direct"};

	void OnEnable() {
		settings = GameObject.Find ("PlayerData").
			GetComponent<PlayerData>().
				CurrentPlayer.
				settings;
	}

	void OnGUI() {
		if(GUI.Button (MainMenu.rect, MainMenu.title))
		{
			GameObject.Find ("PlayerData").GetComponent<PlayerData>().SaveData();
			MenuController.ChangeMenu(MainMenu.menuObject, this.gameObject);
		}

		settings.musicVolume = GUI.HorizontalSlider(new Rect (25,25,150,30), settings.musicVolume, 0.0f, 1.0f);
		GUI.Label(new Rect(25, 50, 150, 30), "Music Volume: " + Mathf.RoundToInt(settings.musicVolume * 100));
		settings.sfxVolume = GUI.HorizontalSlider(new Rect (25,75,150,30), settings.sfxVolume, 0.0f, 1.0f);
		GUI.Label(new Rect(25, 100, 150, 30), "SFX Volume: " + Mathf.RoundToInt(settings.sfxVolume * 100));
		settings.playmode = GUILayout.SelectionGrid(settings.playmode, modes, modes.Length);
	}
}
