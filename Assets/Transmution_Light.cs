using UnityEngine;
using System.Collections;

public class Transmution_Light : MonoBehaviour {


	// Use this for initialization
	public float Lifespan;
	public GameObject Sword;
	private bool SpawnedOnce = false;
	void Start () {

	}

	// Update is called once per frame
	void Update() {
		Lifespan -= Time.deltaTime;
		if (Lifespan <= 0) {
			Destroy(this.gameObject);
		}
		if (!SpawnedOnce){
			Instantiate(Sword, this.transform.position , this.transform.rotation);
			SpawnedOnce = true;
		}
	}
}