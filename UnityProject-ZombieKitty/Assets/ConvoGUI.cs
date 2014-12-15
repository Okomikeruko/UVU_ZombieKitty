using UnityEngine;
using UnityEditor;
using System;
using System.Collections;


public class ConvoGUI : MonoBehaviour {

	private Cast cast;
	private Scene scene;
	private int lineNum;
	private bool isPrinting;

	public float PrintSpeed;
	public ButtonClass Left, Right, Box;

	void Start () {
		cast = GetComponent<Characters>().cast;
		scene = GameObject.Find ("Convo Parser").GetComponent<ConvoParser>().currentScene;
		lineNum = 0;
		foreach (Character c in cast.characters)
		{
			Emotion neutral = c.emotions.Find(e => e.name == "Neutral");
			setImage(neutral.image, c.position);
		}
		UpdateBox();
	}
	
	void OnGUI () {
		GUI.DrawTexture (Right.AnchoredRect(), Right.content.image);
		GUI.DrawTexture (Left.AnchoredRect(), Left.content.image); 
		if(GUI.Button (Box.AnchoredRect(), Box.content, Box.style)) { 
			UpdateBox();
		}
	}

	void UpdateBox() {
		if (lineNum < scene.lines.Count) {
			Line line = scene.lines[lineNum];
			Character character = cast.characters.Find (c => c.name == line.speaker);
			Emotion e = character.emotions.Find (emotion => emotion.name == line.emotion);

			switch(character.name){
			case "Dr. Paws":
				audio.pitch = 1;
				break;
			case "Nurse Whiskers":
				audio.pitch = 1.6F;
				break;
			default:
				break;
			}

			setImage (e.image, character.position);

			if(!isPrinting){
				StartCoroutine(AnimateText (line.speaker + ": ", line.dialog, line._event));
			} else {
				isPrinting = false;
				Box.content.text = line.speaker + ": " + line.dialog;
			}
		} else {
			GameObject.Find ("Convo Parser").GetComponent<ConvoParser>().LoadNextScene();
			Application.LoadLevel ("Game");
		}
	}

	IEnumerator AnimateText(string start, string complete, string _event){
		int i = 0;
		Box.content.text = start;
		isPrinting = true;
		while ( i < complete.Length && isPrinting) {
			Box.content.text += complete[i];
			if(Char.IsLetter(complete[i++])) {
				audio.Play();
			}
			yield return new WaitForSeconds(PrintSpeed);
		}
		isPrinting = false;
		lineNum++;
		if(_event != null)
		{
			switch (_event)
			{
			case "Tutorial":
				if(EditorUtility.DisplayDialog(
					"Do you want to view the Tutorial?",
					"",
					"Yes",
					"No")
				   ){
					Debug.Log ("Play Tutorial");
				}
				UpdateBox ();
				break;
			default:
				break;
			}
		}
	}

	void setImage(Texture image, CharacterPosition p){
		switch(p){
		case CharacterPosition.Left:
			Left.content.image = image;
			break;
		case CharacterPosition.Right:
			Right.content.image = image;
			break;
		default:
			break;
		}
	}
}
