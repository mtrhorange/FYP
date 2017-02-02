using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMenu : MonoBehaviour {

	public int player = 1;

	// Use this for initialization
	void Start () {
		MenuButton[] btns = transform.GetComponentsInChildren<MenuButton> (true);

		foreach (MenuButton btn in btns) {
			btn.player = player;

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
