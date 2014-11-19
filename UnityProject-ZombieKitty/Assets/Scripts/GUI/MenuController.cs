using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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