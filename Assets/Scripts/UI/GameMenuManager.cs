using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour {

	public static GameMenuManager instance;

	public MenuButton selectedBtnP1;
	public MenuButton selectedBtnP2;

	public GameObject player1Menu;
	public GameObject player2Menu;
	public GameObject player1UI;
	public GameObject player2UI;

    public Slider BGM;
    public Slider SFX;

    bool menuOpen = false;
	public bool canEscape = true;

	bool p2CanLeft = true;
	bool p2CanUp = true;
	bool p2CanRight = true;
	bool p2CanDown = true;
	bool p2CanMove = true;


	void Awake() {

		instance = this;
	}

	// Use this for initialization
	void Start () {
        if (PlayerPrefs.HasKey("BGM"))
        {
            BGM.value = PlayerPrefs.GetFloat("BGM");
        }

        if (PlayerPrefs.HasKey("SFX"))
        {
            SFX.value = PlayerPrefs.GetFloat("SFX");
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (menuOpen)
			CheckInput ();

		if (Input.GetKeyDown (KeyCode.Escape)) {

            closeMenu();
            
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


		if (p2CanMove && DPadRight ()) 
			selectedBtnP2.MoveRight ();
		if (p2CanMove && DPadLeft ())
			selectedBtnP2.MoveLeft ();
		if (p2CanMove && DPadUp ())
			selectedBtnP2.MoveUp ();
		if (p2CanMove && DPadDown ())
			selectedBtnP2.MoveDown ();
		if (Input.GetButtonDown ("AButtonCtrl1"))
			selectedBtnP2.Submit ();
		if (Input.GetButtonDown ("BButtonCtrl1"))
			selectedBtnP2.Cancel ();
		

		p2CanMove = true;
		if (DPadLeft() && p2CanMove)
			p2CanMove = false;
		if (DPadRight () && p2CanMove)
			p2CanMove = false;
		if (DPadDown () && p2CanMove)
			p2CanMove = false;
		if (DPadUp () && p2CanMove)
			p2CanMove = false;





		
	}

    public void closeMenu()
    {
        if (canEscape)
        {
            player1Menu.SetActive(!player1Menu.activeSelf);
            player1UI.SetActive(!player1UI.activeSelf);
			if (GameManager.instance.twoPlayers) {
				player2Menu.SetActive (!player2Menu.activeSelf);
				player2UI.SetActive (!player2UI.activeSelf);
			}
            menuOpen = !menuOpen;

			if (menuOpen) {
				Time.timeScale = 0.0001f;

					
			} else {
				Time.timeScale = 1f;
			}

			GameManager.instance.player1.controller.canMove = !GameManager.instance.player1.controller.canMove;
			GameManager.instance.player1.controller.canAction = !GameManager.instance.player1.controller.canAction;

			if (GameManager.instance.twoPlayers) {
				GameManager.instance.player2.controller.canMove = !GameManager.instance.player2.controller.canMove;
				GameManager.instance.player2.controller.canAction = !GameManager.instance.player2.controller.canAction;
			}
				
        }
        else {
            selectedBtnP1.Cancel();
        }
    }

    public void setBGM()
    {
        BGMManager.instance.setBGM(BGM.value);
        PlayerPrefs.SetFloat("BGM", BGM.value);
    }

    public void setSFX()
    {
        SFXManager.instance.setSFX(SFX.value);
        PlayerPrefs.SetFloat("SFX", SFX.value);
    }

	bool DPadUp()
	{
		float yAxis = Input.GetAxis("DPadYCtrl1");
		if (yAxis == 1) {
			return true;
		} else {
			return false;
		}
	}

	bool DPadDown()
	{
		float yAxis = Input.GetAxis("DPadYCtrl1");
		if (yAxis == -1) {
			return true;
		} else {
			return false;
		}
	}

	bool DPadRight()
	{
		float xAxis = Input.GetAxis("DPadXCtrl1");
		if (xAxis == 1) {
			return true;
		} else {
			return false;
		}
	}

	bool DPadLeft()
	{
		float xAxis = Input.GetAxis("DPadXCtrl1");

		if (xAxis == -1)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
