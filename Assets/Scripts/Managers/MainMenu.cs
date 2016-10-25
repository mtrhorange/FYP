using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class MainMenu : MonoBehaviour {

	public string playerName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadCharacterInfo() {

		SaveSlot[] slots = FindObjectsOfType<SaveSlot> ();



		foreach (SaveSlot s in slots) {

			for (int i = 0; i < SaveLoad.savedCharacters.Count; i++) {

				if (s.saveId == SaveLoad.savedCharacters [i].saveId) {

					s.playerName = SaveLoad.savedCharacters [i].name;
					s.playerLevel = SaveLoad.savedCharacters [i].Level;

				}

			}
			s.RefreshInfo ();
		}

	}

	public void NameInput(Text input) {

		playerName = input.text;

	}

	public void NewCharacter(SaveSlot s) {

		GameManager.instance.NewCharacter (s, playerName);

	}


}
