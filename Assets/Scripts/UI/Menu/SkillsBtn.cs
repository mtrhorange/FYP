using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillsBtn : MenuButton {

	GameObject descPanel;
	Text descTxt;
	public string description;


	public override void Awake () {
		btnType = ButtonTypes.Skill;
		SetSkillDescription ();

		descPanel = transform.parent.Find ("Description").gameObject;
		descTxt = descPanel.transform.GetChild (0).GetComponent<Text> ();
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
		Active ();
		descPanel.SetActive (true);
		descTxt.text = description;
	}

	public override void Deselect() {
		base.Deselect ();
		Inactive ();
	}

	public override void Submit() {
		if (SubmitBtn != null) {
			SubmitBtn.Select ();
			selected = false;
			Inactive ();
		}


	}

	public override void Cancel() {

		if (CancelBtn != null) {
			CancelBtn.Select ();
			Deselect ();
			CancelBtn.SubmitBtn = this;
			descPanel.SetActive (false);
		}

	}

	//Set description of skill here!!
	void SetSkillDescription() {

		switch (gameObject.name) {

		case "ActiveSkill1":
			description = "This is Active Skill 1";
			break;

		case "ActiveSkill2":
			description = "This is Active Skill 2";
			break;

		case "ActiveSkill3":
			description = "This is Active Skill 3";
			break;

		case "ActiveSkill4":
			description = "This is Active Skill 4";
			break;

		case "PassiveSkill1":
			description = "This is Passive Skill 1";
			break;

		case "PassiveSkill2":
			description = "This is Passive Skill 2";
			break;

		case "PassiveSkill3":
			description = "This is Passive Skill 3";
			break;

		case "PassiveSkill4":
			description = "This is Passive Skill 4";
			break;
		}

	}
}
