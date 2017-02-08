using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Skills{ None, FirePillar, IceSpike, ChainLightning, DrainHeal, AoeLightning, GroundSmash, VerticalStrike, SpearBreaker, FireBall}
public enum Passives{ None, MaxHealth, MinDmg, MaxDmg, WeaponBuff, SpellBuff, DefenseBuff, FrontSlash, IceBoltSpike} 

public class PlayerSkills : MonoBehaviour {

	//Spells
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

	Player player;



	// Use this for initialization
	void Start () {
		player = GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
