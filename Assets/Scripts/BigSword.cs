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
			Destroy(Instantiate(GroundParticle, this.transform.position , this.transform.rotation), 2f);
			summononce = true;
			Camera cam = FindObjectOfType<Camera> ();
			cam.GetComponent<CameraShake> ().ShakeCamera (0.3f, 0.2f);
		}
		if (destroy <= 0) {
			Destroy (this.gameObject);
		}
	}
}
