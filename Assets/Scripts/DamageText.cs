using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

	float lifespan = 0.5f, scale = 1f;
	float posOffset = 0f;
	public Transform target;
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
		posOffset += 1f * Time.deltaTime * 40f;

		if (target) {
			Camera camera = FindObjectOfType<Camera>();
			Vector3 screenPos = camera.WorldToScreenPoint(target.position);
			transform.position = screenPos + new Vector3 (0, posOffset, 0);
		}

        transform.localScale = new Vector3(scale, scale, scale);

		if (lifespan < 0) {

			text.color = new Color (text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime*2.5f);

			if (text.color.a < 0.1f)
				Destroy (this.gameObject);
		}
	}
}
