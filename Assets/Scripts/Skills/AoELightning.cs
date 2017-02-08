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
		btwnStrikes = betweenStrikes;
		GetDamage ();
	}
	
	// Update is called once per frame
	void Update () {
		betweenStrikes -= Time.deltaTime;

		if (GetComponent<CapsuleCollider> ().enabled)
			GetComponent<CapsuleCollider> ().enabled = false;

		if (Strikes > 0 && betweenStrikes <= 0) {
			Instantiate(Lightning, Cloud.transform.position , Cloud.transform.rotation);
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
