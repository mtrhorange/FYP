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
				text.text += "\n\nCast a fire pillar in front of the player and damage all monsters within the pillar’s range. Summons on the strongest enemy";
				break;
			case Skills.IceSpike:
				text.text += "\n\nCast an ice spike in front of the player, damage all enemies hit.";
				break;
			case Skills.ChainLightning:
				text.text += "\n\nCast lightning and damage the nearest monster near it. The lightning will reflect off the first target and damage the next nearest monster. The number of lightning reflection depends on the skill’s level.";
				break;
			case Skills.DrainHeal:
				text.text += "\n\nCast a spell circle on the ground which will drain health from monsters that are inside the radius to the player.";
				break;
			case Skills.AoeLightning:
				text.text += "\n\nCast a lightning cloud that will damage all monsters within the area.";
				break;
			case Skills.GroundSmash:
				text.text += "\n\nCast a giant spear in the air that will smash into the ground 45° down and cause a earth fissure which damages all enemies all monsters within the area.";
				break;
			case Skills.VerticalStrike:
				text.text += "\n\nCast a ground spike in front of the player that will damage all monsters within its range.";
				break;
			case Skills.SpearBreaker:
				text.text += "\n\nCast a giant polearm in the air that will slam into the ground and damage all monsters in its way. Summons on the strongest enemy";
				break;
			}



		}

	}

	void UpdateSkill() {

		if (type == ActiveType.Active) {
            switch (skill)
            {

                case Skills.AoeLightning:
                    chargeCost = p.GetAoeLightningCost();
                    chargeTime = p.GetAoeLightningTime();
                    damage = "Something like 50% of player dmg";
                    break;
                case Skills.ChainLightning:
                    chargeCost = p.GetChainLightningCost();
                    chargeTime = p.GetChainLightningTime();
                    damage = "Something like 50% of player dmg";
                    break;
                case Skills.DrainHeal:
                    chargeCost = p.GetDrainHealCost();
                    chargeTime = p.GetDrainHealTime();
                    damage = "Something like 50% of player dmg";
                    break;
                case Skills.FirePillar:
                    chargeCost = p.GetFirePillarCost();
                    chargeTime = p.GetFirePillarTime();
                    damage = "Something like 50% of player dmg";
                    break;
                case Skills.GroundSmash:
                    chargeCost = p.GetGroundSmashCost();
                    chargeTime = p.GetGroundSmashTime();
                    damage = "Something like 50% of player dmg";
                    break;
                case Skills.IceSpike:
                    chargeCost = p.GetIceSpikeCost();
                    chargeTime = p.GetIceSpikeTime();
                    damage = "Something like 50% of player dmg";
                    break;
                case Skills.SpearBreaker:
                    chargeCost = p.GetSpearBreakerCost();
                    chargeTime = p.GetSpearBreakerTime();
                    damage = "Something like 50% of player dmg";
                    break;
                case Skills.VerticalStrike:
                    chargeCost = p.GetVerticalStrikeCost();
                    chargeTime = p.GetVerticalStrikeTime();
                    damage = "Something like 50% of player dmg";
                    break;
            }
		}


	}


}
