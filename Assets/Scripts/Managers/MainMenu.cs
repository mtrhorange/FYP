using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class MainMenu : MonoBehaviour {

	public string playerName;
    public Slider BGM;
    public Slider SFX;

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.HasKey("BGM"))
        {
            BGM.value = PlayerPrefs.GetFloat("BGM");
        }

        if (PlayerPrefs.HasKey("SFX"))
        {
            SFX.value = PlayerPrefs.GetFloat("SFX");
        }
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

    public void setBGM()
    {
        BGMManager.instance.setBGM(BGM.value);
        PlayerPrefs.SetFloat("BGM", BGM.value);
    }

    public void setSFX()
    {
        SFXManager.instance.setSFX(SFX.value);
        PlayerPrefs.SetFloat("SFX", SFX.value);
    }
}
