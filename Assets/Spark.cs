using UnityEngine;
using System.Collections;

public class Spark : MonoBehaviour {

	// Use this for initialization
	private float lifeSpan = 3;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifeSpan -= Time.deltaTime;
		if (lifeSpan <= 0) {
			Destroy (this.gameObject);
		}
	}
}
