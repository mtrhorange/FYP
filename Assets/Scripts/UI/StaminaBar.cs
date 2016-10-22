using UnityEngine;
using System.Collections;

public class StaminaBar : MonoBehaviour {

	Player player;

	// Use this for initialization
	void Start () {
		player = Player.instance;
		player.staminaBar = this;

		SetMaxStamina ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetStamina() {
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (player.Stamina, 8);

	}

	public void SetMaxStamina() {
		transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (player.MaxStamina + 10f, 15);
		SetStamina ();


	}
}
