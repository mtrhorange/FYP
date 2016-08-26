using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public enum Type { UNARMED};

	public float damageMin = 10f;
	public float damageMax = 13f;


	// Use this for initialization
	void Start () {
	
	}

	public Weapon(float min, float max) {
		damageMin = min;
		damageMax = max;

	}

	// Update is called once per frame
	void Update () {
	
	}
}
