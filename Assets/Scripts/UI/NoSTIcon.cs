using UnityEngine;
using System.Collections;

public class NoSTIcon : MonoBehaviour {

	public Transform player;

	float timer = 2f;
	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		Camera camera = FindObjectOfType<Camera>();
		Vector3 screenPos = camera.WorldToScreenPoint(player.position);
		transform.position = screenPos + new Vector3 (0, -20, 0);

		timer -= Time.deltaTime;
		if (timer < 0)
			Destroy (gameObject);
	}
}
