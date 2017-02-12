using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ActiveType {Active, Passive}


public class SkillsBtn : MenuButton {

	public GameObject descPanel;
	public Text descTxt;
	Text lvlTxt;
	public int skillLvl;
	public string description;

	public Skills skill;
	public Passives passive;
	public GameObject panel;
	public Image spellImage;
	public ActiveType activeType;

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

		if (activeType == ActiveType.Active) {
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
			case Skills.AoeLightning:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.aoeLightningLevel;
				else
					skillLvl = GameManager.instance.player2.skills.aoeLightningLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			case Skills.GroundSmash:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.groundSmashLevel;
				else
					skillLvl = GameManager.instance.player2.skills.groundSmashLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			case Skills.VerticalStrike:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.verticalStrikeLevel;
				else
					skillLvl = GameManager.instance.player2.skills.verticalStrikeLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			case Skills.SpearBreaker:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.spearBreakerLevel;
				else
					skillLvl = GameManager.instance.player2.skills.spearBreakerLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			}
		} else {
			switch (passive) {

			case Passives.DefenseBuff:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.defenseBuffLevel;
				else
					skillLvl = GameManager.instance.player2.skills.defenseBuffLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			case Passives.FrontSlash:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.frontSlashLevel;
				else
					skillLvl = GameManager.instance.player2.skills.frontSlashLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			case Passives.IceBoltSpike:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.iceBoltSpikeLevel;
				else
					skillLvl = GameManager.instance.player2.skills.iceBoltSpikeLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			case Passives.MaxDmg:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.maxDmgLevel;
				else
					skillLvl = GameManager.instance.player2.skills.maxDmgLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			case Passives.MaxHealth:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.maxHealthLevel;
				else
					skillLvl = GameManager.instance.player2.skills.maxHealthLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			case Passives.MinDmg:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.minDmgLevel;
				else
					skillLvl = GameManager.instance.player2.skills.minDmgLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			case Passives.SpellBuff:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.spellBuffLevel;
				else
					skillLvl = GameManager.instance.player2.skills.spellBuffLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			case Passives.WeaponBuff:
				if (player == 1)
					skillLvl = GameManager.instance.player1.skills.weaponBuffLevel;
				else
					skillLvl = GameManager.instance.player2.skills.weaponBuffLevel;
				lvlTxt.text = "Lv." + skillLvl;
				break;
			}
				
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
			description = "Fire Pillar\nCharge Time: " + GameManager.instance.player1.GetFirePillarTime().ToString() 
                    + "\nCharge Cost: " + GameManager.instance.player1.GetFirePillarCost().ToString();
			break;

		case "ActiveSkill2":
			description = "Chain Lightning\nCharge Time: " + GameManager.instance.player1.GetChainLightningTime().ToString()
                    + "\nCharge Cost: " + GameManager.instance.player1.GetChainLightningCost().ToString();
                break;

		case "ActiveSkill3":
			description = "Ice Spikes\nCharge Time: " + GameManager.instance.player1.GetIceSpikeTime().ToString()
                    + "\nCharge Cost: " + GameManager.instance.player1.GetIceSpikeCost().ToString();
                break;

		case "ActiveSkill4":
			description = "Drain Heal\nCharge Time: " + GameManager.instance.player1.GetDrainHealTime().ToString()
                    + "\nCharge Cost: " + GameManager.instance.player1.GetDrainHealCost().ToString();
                break;

		case "ActiveSkill5":
			description = "AOE Lightning\nCharge Time: " + GameManager.instance.player1.GetAoeLightningTime().ToString()
                    + "\nCharge Cost: " + GameManager.instance.player1.GetAoeLightningCost().ToString();
                break;

		case "ActiveSkill6":
			description = "Ground Smash\nCharge Time: " + GameManager.instance.player1.GetGroundSmashTime().ToString()
                    + "\nCharge Cost: " + GameManager.instance.player1.GetFrontSlashCost().ToString();
                break;

		case "ActiveSkill7":
			description = "Spear Breaker\nCharge Time: " + GameManager.instance.player1.GetSpearBreakerTime().ToString()
                    + "\nCharge Cost: " + GameManager.instance.player1.GetSpearBreakerCost().ToString();
                break;

		case "ActiveSkill8":
			description = "Vertical Strike\nCharge Time: " + GameManager.instance.player1.GetVerticalStrikeTime().ToString()
                    + "\nCharge Cost: " + GameManager.instance.player1.GetVerticalStrikeCost().ToString();
                break;

            case "PassiveSkill1":
                description = "+ Max Health";
                break;

            case "PassiveSkill2":
                description = "+ Minimum Damage";
                break;

            case "PassiveSkill3":
                description = "+ Maximum Damage";
                break;

            case "PassiveSkill4":
                description = "Weapon Buff";
                break;
            case "PassiveSkill5":
                description = "Spell Buff";
                break;

            case "PassiveSkill6":
                description = "+ Defense";
                break;

            case "PassiveSkill7":
                description = "Front Slash chance";
                break;

            case "PassiveSkill8":
                description = "Ice Bolt Spike chance";
                break;

        }
	}
}
