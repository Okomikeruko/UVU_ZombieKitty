using UnityEngine;
using System.Collections;

public class OptionsMenuGUI : MonoBehaviour {
	
	[SerializeField]
	public ButtonClass MainMenu, SFX, Music, Mode;
	private GameSettings settings;
	public Texture[] modeTextures;
	public GUIStyle sliderBar, sliderThumb;

	void OnEnable() {
		settings = GameObject.Find ("PlayerData").
			GetComponent<PlayerData>().
				CurrentPlayer.
					settings;
	}

	void OnGUI() {
		sliderThumb.overflow.left = sliderThumb.overflow.right = Mathf.RoundToInt(18.0F * (Screen.width / 814.0F));

		if(GUI.Button (MainMenu.AnchoredRect(), MainMenu.content, MainMenu.style))
		{
			GameObject.Find ("PlayerData").GetComponent<PlayerData>().SaveData();
			MenuController.ChangeMenu(MainMenu.menuObject, this.gameObject);
		}

		settings.musicVolume = GUI.HorizontalSlider(
			Music.AnchoredRect(), 
			settings.musicVolume, 
			0.0f, 1.0f,
			sliderBar,
			sliderThumb);


		settings.sfxVolume = GUI.HorizontalSlider(
			SFX.AnchoredRect(), 
			settings.sfxVolume, 
			0.0f, 1.0f,
			sliderBar,
			sliderThumb);

		settings.playmode = GUI.SelectionGrid(
			Mode.AnchoredRect(), 
			settings.playmode, 
			modeTextures, 
			modeTextures.Length, 
			Mode.style);
	}
}
