using UnityEngine;
using System.Collections;

public class Staff : Weapon {

	GameObject spellIceBall;

	public enum StaffType {Ice, Fire}

	public StaffType staffType;

	public delegate void SpellBolt ();
	public SpellBolt spellBolt;

	// Use this for initialization
	public override void Start() {

		base.Start ();

		spellIceBall = (GameObject)Resources.Load ("Skills/Ice_Ball");

		if (staffType == StaffType.Ice)
			spellBolt = CastIceBall;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	public Staff(float min, float max):base(min, max) {

	}

	public override void AttackTrigger(int i) {

		if (i == 1)
			spellBolt ();

	}

	void CastIceBall() {

		GameObject ice = (GameObject)Instantiate (spellIceBall, player.transform.position + player.transform.up * 2f + player.transform.forward, player.transform.localRotation);
		ice.GetComponent<Ice_Ball_Script> ().player = player;
	}

}
