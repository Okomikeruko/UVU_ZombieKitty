using UnityEngine;
using System.Collections;

public class PlayMenuGUI : MonoBehaviour {
	
	[SerializeField]
	public ButtonClass BeginnerLevels, IntermediateLevels, AdvancedLevels, Back, Resume, NewGame;
	private PlayerData playerData;

	void Start(){
		playerData = GameObject.Find ("PlayerData").GetComponent<PlayerData>();
	}
	
	void OnGUI(){
		if(GUI.Button (NewGame.AnchoredRect(), NewGame.content, NewGame.style))
		{

		}
		if(GUI.Button (Resume.AnchoredRect(), Resume.content, Resume.style))
		{
			
		}
		if(GUI.Button (Back.AnchoredRect(), Back.content, Back.style))
		{
			MenuController.ChangeMenu(Back.menuObject, this.gameObject);
		}



		if(GUI.Button (BeginnerLevels.AnchoredRect(), BeginnerLevels.content, BeginnerLevels.style))
		{

		}
		GUI.enabled = playerData.CurrentPlayer.progress.Level.Count > 3;
		if(GUI.Button (IntermediateLevels.AnchoredRect (), IntermediateLevels.content, IntermediateLevels.style))
		{

		}
		GUI.enabled = playerData.CurrentPlayer.progress.Level.Count > 6;
		if(GUI.Button (AdvancedLevels.AnchoredRect(), AdvancedLevels.content, AdvancedLevels.style))
		{

		}
	}
}
