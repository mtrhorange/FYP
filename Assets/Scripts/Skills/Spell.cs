using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

	public enum Type { Bolt, Area}
	public enum EffectType { None, Burn, Slow, Stun }

	public float damage;
	public Player player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual float GetDamage() {

		float dmg = Random.Range (player.currentWeapon.damageMin, player.currentWeapon.damageMax);
		dmg = dmg * (1f + 0.05f * player.skills.spellBuffLevel);
		damage = dmg;

		return damage;

	}
}
