using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SaveSlot : MonoBehaviour {

	public int saveId;
	public string playerName = null;
	public int playerLevel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RefreshInfo() {
		if (playerName != "")
			transform.GetChild (0).GetComponent<Text> ().text = "Name: " + playerName + "\nLevel: " + playerLevel.ToString ();
		else
			transform.GetChild (0).GetComponent<Text> ().text = "Slot " + saveId + " is empty!";

	}

}
