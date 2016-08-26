using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float health;
	public GameObject damageText;

	public float damage;

	//Status Effects
	bool isBurning = false;
	bool isFrozen = false;
	bool isSlowed = false;
	bool isStunned = false;
	bool isConfused = false;

	// Use this for initialization
	void Start () {
		damageText = (GameObject)Resources.Load ("DamageText");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ReceiveDamage(float dmg) {

		health -= dmg;
		GameObject txt = (GameObject)Instantiate (damageText, transform.position, Quaternion.identity);
		txt.GetComponent<TextMesh> ().text = dmg.ToString("F0");
		txt.transform.Rotate (55, 0, 0);


	}

	void OnTriggerEnter(Collider other) {

		if (other.transform.parent.GetComponent<Player> ()) {
			Player player = other.transform.parent.GetComponent<Player> ();
			float dmg = Random.Range (player.damageMin, player.damageMax);
			ReceiveDamage (dmg);

		}
	}
}
