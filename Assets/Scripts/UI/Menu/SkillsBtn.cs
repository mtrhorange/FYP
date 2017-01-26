using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillsBtn : MenuButton {

	GameObject descPanel;
	Text descTxt;
	Text lvlTxt;
	public int skillLvl;
	public string description;

	public Skills skill;


	public override void Awake () {
		btnType = ButtonTypes.Skill;
		SetSkillDescription ();

		descPanel = transform.parent.Find ("Description").gameObject;
		descTxt = descPanel.transform.GetChild (0).GetComponent<Text> ();
		lvlTxt = transform.Find ("SkillLvl").GetComponent<Text> ();
		base.Awake ();
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();

		UpdateSkillLvl ();

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

	public void UpdateSkillLvl() {

		switch (skill) {

		case Skills.FirePillar:
			if (player == 1)
				skillLvl = GameManager.instance.player1.skills.firePillarLevel;
			else
				skillLvl = GameManager.instance.player2.skills.firePillarLevel;
			lvlTxt.text = "Lv." + skillLvl;
			break;
		case Skills.ChainLightning:
			if (player == 1)
				skillLvl = GameManager.instance.player1.skills.chainLightningLevel;
			else
				skillLvl = GameManager.instance.player2.skills.chainLightningLevel;
			lvlTxt.text = "Lv." + skillLvl;
			break;
		case Skills.IceSpike:
			if (player == 1)
				skillLvl = GameManager.instance.player1.skills.iceSpikesLevel;
			else
				skillLvl = GameManager.instance.player2.skills.iceSpikesLevel;
			lvlTxt.text = "Lv." + skillLvl;
			break;

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
