using UnityEngine;
using System.Collections;

public class FrontSlash : Spell {

	float colliderTimer = 0.3f;
	bool collided = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		colliderTimer -= Time.deltaTime;

		if (GetComponent<SphereCollider> ().enabled)
			GetComponent<SphereCollider> ().enabled = false;

		if (colliderTimer <= 0 && collided == false) {
			GetComponent<SphereCollider> ().enabled = true;
			collided = true;
		}
	}

	void OnTriggerEnter(Collider other) {

		if (other.GetComponent<Enemy> () && other.GetType () == typeof(CapsuleCollider)) {

			float dmg = GetDamage ();
			dmg *= 0.5f;

			other.GetComponent<Enemy> ().ReceiveDamage (dmg, player);
		}

	}
}
