using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SavePlayer() {

		GameManager.instance.UpdatePlayer1Data ();
		GameManager.instance.UpdatePlayer2Data ();

	}

	public void LoadMainMenu() {

		LevelManager.instance.LoadMainMenu ();
	}

	public void LoadGame() {

		LevelManager.instance.LoadGame ();

	}

	public void SelectPlayer() {

		GameManager.instance.SelectPlayer (this.GetComponent<SaveSlot> ());
	}

	public void NewCharacter() {

		GameManager.instance.NewCharacter (this.GetComponent<SaveSlot> (), FindObjectOfType<MainMenu> ().playerName);
	}
}
