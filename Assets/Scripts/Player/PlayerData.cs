using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class PlayerData {

	public string name = " ";
	public int saveId = -1;
	public float maxHealth = 100f;
	public float baseMaxHealth = 100f;
	public float health = 100f;
	public float maxStamina = 100f;
	public float baseMaxStamina = 100f;
	public float stamina = 100f;
	public float spellStaminaDrain = 5f;
	public int livesRemaining = 5;
	public int skillPoints = 0;
	public bool recoverStamina = true; //Can stamina be recovered
	public bool isPermaDead = false;

	public int level = 1;
	public float exp = 0;

	//Skills
	public Skills skillCType, skillVType, skillAType, skillSType, skillDType;

	public int firePillarLevel = 0;
	public int iceSpikesLevel = 0;
	public int chainLightningLevel = 0;
	public int drainHealLevel = 0;
	public int aoeLightningLevel = 0;
	public int groundSmashLevel = 0;
	public int verticalStrikeLevel = 0;

	//Passives
	public int maxHealthLevel = 0;
	public int minDmgLevel = 0;
	public int maxDmgLevel = 0;
	public int weaponBuffLevel = 0;
	public int spellBuffLevel = 0;
	public int defenseBuffLevel = 0;
	public int frontSlashLevel = 0;
	public int iceBoltSpikeLevel = 0;

	public PlayerData(string n, int sId) {

		name = n;
		saveId = sId;

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Player's current health
	public float Health {
		get {
			return health;
		}
		set {
			if (value > maxHealth)
				health = maxHealth;
			else
				health = value;
		}

	}

	//Player max health
	public float MaxHealth {
		get {
			return maxHealth;
		}

		set {
			maxHealth = value;
			if (health > maxHealth)
				health = maxHealth;
		}


	}

	//Player stamina
	public float Stamina {
		get { return stamina; }
		set {
			if (value > maxStamina)
				stamina = maxStamina;
			else
				stamina = value;
		}


	}

	//Player Max Stamina
	public float MaxStamina { 
		get { return maxStamina; }
		set { 
			maxStamina = value; 
			if (stamina > maxStamina)
				stamina = maxStamina;
		}
	}

	//Player current level
	public int Level {

		get { return level; }
		set {
			if (value < 0)
				level = 1;
			else
				level = value;
		}

	}

	//Player exp & levelup
	public float Exp {

		get { return exp; }
		set {
			exp = value;
			float expLimit = 50 + (50 * level);
			if (exp >= expLimit) {

				level++;
				exp -= expLimit;

			}
		}

	}

	public int SkillPoints {

		get { return skillPoints; }
		set {
			skillPoints = value;
		}

	}

	public int Lives {

		get { return livesRemaining; }
		set { livesRemaining = value; }
	}
}
