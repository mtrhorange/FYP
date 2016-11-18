using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

	public MenuButton UpBtn, DownBtn, LeftBtn, RightBtn, SelectBtn, CancelBtn;

	public bool selected;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CheckInput() {
		if (Input.GetButtonDown ("AttackL"))
			Select ();
		if (Input.GetButtonDown ("AttackR"))
			Cancel ();
	}

	public void Select() {


	}

	void Cancel() {

	}

	public bool MoveLeft() {
		if (LeftBtn == null)
			return false;
		else {


			return true;
		}

	}


	#region LevelManager

	public void LoadMainMenu() {

		LevelManager.instance.LoadMainMenu ();
	}
    #endregion

	#region GameManager

	public void SelectPlayer() {

		GameManager.instance.SelectPlayer (this.GetComponent<SaveSlot> ());
	}

	public void NewCharacter() {

		GameManager.instance.NewCharacter (this.GetComponent<SaveSlot> (), FindObjectOfType<MainMenu> ().playerName);
	}

	#endregion

	#region SaveLoad

	public void LoadGame() {

		LevelManager.instance.LoadGame ();

	}

	public void SavePlayer() {
		GameManager.instance.UpdatePlayer1Data ();
		if (GameManager.instance.player2Data.saveId != 0)
			GameManager.instance.UpdatePlayer2Data ();
	}
	#endregion
}
