using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillDescPanel : MenuButton {

	Text text;

	public Skills skill;
	public Passives passive;
	public ActiveType type;

	float chargeCost;
	float chargeTime;
	string damage;

	Player p;
	// Use this for initialization
	void Start () {
		text =	transform.Find ("Text").GetComponent<Text>();

		if (player == 1)
			p = GameManager.instance.player1;
		else
			p = GameManager.instance.player2;

		UpdateSkill ();
		UpdateDescription ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateDescription() {

		if (type == ActiveType.Active) {

			string[] name = skill.ToString ().Split (new char[] {'.'}, System.StringSplitOptions.None);
			text.text = "Skill Description\n\n";
			text.text += "Name: " + name[0];
			text.text += "\nCharge Cost: " + chargeCost;
			text.text += "\nCharge Time: " + chargeTime;

			switch (skill) {
			case Skills.FirePillar:
				text.text += "\n\nFire Pillar shoots fire and shoot idk type something here";
				break;
			case Skills.IceSpike:
				text.text += "\n\nFire Pillar shoots fire and shoot idk type something here";
				break;
			case Skills.ChainLightning:
				text.text += "\n\nFire Pillar shoots fire and shoot idk type something here";
				break;
			case Skills.DrainHeal:
				text.text += "\n\nFire Pillar shoots fire and shoot idk type something here";
				break;
			case Skills.AoeLightning:
				text.text += "\n\nFire Pillar shoots fire and shoot idk type something here";
				break;
			case Skills.GroundSmash:
				text.text += "\n\nFire Pillar shoots fire and shoot idk type something here";
				break;
			case Skills.VerticalStrike:
				text.text += "\n\nFire Pillar shoots fire and shoot idk type something here";
				break;
			case Skills.SpearBreaker:
				text.text += "\n\nFire Pillar shoots fire and shoot idk type something here";
				break;
			}



		}

	}

	void UpdateSkill() {

		if (type == ActiveType.Active) {
			switch (skill) {

			case Skills.FirePillar:
				chargeCost = p.GetFirePillarCost ();
				chargeTime = p.GetFirePillarTime ();
				damage = "Something like 50% of player dmg";
				break;

			}
		}


	}


}
