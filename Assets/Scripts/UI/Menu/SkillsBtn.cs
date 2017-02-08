using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillsBtn : MenuButton {

	public GameObject descPanel;
	public Text descTxt;
	Text lvlTxt;
	public int skillLvl;
	public string description;

	public Skills skill;
	public GameObject panel;
	public Image spellImage;

	public override void Awake () {
		btnType = ButtonTypes.Skill;
		SetSkillDescription ();

		//descPanel = transform.parent.Find ("Description").gameObject;
		//descTxt = descPanel.transform.GetChild (0).GetComponent<Text> ();
		lvlTxt = transform.Find ("SkillLvl").GetComponent<Text> ();
		//panel = transform.parent.gameObject;
		GetComponent<Image>().sprite = spellImage.sprite;
		base.Awake ();
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();
		//panel.SetActive (false);
		UpdateSkillLvl ();

	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}

	public override void Select(){
		base.Select ();
		//panel.SetActive (true);
		Active ();
		descPanel.SetActive (true);
		descTxt.text = description;

	}

	public override void Deselect() {
		base.Deselect ();
		//panel.SetActive (false);
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
			CancelBtn.GetComponent<TabBtn> ().panel = panel;
			CancelBtn.Select ();
			Deselect ();
			CancelBtn.SubmitBtn = this;
			descPanel.SetActive (false);
		}

	}

	public void SwitchPanel() {
		panel.SetActive (!panel.activeSelf);
		descPanel.SetActive (!descPanel.activeSelf);

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
		case Skills.DrainHeal:
			if (player == 1)
				skillLvl = GameManager.instance.player1.skills.drainHealLevel;
			else
				skillLvl = GameManager.instance.player2.skills.drainHealLevel;
			lvlTxt.text = "Lv." + skillLvl;
			break;

		}


	}

	public override bool MoveRight() {

		if (base.MoveRight () == false)
			return false;
		else {
			//RightBtn.GetComponent<SkillsBtn> ().Awake ();
			if (panel != RightBtn.GetComponent<SkillsBtn> ().panel) {
				SwitchPanel ();
				//RightBtn.GetComponent<SkillsBtn> ().SwitchPanel ();
				RightBtn.GetComponent<SkillsBtn>().panel.SetActive(!panel.activeSelf);
			}
			return true;
		}
	}

	public override bool MoveLeft() {

		if (base.MoveLeft () == false)
			return false;
		else {
			//RightBtn.GetComponent<SkillsBtn> ().Awake ();
			if (panel != LeftBtn.GetComponent<SkillsBtn> ().panel) {
				SwitchPanel ();
				LeftBtn.GetComponent<SkillsBtn> ().panel.SetActive(!panel.activeSelf);
			}
			return true;
		}
	}

	//Set description of skill here!!
	void SetSkillDescription() {

		switch (gameObject.name) {

		case "ActiveSkill1":
			description = "Fire Pillar\nCharge Time: " + GameManager.instance.player1.GetFirePillarTime().ToString();
			break;

		case "ActiveSkill2":
			description = "Chain Lightning\nCharge Time: " + GameManager.instance.player1.GetChainLightningTime().ToString();
			break;

		case "ActiveSkill3":
			description = "Ice Spikes\nCharge Time: " + GameManager.instance.player1.GetIceSpikeTime().ToString();
			break;

		case "ActiveSkill4":
			description = "Drain Heal\nCharge Time: " + GameManager.instance.player1.GetDrainHealTime().ToString();
			break;

		case "ActiveSkill5":
			description = "This is Active Skill 1";
			break;

		case "ActiveSkill6":
			description = "This is Active Skill 2";
			break;

		case "ActiveSkill7":
			description = "This is Active Skill 3";
			break;

		case "ActiveSkill8":
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
