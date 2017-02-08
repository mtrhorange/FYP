using UnityEngine;
using System.Collections;

public class Lifespan : MonoBehaviour {

	// Use this for initialization
	public float lifespan;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
		if (lifespan <= 0) {
			Destroy (this.gameObject);
		}
		
	}
}
