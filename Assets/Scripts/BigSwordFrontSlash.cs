using UnityEngine;
using System.Collections;

public class BigSwordFrontSlash : MonoBehaviour {

	// Use this for initialization
	public float spawnFront, destroy;
	public GameObject FrontEffect, player , sparks;
	private bool spawned = false;

	void Start () {
	}

	// Update is called once per frame
	void Update () {
		spawnFront -= Time.deltaTime;
		destroy -= Time.deltaTime;

		if (spawnFront <= 0 && !spawned) {
			Instantiate(FrontEffect, player.transform.position + ((player.transform.forward) * 3) , player.transform.rotation);
			Instantiate(sparks, player.transform.position + ((player.transform.forward) * 3) , player.transform.rotation);
			Instantiate(sparks, player.transform.position + ((player.transform.forward) * 1) , player.transform.rotation);
			spawned = true;
		}
		if (destroy <= 0) {
			Destroy (this.gameObject);
		}

	}
}
