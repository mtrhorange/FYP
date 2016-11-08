using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	Player player;
	public int playerNo = 1;

	// Use this for initialization
	void Start () {

		if (playerNo == 1)
			player = GameManager.instance.player1;
		else {
			if (GameManager.instance.player2 != null)
				player = GameManager.instance.player2;
			else {
				transform.parent.gameObject.SetActive (false);
				return;
			}
		}
		player.healthBar = this;

		SetHealth ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetHealth () {

		GetComponent<RectTransform> ().sizeDelta = new Vector2 (100f * player.Health/player.MaxHealth, 8);

	}
}
