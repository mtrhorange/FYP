using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillSelection : MenuButton {

	public GameObject selectionPanel;
	public GameObject keyAssignPanel;
	public enum SelectionType {AddPoint, SkillInfo, KeyAssign}
	public SelectionType selectionType;
	public enum ActiveType {Active, Passive}
	public ActiveType activeType;

	public override void Awake () {
		btnType = ButtonTypes.SkillSelection;
		if (name == "AddPoint") {
			selectionType = SelectionType.AddPoint;
			DownBtn = transform.parent.Find ("SkillInfo").GetComponent<MenuButton> ();
		} else if (name == "SkillInfo") {
			selectionType = SelectionType.SkillInfo;
			if (activeType == ActiveType.Active)
				DownBtn = transform.parent.Find ("KeyAssign").GetComponent<MenuButton> ();
			UpBtn = transform.parent.Find ("AddPoint").GetComponent<MenuButton> ();
		}
		else {
			selectionType = SelectionType.KeyAssign;
			SubmitBtn = keyAssignPanel.GetComponent<MenuButton>();
			UpBtn = transform.parent.Find ("SkillInfo").GetComponent<MenuButton> ();
		}
		selectedImg = transform.GetChild (0).gameObject;
		CancelBtn = transform.parent.parent.GetComponent<MenuButton> ();
		base.Awake ();

	}

	// Use this for initialization
	public override void Start () {
		
		base.Start ();
	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}

	public override void Select(){
		base.Select ();

		selectionPanel.SetActive (true);
		//Active ();
		selectedImg.SetActive (true);
		SelectionActive ();

	}

	public override void Deselect() {
		base.Deselect ();
		selectedImg.SetActive (false);
		//Inactive ();
	}

	public override void Submit() {
		if (SubmitBtn != null && selectionType == SelectionType.KeyAssign) {
			SubmitBtn.Select ();
			SubmitBtn.CancelBtn = this;
			selected = false;
			//Inactive ();
			SelectionInactive ();
		}


	}

	public override void Cancel() {

		if (CancelBtn != null) {
			CancelBtn.Select ();
			Deselect ();
			CancelBtn.SubmitBtn = this;
			selectionPanel.SetActive (false);
		}

	}
}
