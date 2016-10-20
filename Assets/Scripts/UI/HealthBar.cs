using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	Player player;

	// Use this for initialization
	void Start () {
		player = Player.instance;
		player.healthBar = this;

		SetHealth (player.Health);
		transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (player.MaxHealth + 10f, 15);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetHealth (float h) {

		GetComponent<RectTransform> ().sizeDelta = new Vector2 (h, 8);

	}
}
