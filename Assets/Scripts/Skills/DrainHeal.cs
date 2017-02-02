using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrainHeal : Spell {

	public List<Enemy> enemiesHit;
	public List<float> enemyHitTime;

	public float hitTime = 0.75f;
	public float lifeSpan = 3.5f;
	// Use this for initialization
	void Start () {
		enemiesHit = new List<Enemy> ();
		enemyHitTime = new List<float> ();

		GetDamage ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateEnemiesHit ();

		lifeSpan -= Time.deltaTime;
		if (lifeSpan < 0)
			Destroy (gameObject);
	}

	void UpdateEnemiesHit() {



		for (int i = enemyHitTime.Count - 1; i > 0; i--) {
			enemyHitTime [i] -= Time.deltaTime;
			if (enemyHitTime[i] <= 0) {
				enemyHitTime.RemoveAt (i);
				enemiesHit.RemoveAt (i);
			}
		}
	}

	void OnTriggerStay(Collider other) {

		if (other.GetComponent<Enemy>() && other.GetType() == typeof(CapsuleCollider))
		{
			bool hit = true;
			foreach (Enemy enemy in enemiesHit) {
				if (other.GetComponent<Enemy> () == enemy) {
					hit = false;
				}
			}
			if (hit == true) {
				enemiesHit.Add (other.GetComponent<Enemy> ());
				enemyHitTime.Add (hitTime);

				float dmg = damage * 0.5f;
				other.GetComponent<Enemy> ().ReceiveDamage (dmg, player);
				player.ReceiveHeal (dmg * 0.5f);
			}
		}

	}
}
