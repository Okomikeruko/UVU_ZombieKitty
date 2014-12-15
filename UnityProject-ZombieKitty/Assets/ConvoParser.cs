using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

public class ConvoParser : MonoBehaviour {

	public Script script;
	public Scene currentScene;
	public string Filename;
	private string Data, _Location; 

	void Awake () {
		DontDestroyOnLoad(this.gameObject);
		_Location = Application.dataPath + "\\XML";
		LoadScript();
		if(Data.ToString() != "") {
			script = (Script)DeserializeObject(Data);
		}
		currentScene = script.scenes[0];
	}

	void LoadScript() {
		StreamReader r = File.OpenText(_Location + "\\" + Filename);
		string info = r.ReadToEnd();
		r.Close();
		Data = info;
	}

	object DeserializeObject (string pXmlizedString) {
		XmlSerializer xs = new XmlSerializer(typeof(Script));
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
		return xs.Deserialize(memoryStream);
	}

	byte[] StringToUTF8ByteArray(string pXmlString) {
		UTF8Encoding encoding = new UTF8Encoding();
		byte[] byteArray = encoding.GetBytes(pXmlString);
		return byteArray;
	}

	public void LoadNextScene(){
		int index = script.scenes.FindIndex(s => s == currentScene);
		if (++index < script.scenes.Count) {
			currentScene = script.scenes[index];
		}
	}
}

[XmlRoot("root")]
public class Script
{
	[XmlElement("scene")]
	public List<Scene> scenes { get; set; }

	public Script() {
		scenes = new List<Scene>();
	}
}

public class Scene
{
	[XmlAttribute("num")]
	public int num { get; set; }

	[XmlElement("line")]
	public List<Line> lines { get; set; }

	public Scene() {
		lines = new List<Line>();
	}
}

public class Line
{
	[XmlAttribute("speaker")]
	public string speaker { get; set; }

	[XmlAttribute("emotion")]
	public string emotion { get; set; }

	[XmlAttribute("event")] 
	public string _event { get; set; }

	[XmlText]
	public string dialog { get; set; }
}
