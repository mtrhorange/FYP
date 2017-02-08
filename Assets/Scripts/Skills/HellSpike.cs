using UnityEngine;
using System.Collections;

public class HellSpike : Spell {

	float colliderSpan = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		colliderSpan -= Time.deltaTime;

		if (GetComponent<CapsuleCollider> ().enabled && colliderSpan < 0)
			GetComponent<CapsuleCollider> ().enabled = false;
	}

	void OnTriggerEnter(Collider other) {

		if (other.GetComponent<Enemy> () && other.GetType () == typeof(CapsuleCollider)) {

			float dmg = GetDamage ();

			other.GetComponent<Enemy> ().ReceiveDamage (dmg, player);

		}

	}
}
