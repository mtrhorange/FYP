using UnityEngine;
using System.Collections;

public class BigSword : MonoBehaviour {

	// Use this for initialization
	public GameObject GroundParticle;
	public float hitfloor;
	public float destroy;
	private bool summononce = false;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		hitfloor -= Time.deltaTime;
		destroy -= Time.deltaTime;
		if (hitfloor <= 0 && !summononce) {
			Instantiate(GroundParticle, this.transform.position , this.transform.rotation);
			summononce = true;
		}
		if (destroy <= 0) {
			Destroy (this.gameObject);
		}
	}
}
