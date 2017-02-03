using UnityEngine;
using System.Collections;

public class GroundSmash : Spell {

	// Use this for initialization
	public float lifespan;
	public GameObject Sword;
	private bool SpawnedOnce = false;

	void Start () {

	}

	// Update is called once per frame
	void Update() {
		lifespan -= Time.deltaTime;
		if (lifespan <= 0) {
			Destroy(this.gameObject);
		}
		if (!SpawnedOnce){
			Instantiate(Sword, this.transform.position , this.transform.rotation);
			SpawnedOnce = true;
		}
	}
}
