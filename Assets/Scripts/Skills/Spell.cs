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

	public float GetDamage() {

		damage = Random.Range (player.currentWeapon.damageMin, player.currentWeapon.damageMax);
		return damage;

	}
}
