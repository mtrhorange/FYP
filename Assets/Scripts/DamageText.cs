using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

	float lifespan = 0.5f, scale = 1f; 
	//TextMesh text;
	Text text;
	// Use this for initialization
	void Start () {
		//text = GetComponent<TextMesh> ();
		text = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
        //scale += Time.deltaTime * 0.5f;
        //Debug.Log(scale);
		transform.position += Vector3.up * Time.deltaTime * 20f;
        transform.localScale = new Vector3(scale, scale, scale);

		if (lifespan < 0) {

			text.color = new Color (text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime*2.5f);

			if (text.color.a < 0.1f)
				Destroy (this.gameObject);
		}
	}
}
