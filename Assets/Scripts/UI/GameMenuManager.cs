using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour {

	public static GameMenuManager instance;

	public MenuButton selectedBtnP1;

	public GameObject player1Menu;
	public GameObject player2Menu;
	public GameObject player1UI;
	public GameObject player2UI;

    public Slider BGM;
    public Slider SFX;

    bool menuOpen = false;
	public bool canEscape = true;

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
	}

    public void closeMenu()
    {
        if (canEscape)
        {
            player1Menu.SetActive(!player1Menu.activeSelf);
            player1UI.SetActive(!player1UI.activeSelf);
            player2UI.SetActive(!player2UI.activeSelf);
            menuOpen = !menuOpen;

            if (menuOpen)
                Time.timeScale = 0.0001f;
            else
                Time.timeScale = 1f;
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
}
