using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AoELightning : Spell {

	// Use this for initialization
	public GameObject Lightning, Cloud;
	public float betweenStrikes;
	public int Strikes;
	private float btwnStrikes;

	bool striked = false;

	void Start () {
		int lvl = player.skills.aoeLightningLevel;
		Strikes = (lvl < 15) ? (lvl + 10) : 25;
		betweenStrikes = (lvl < 15) ? (2.0f - lvl * 0.05f) : (2.0f - 15 * 0.05f);

		btwnStrikes = betweenStrikes;
		GetDamage ();
	}
	
	// Update is called once per frame
	void Update () {
		betweenStrikes -= Time.deltaTime;

		if (GetComponent<CapsuleCollider> ().enabled)
			GetComponent<CapsuleCollider> ().enabled = false;

		if (Strikes > 0 && betweenStrikes <= 0) {
			Instantiate(Lightning, Cloud.transform.position , Quaternion.Euler(90f, 0, 0));
			Strikes -= 1;
			betweenStrikes = btwnStrikes;

			GetComponent<CapsuleCollider> ().enabled = true;
		}
		if (Strikes <= 0) {
			Destroy (this.gameObject);
		}
	}

	public override float GetDamage() {

		float dmg = base.GetDamage ();
		dmg *= 0.25f;
		damage = dmg;
		return dmg;
	}

	void OnTriggerEnter(Collider other) {

		if (other.GetComponent<Enemy> () && other.GetType () == typeof(CapsuleCollider)) {
			other.GetComponent<Enemy> ().ReceiveDamage (damage, player);
		}

	}
}
