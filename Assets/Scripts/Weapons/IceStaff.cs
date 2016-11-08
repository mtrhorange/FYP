using UnityEngine;
using System.Collections;

public class IceStaff : Weapon {

	GameObject spellIceBall;


	// Use this for initialization
	public override void Start() {

		base.Start ();

		spellIceBall = (GameObject)Resources.Load ("Skills/Ice_Ball");
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

	public IceStaff(float min, float max):base(min, max) {

	}

	public override void AttackTrigger(int i) {

		if (i == 1)
			CastIceBall ();

	}

	void CastIceBall() {

		GameObject ice = (GameObject)Instantiate (spellIceBall, player.transform.position + player.transform.up * 2f + player.transform.forward, player.transform.localRotation);
		ice.GetComponent<Ice_Ball_Script> ().player = player;

	}
}
