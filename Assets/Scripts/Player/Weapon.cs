using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public enum Type { UNARMED};

	public float damageMin = 10f;
	public float damageMax = 13f;

	GameObject capCast1, capCast2;

	Vector3 prevPos;

	// Use this for initialization
	void Start () {
		capCast1 = transform.Find ("CapCast1");
		capCast2 = transform.Find ("CapCast2");
		prevPos = transform.position;
	}

	public Weapon(float min, float max) {
		damageMin = min;
		damageMax = max;

	}

	// Update is called once per frame
	void Update () {

		CheckHitEnemy ();

		prevPos = transform.position;
	}

	void CheckHitEnemy() {

		RaycastHit[] hits = Physics.CapsuleCastAll (capCast1, capCast2, capCast1.GetComponent<SphereCollider> ().radius, prevPos - transform.position, Vector3.Distance (prevPos, transform.position));

	}
}
