using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceSpikeSpell : Spell {

	public List<Enemy> enemiesHit;

	// Use this for initialization
	void Start () {
		enemiesHit = new List<Enemy> ();
		CastIceSpike ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EnemyHit(Enemy enemy) {

		enemiesHit.Add (enemy);
		GetDamage ();
		enemy.ReceiveDamage (damage, player);
	}

	public void CastIceSpike() {

		StartCoroutine (_IceSpike());
		StopCoroutine (_IceSpike ());
	}

	IEnumerator _IceSpike()
	{
		GameObject iceSpike = (GameObject)Resources.Load ("Skills/IceSpike");

		Vector3 pos = transform.position;
		Vector3 forward = transform.forward;
		GameObject ice;

		ice = (GameObject)Instantiate(iceSpike, pos + forward * 2, transform.rotation * Quaternion.Euler(0, 90, 0));
		ice.transform.SetParent (transform);
		ice.GetComponent<Spell> ().player = player;
		yield return new WaitForSeconds(0.05f);
		ice = (GameObject)Instantiate(iceSpike, pos + forward * 3, transform.rotation * Quaternion.Euler(0, 300, 0));
		ice.transform.SetParent (transform);
		ice.GetComponent<Spell> ().player = player;
		yield return new WaitForSeconds(0.05f);
		ice = (GameObject)Instantiate(iceSpike, pos + forward * 4, transform.rotation * Quaternion.Euler(0, 126, 0));
		ice.transform.SetParent (transform);
		ice.GetComponent<Spell> ().player = player;
		yield return new WaitForSeconds(0.05f);
		ice = (GameObject)Instantiate(iceSpike, pos + forward * 5, transform.rotation * Quaternion.Euler(0, 0, 0));
		ice.transform.SetParent (transform);
		ice.GetComponent<Spell> ().player = player;
		yield return new WaitForSeconds(0.05f);
		ice = (GameObject)Instantiate(iceSpike, pos + forward * 6, transform.rotation * Quaternion.Euler(0, 40, 0));
		ice.transform.SetParent (transform);
		ice.GetComponent<Spell> ().player = player;
		yield return new WaitForSeconds(0.05f);
		ice = (GameObject)Instantiate(iceSpike, pos + forward * 7, transform.rotation * Quaternion.Euler(0, 185, 0));
		ice.transform.SetParent (transform);
		ice.GetComponent<Spell> ().player = player;
		yield return new WaitForSeconds (3f);
		Destroy (this.gameObject);
	}
}
