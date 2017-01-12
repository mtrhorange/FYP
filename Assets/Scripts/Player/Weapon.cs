using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {

	public enum Types { Sword, Staff};
	public Types type;
	public float damageMin;
	public float damageMax;

	public int level = 1;

	GameObject capCast1, capCast2;

	Vector3 prevPos;

	bool isAttacking = false;
	public bool isMelee = true;
	bool newHit = true;

	List<Enemy> enemiesHit;

	Transform interactionPt;

	public Player player;

	PlayerSkills skills;

	// Use this for initialization
	public virtual void Start () {
		prevPos = transform.position;
		enemiesHit = new List<Enemy> ();

		if (isMelee) {
			capCast1 = transform.Find ("CapCast1").gameObject;
			capCast2 = transform.Find ("CapCast2").gameObject;
		}


		UpdateDamage ();


	}

	public Weapon(float min, float max) {
		damageMin = min;
		damageMax = max;

	}

	public virtual void AttackTrigger(int i) {

		if (i == 1) {
			isAttacking = true;
			SFXManager.instance.playSFX (sounds.swing);
		}
		else {
			isAttacking = false;
			enemiesHit.Clear ();
		}

	}

	// Update is called once per frame
	public virtual void Update () {
		if (isAttacking && isMelee)
			CheckHitEnemy ();

		prevPos = transform.position;

	}

	public void CheckHitEnemy() {

		RaycastHit[] hits = Physics.CapsuleCastAll (capCast1.transform.position, capCast2.transform.position, 
			capCast1.GetComponent<SphereCollider> ().radius, prevPos - transform.position, Vector3.Distance (prevPos, transform.position));

		for (int i = 0; i < hits.Length; i++) {
			RaycastHit hit = hits[i];

			if (hit.transform.GetComponent<Enemy> ()) {
				Enemy enemy = hit.transform.GetComponent<Enemy> ();
				foreach (Enemy e in enemiesHit) {

					if (e == enemy)
						newHit = false;
				}

				if (newHit == true) {
					float finalDamage = CalculateMeleeDamage ();

					hit.transform.GetComponent<Enemy> ().ReceiveDamage (finalDamage, player);
					enemiesHit.Add (enemy);
				}


				newHit = true;
			}
		}

	}

	public float CalculateMeleeDamage() {

		float min = damageMin;
		float max = damageMax;
		float finalDamage;

		int maxDmgLevel = player.skills.maxDmgLevel;
		int minDmgLevel = player.skills.minDmgLevel;
		int attackBuffLevel = player.skills.weaponBuffLevel;

		max = (max + Mathf.Ceil((float)maxDmgLevel/2f));
		min = (min + Mathf.Ceil ((float)minDmgLevel/2f));
		if (min >= max)
			min = max - 1;

		finalDamage = Random.Range (min, max);

		if (attackBuffLevel > 0) {
			finalDamage = finalDamage * (1.1f+ 0.02f * (attackBuffLevel - 1));
		}

		return finalDamage;


	}

	public void UpdateDamage() {

		level = player.Level;

		damageMin = 3f + 1f * (level - 1);
		damageMax = 5f + 2f * (level - 1);

	}
}
