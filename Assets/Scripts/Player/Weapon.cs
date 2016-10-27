using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {

	public enum Type { UNARMED};

	public float damageMin = 10f;
	public float damageMax = 13f;

	GameObject capCast1, capCast2;

	Vector3 prevPos;

	bool isAttacking = false;
	bool newHit = true;

	List<Enemy> enemiesHit;

	// Use this for initialization
	void Start () {
		capCast1 = transform.Find ("CapCast1").gameObject;
		capCast2 = transform.Find ("CapCast2").gameObject;
		prevPos = transform.position;

		enemiesHit = new List<Enemy> ();
	}

	public Weapon(float min, float max) {
		damageMin = min;
		damageMax = max;

	}

	public void AttackTrigger(int i) {

		if (i == 1)
			isAttacking = true;
		else {
			isAttacking = false;
			enemiesHit.Clear ();
		}

	}

	// Update is called once per frame
	void Update () {
		if (isAttacking)
			CheckHitEnemy ();

		prevPos = transform.position;
	}

	void CheckHitEnemy() {

		RaycastHit[] hits = Physics.CapsuleCastAll (capCast1.transform.position, capCast2.transform.position, capCast1.GetComponent<SphereCollider> ().radius, prevPos - transform.position, Vector3.Distance (prevPos, transform.position));

		for (int i = 0; i < hits.Length; i++) {
			RaycastHit hit = hits[i];

			if (hit.transform.GetComponent<Enemy> ()) {
				Enemy enemy = hit.transform.GetComponent<Enemy> ();
				foreach (Enemy e in enemiesHit) {

					if (e == enemy)
						newHit = false;
				}

				if (newHit == true) {
					hit.transform.GetComponent<Enemy> ().ReceiveDamage (Random.Range (damageMin, damageMax));
					enemiesHit.Add (enemy);
				}


				newHit = true;
			}


		}


	}
}
