using UnityEngine;
using System.Collections;

public class BigSwordFrontSlash : MonoBehaviour {

	// Use this for initialization
	public float spawnFront, destroy;
	public GameObject FrontEffect, parentObj , sparks;
	private bool spawned = false;

	void Start () {
		Time.timeScale = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
		spawnFront -= Time.deltaTime;
		destroy -= Time.deltaTime;

		if (spawnFront <= 0 && !spawned) {
			Instantiate(FrontEffect, parentObj.transform.position + ((transform.forward) * 3) , parentObj.transform.rotation);
			Instantiate(sparks, parentObj.transform.position + ((transform.forward) * 3) , parentObj.transform.rotation);
			Instantiate(sparks, parentObj.transform.position + ((transform.forward) * 1) , parentObj.transform.rotation);
			spawned = true;
		}
		if (destroy <= 0) {
			Destroy (this.gameObject);
		}

	}
}
