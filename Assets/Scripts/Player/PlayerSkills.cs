using UnityEngine;
using System.Collections;

public class PlayerSkills : MonoBehaviour {

	//Spells

	//Passives
	public int maxHealthLevel = 0;
	public int minDmgLevel = 0;
	public int maxDmgLevel = 0;
	public int weaponBuffLevel = 0;
	public int spellBuffLevel = 0;
	public int defenseBuffLevel = 0;

	Player player;
	// Use this for initialization
	void Start () {
		player = GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
