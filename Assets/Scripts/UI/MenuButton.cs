using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour {

	public MenuButton UpBtn, DownBtn, LeftBtn, RightBtn, SubmitBtn, CancelBtn;

	public enum ButtonTypes {MainMenu, Tab, Panel, Skill, SkillSelection, KeyAssign}
	public ButtonTypes btnType;


	public bool selected;

	public GameObject selectedImg;

	public virtual void Awake() {



	}

	// Use this for initialization
	public virtual void Start () {
		if (btnType != ButtonTypes.MainMenu) {
			

			if (!selected)
				Deselect ();
			else
				Select ();

		}


	}
	
	// Update is called once per frame
	public virtual void Update () {
		
	}



	public virtual void Select() {
		selected = true;

		GameMenuManager.instance.selectedBtnP1 = this;
	}

	public virtual void Deselect() {
		selected = false;


	}

	public virtual void Submit() {


	}

	public virtual void Cancel() {

	}

	public void Inactive() {
		Color color = GetComponent<Image> ().color;
		GetComponent<Image> ().color = new Color (color.r, color.g, color.b, 0.5f);

	}

	public void Active() {
		Color color = GetComponent<Image> ().color;
		GetComponent<Image> ().color = new Color (color.r, color.g, color.b, 1f);

	}

	public void SelectionActive() {
		Color color = selectedImg.GetComponent<Image> ().color;
		selectedImg.GetComponent<Image> ().color = new Color (color.r, color.g, color.b, 1f);
		if (selectedImg.GetComponent<Animator> ())
			selectedImg.GetComponent<Animator> ().enabled = true;

	}

	public void SelectionInactive() {
		Color color = selectedImg.GetComponent<Image> ().color;
		selectedImg.GetComponent<Image> ().color = new Color (color.r, color.g, color.b, 0.5f);
		if (selectedImg.GetComponent<Animator> ())
			selectedImg.GetComponent<Animator> ().enabled = false;

	}

	public bool MoveLeft() {
		if (LeftBtn == null)
			return false;
		else {
			LeftBtn.Select ();
			Deselect ();
			return true;
		}
	}

	public bool MoveRight() {

		if (RightBtn == null)
			return false;
		else {
			RightBtn.Select ();
			Deselect ();
			return true;
		}
	}

	public bool MoveDown() {

		if (DownBtn == null)
			return false;
		else {
			DownBtn.Select ();
			Deselect ();
			return true;
		}
	}

	public bool MoveUp() {

		if (UpBtn == null)
			return false;
		else {
			UpBtn.Select ();
			Deselect ();
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

	public void QuitGame() {

		Application.Quit ();

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
