using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMenu : MonoBehaviour {

	public int player = 1;

	void Awake() {
		MenuButton[] btns = transform.GetComponentsInChildren<MenuButton> (true);

		foreach (MenuButton btn in btns) {
			btn.player = player;
		}



	}

	// Use this for initialization
	void Start () {
		if (!GameMenuManager.instance.menuOpen && gameObject.activeSelf)
			gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
