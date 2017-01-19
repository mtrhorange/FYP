using UnityEngine;
using System.Collections;

public class StaminaBar : MonoBehaviour {

	public Player player;
	public int playerNo = 1;

	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetPlayer(Player p) {

		player = p;
		player.staminaBar = this;
		//SetStamina ();

	}

	public void SetStamina() {
		GetComponent<RectTransform> ().sizeDelta = new Vector2 (player.Stamina, 8);

	}

	public void SetMaxStamina() {
		transform.parent.GetComponent<RectTransform> ().sizeDelta = new Vector2 (player.MaxStamina + 10f, 15);
		SetStamina ();


	}
}
