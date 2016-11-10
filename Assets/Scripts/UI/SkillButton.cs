using UnityEngine;
using System.Collections;

public class SkillButton : MonoBehaviour {

	public int playerNo = 1;

	public enum Skills
	{
		maxHealthBuff,
		minDmg,
		maxDmg,
		weaponBuff,
		spellBuff,
		defenseBuff
	}

	public Skills skillType;
	GameManager gm;
	// Use this for initialization
	void Start () {
		gm = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void IncreaseSkill() {



	}

	void IncreaseMaxHealthBuff() {

		if (playerNo == 1) {

		}

	}
}
