using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NoSTIcon : MonoBehaviour {

	public Transform player;

	float timer = 0.8f;
	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		Camera camera = FindObjectOfType<Camera>();
		Vector3 screenPos = camera.WorldToScreenPoint(player.position);
		transform.position = screenPos + new Vector3 (0, -30, 0);

		timer -= Time.deltaTime;

		Color color = GetComponent<Image> ().color;

		GetComponent<Image> ().color = new Color (color.r, color.g, color.b, timer / 0.5f);

		if (timer < 0)
			Destroy (gameObject);
	}
}
