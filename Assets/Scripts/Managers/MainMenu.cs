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

		SaveLoad.Load ();

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

		PlayerData player = new PlayerData (playerName, s.saveId);

		bool existing = false;
		for (int i = 0; i < SaveLoad.savedCharacters.Count; i++) {

			if (player.saveId == SaveLoad.savedCharacters [i].saveId) {
				existing = true;
				SaveLoad.savedCharacters.RemoveAt (i);


			}

		}


		SaveLoad.NewCharacter (player);
			

	}
}
