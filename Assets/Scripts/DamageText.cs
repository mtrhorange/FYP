using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {

	float lifespan = 1.5f, scale = 1f; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
        scale += Time.deltaTime * 1.5f;
        //Debug.Log(scale);
		transform.position += Vector3.up * Time.deltaTime * 3f;
        transform.localScale = new Vector3(scale, scale, scale);

		if (lifespan < 0)
			Destroy (this.gameObject);
	}
}
