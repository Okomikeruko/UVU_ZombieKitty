using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public static void ChangeMenu(GameObject obj, GameObject self)
	{
		self.SetActive(false);
		obj.SetActive(true);
	}
}

[System.Serializable]
public class ButtonClass {
	public string title;
	public Rect rect;
	public GameObject menuObject;
}