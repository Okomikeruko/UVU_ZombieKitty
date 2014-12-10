using UnityEngine;
using System.Collections;

public class iOSbehaviour : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(this.gameObject);

		Screen.orientation = ScreenOrientation.LandscapeLeft;

		Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = false;

		Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = true;
	}

	void Start() {
		Screen.orientation = ScreenOrientation.AutoRotation;
	}
}
