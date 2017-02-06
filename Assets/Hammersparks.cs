using UnityEngine;
using System.Collections;

public class Hammersparks : MonoBehaviour {

	// Use this for initialization
	public float spawnSparks, spawnSparks2;
	public GameObject Sparks, spawnPoint;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		spawnSparks -= Time.deltaTime;
		if (spawnSparks <= 0) {
			Instantiate (Sparks, spawnPoint.transform.position, spawnPoint.transform.rotation);
			spawnSparks = spawnSparks2;
		}

	}
}
