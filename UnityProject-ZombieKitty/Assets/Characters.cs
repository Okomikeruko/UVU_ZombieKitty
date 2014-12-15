using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Characters : MonoBehaviour {

	[SerializeField]
	public Cast cast;
}

public enum CharacterPosition{
	Left,
	Right,
};

[System.Serializable]
public class Cast{
	[SerializeField]
	public List<Character> characters;
}

[System.Serializable]
public class Character{
	public string name;
	public CharacterPosition position;
	[SerializeField]
	public List<Emotion> emotions;
}

[System.Serializable]
public class Emotion
{
	public string name;
	public Texture image;
}