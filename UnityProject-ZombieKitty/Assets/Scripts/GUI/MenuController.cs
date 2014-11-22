using UnityEngine;
using System.Collections;


public class MenuController : MonoBehaviour {

	public static void ChangeMenu(GameObject obj, GameObject self)
	{
		self.SetActive(false);
		obj.SetActive(true);
	}
}

public enum Anchor{
	TopLeft,
	TopCenter,
	TopRight,
	MiddleLeft,
	MiddleCenter,
	MiddleRight,
	BottomLeft,
	BottomCenter,
	BottomRight};

[System.Serializable]
public class ButtonClass {

	
	public string title;
	[SerializeField]
	public Anchor anchorPoint;
	public Rect rect;
	public GUIContent content;
	public GUIStyle style;
	public GameObject menuObject;

	public Rect AnchoredRect()
	{
		Rect output = rect;
		switch (anchorPoint)
		{
		case Anchor.TopLeft:
			break;
		case Anchor.TopCenter:
			output.x = (Screen.width / 2) + rect.x;
			break;
		case Anchor.TopRight:
			output.x = Screen.width + rect.x;
			break;
		case Anchor.MiddleLeft:
			output.y = (Screen.height / 2) + rect.y;
			break;
		case Anchor.MiddleCenter:
			output.x = (Screen.width / 2) + rect.x;
			output.y = (Screen.height / 2) + rect.y;
			break;
		case Anchor.MiddleRight:
			output.x = Screen.width + rect.x;
			output.y = (Screen.height / 2) + rect.y;
			break;
		case Anchor.BottomLeft:
			output.y = Screen.height + rect.y;
			break;
		case Anchor.BottomCenter:
			output.x = (Screen.width / 2) + rect.x;
			output.y = Screen.height + rect.y;
			break;
		case Anchor.BottomRight:
			output.x = Screen.width + rect.x;
			output.y = Screen.height + rect.y;
			break;
		default:
			break;
		}
		return output;
	}
}