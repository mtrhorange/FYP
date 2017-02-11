using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour {

	public MenuButton UpBtn, DownBtn, LeftBtn, RightBtn, SubmitBtn, CancelBtn;

	public enum ButtonTypes {MainMenu, Tab, Panel, Skill, SkillSelection, KeyAssign, SkillDesc}
	public ButtonTypes btnType;

	public bool selected;

	public GameObject selectedImg;

	public int player = 1;

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

		if (player == 1)
			GameMenuManager.instance.selectedBtnP1 = this;
		else
			GameMenuManager.instance.selectedBtnP2 = this;
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

	public virtual bool MoveLeft() {
		if (LeftBtn == null)
			return false;
		else {
			Deselect ();
			LeftBtn.Select ();
			return true;
		}
	}

	public virtual bool MoveRight() {

		if (RightBtn == null)
			return false;
		else {
			Deselect ();
			RightBtn.Select ();
			return true;
		}
	}

	public virtual bool MoveDown() {

		if (DownBtn == null)
			return false;
		else {
			Deselect ();
			DownBtn.Select ();
			return true;
		}
	}

	public virtual bool MoveUp() {

		if (UpBtn == null)
			return false;
		else {
			Deselect ();
			UpBtn.Select ();
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
		GameManager.instance.SavePlayers ();
	}
	#endregion
}
