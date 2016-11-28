using UnityEngine;
using System.Collections;

public class GameMenuManager : MonoBehaviour {

	public static GameMenuManager instance;

	public MenuButton selectedBtn;

	void Awake() {

		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		CheckInput ();
	}

	public void CheckInput() {
		if (Input.GetButtonDown ("AttackL"))
			selectedBtn.Submit ();
		if (Input.GetButtonDown ("AttackR"))
			selectedBtn.Cancel ();
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			selectedBtn.MoveLeft ();
		if (Input.GetKeyDown (KeyCode.RightArrow))
			selectedBtn.MoveRight ();
		if (Input.GetKeyDown (KeyCode.UpArrow))
			selectedBtn.MoveUp ();
		if (Input.GetKeyDown (KeyCode.DownArrow))
			selectedBtn.MoveDown ();
	}
}
