using UnityEngine;
using System.Collections;

public class GameMenuManager : MonoBehaviour {

	public static GameMenuManager instance;

	public MenuButton selectedBtnP1;

	public GameObject player1Menu;
	public GameObject player2Menu;
	public GameObject player1UI;
	public GameObject player2UI;

	bool menuOpen = false;
	public bool canEscape = true;

	void Awake() {

		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (menuOpen)
			CheckInput ();

		if (Input.GetKeyDown (KeyCode.Escape)) {

			if (canEscape) {
				player1Menu.SetActive (!player1Menu.activeSelf);
				player1UI.SetActive (!player1UI.activeSelf);
				player2UI.SetActive (!player2UI.activeSelf);
				menuOpen = !menuOpen;

				if (menuOpen)
					Time.timeScale = 0.0001f;
				else
					Time.timeScale = 1f;
			} else {

				selectedBtnP1.Cancel ();

			}
		}
	}

	public void CheckInput() {
		if (Input.GetButtonDown ("AttackL"))
			selectedBtnP1.Submit ();
		if (Input.GetButtonDown ("AttackR"))
			selectedBtnP1.Cancel ();
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			selectedBtnP1.MoveLeft ();
		if (Input.GetKeyDown (KeyCode.RightArrow))
			selectedBtnP1.MoveRight ();
		if (Input.GetKeyDown (KeyCode.UpArrow))
			selectedBtnP1.MoveUp ();
		if (Input.GetKeyDown (KeyCode.DownArrow))
			selectedBtnP1.MoveDown ();
	}
}
