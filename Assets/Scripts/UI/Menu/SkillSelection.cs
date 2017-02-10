using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillSelection : MenuButton {

	public GameObject selectionPanel;
	public GameObject keyAssignPanel;
	public enum SelectionType {AddPoint, SkillInfo, KeyAssign}
	public SelectionType selectionType;
	public ActiveType activeType;

	public Skills skill;
	public Passives passive;

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
		activeType = transform.parent.parent.GetComponent<SkillsBtn> ().activeType;
		skill = transform.parent.parent.GetComponent<SkillsBtn> ().skill;
		passive = transform.parent.parent.GetComponent<SkillsBtn> ().passive;


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

		GameMenuManager.instance.canEscape = false;

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
			keyAssignPanel.GetComponent<KeyAssignPanel> ().assignSkill = skill;
		} else if (selectionType == SelectionType.AddPoint) {

			Cancel ();
			AddSkillPoint ();


		}


	}

	public override void Cancel() {

		if (CancelBtn != null) {
			CancelBtn.Select ();
			Deselect ();
			CancelBtn.SubmitBtn = this;
			selectionPanel.SetActive (false);
			GameMenuManager.instance.canEscape = true;
		}
	}

	public void AddSkillPoint() {

		if (activeType == ActiveType.Active) {
			switch (skill) {
			case Skills.FirePillar:
				if (player == 1)
					GameManager.instance.player1.skills.firePillarLevel++;
				else
					GameManager.instance.player2.skills.firePillarLevel++;
				break;
			case Skills.IceSpike:
				if (player == 1)
					GameManager.instance.player1.skills.iceSpikesLevel++;
				else
					GameManager.instance.player2.skills.iceSpikesLevel++;
				break;
			case Skills.ChainLightning:
				if (player == 1)
					GameManager.instance.player1.skills.chainLightningLevel++;
				else
					GameManager.instance.player2.skills.chainLightningLevel++;
				break;
			case Skills.DrainHeal:
				if (player == 1)
					GameManager.instance.player1.skills.drainHealLevel++;
				else
					GameManager.instance.player2.skills.drainHealLevel++;
				break;
			case Skills.AoeLightning:
				if (player == 1)
					GameManager.instance.player1.skills.aoeLightningLevel++;
				else
					GameManager.instance.player2.skills.aoeLightningLevel++;
				break;
			case Skills.GroundSmash:
				if (player == 1)
					GameManager.instance.player1.skills.groundSmashLevel++;
				else
					GameManager.instance.player2.skills.groundSmashLevel++;
				break;
			case Skills.VerticalStrike:
				if (player == 1)
					GameManager.instance.player1.skills.verticalStrikeLevel++;
				else
					GameManager.instance.player2.skills.verticalStrikeLevel++;
				break;
			case Skills.SpearBreaker:
				if (player == 1)
					GameManager.instance.player1.skills.spearBreakerLevel++;
				else
					GameManager.instance.player2.skills.spearBreakerLevel++;
				break;
			}
		} else {

			switch (passive) {

			case Passives.MaxHealth:
				if (player == 1)
					GameManager.instance.player1.skills.maxHealthLevel++;
				else
					GameManager.instance.player2.skills.maxHealthLevel++;
				break;
			case Passives.MinDmg:
				if (player == 1)
					GameManager.instance.player1.skills.minDmgLevel++;
				else
					GameManager.instance.player2.skills.minDmgLevel++;
				break;
			case Passives.MaxDmg:
				if (player == 1)
					GameManager.instance.player1.skills.maxDmgLevel++;
				else
					GameManager.instance.player2.skills.maxDmgLevel++;
				break;
			case Passives.WeaponBuff:
				if (player == 1)
					GameManager.instance.player1.skills.weaponBuffLevel++;
				else
					GameManager.instance.player2.skills.weaponBuffLevel++;
				break;
			case Passives.SpellBuff:
				if (player == 1)
					GameManager.instance.player1.skills.spellBuffLevel++;
				else
					GameManager.instance.player2.skills.spellBuffLevel++;
				break;
			case Passives.DefenseBuff:
				if (player == 1)
					GameManager.instance.player1.skills.defenseBuffLevel++;
				else
					GameManager.instance.player2.skills.defenseBuffLevel++;
				break;
			case Passives.FrontSlash:
				if (player == 1)
					GameManager.instance.player1.skills.frontSlashLevel++;
				else
					GameManager.instance.player2.skills.frontSlashLevel++;
				break;
			case Passives.IceBoltSpike:
				if (player == 1)
					GameManager.instance.player1.skills.iceBoltSpikeLevel++;
				else
					GameManager.instance.player2.skills.iceBoltSpikeLevel++;
				break;
			}

		}

		transform.parent.parent.GetComponent<SkillsBtn> ().UpdateSkillLvl ();

		if (player == 1)
			GameManager.instance.player1.SkillPoints--;
		else
			GameManager.instance.player2.SkillPoints--;

	}
}
