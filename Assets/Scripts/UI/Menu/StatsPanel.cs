using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour {

	public int playerNo = 1;

	Text LevelTxt;

	Image HealthBar;
	float HealthBarMaxWidth;
	Text HealthTxt;

	Image StaminaBar;
	float StaminaBarMaxWidth;
	Text StaminaTxt;

	Image ExpBar;
	float ExpBarMaxWidth;
	Text ExpTxt;

	Text LivesTxt;

	GameManager manager;
	// Use this for initialization
	void Start () {
		manager = GameManager.instance;
		LevelTxt = transform.Find ("LevelTxt").GetChild (0).GetComponent<Text> ();

        //Health
		HealthTxt = transform.Find ("HealthTxt").Find ("Health").GetComponent<Text>();
		HealthBar = transform.Find ("HealthTxt").Find ("Bar").Find ("InnerBar").GetComponent<Image> ();
		HealthBarMaxWidth = HealthBar.GetComponent<RectTransform> ().sizeDelta.x;

        //Stamina
        StaminaTxt = transform.Find("StaminaTxt").Find("Stamina").GetComponent<Text>();
        StaminaBar = transform.Find("StaminaTxt").Find("Bar").Find("InnerBar").GetComponent<Image>();
        StaminaBarMaxWidth = StaminaBar.GetComponent<RectTransform>().sizeDelta.x;

        //Exp
        ExpTxt = transform.Find("ExpTxt").Find("Exp").GetComponent<Text>();
        ExpBar = transform.Find("ExpTxt").Find("Bar").Find("InnerBar").GetComponent<Image>();
        ExpBarMaxWidth = ExpBar.GetComponent<RectTransform>().sizeDelta.x;
	}

	// Update is called once per frame
	void Update () {
		if (this.gameObject.activeInHierarchy) {
			LevelTxt.text = manager.player1.Level.ToString();

            //Health
			HealthTxt.text = manager.player1.Health.ToString("F0") + "/" + manager.player1.MaxHealth.ToString();
			HealthBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (HealthBarMaxWidth * (manager.player1.Health / manager.player1.MaxHealth), HealthBar.GetComponent<RectTransform> ().sizeDelta.y); 

            //Stamina
            StaminaTxt.text = manager.player1.Stamina.ToString("F0") + "/" + manager.player1.MaxStamina.ToString();
            StaminaBar.GetComponent<RectTransform>().sizeDelta = new Vector2(StaminaBarMaxWidth * (manager.player1.Stamina / manager.player1.MaxStamina), StaminaBar.GetComponent<RectTransform>().sizeDelta.y);

            //Exp
            ExpTxt.text = manager.player1.Exp.ToString("F0") + "/" + (50 + (50 * manager.player1.Level)).ToString("F0");
            ExpBar.GetComponent<RectTransform>().sizeDelta = new Vector2(ExpBarMaxWidth * (manager.player1.Exp / (50 + (50 * manager.player1.Level))), ExpBar.GetComponent<RectTransform>().sizeDelta.y);
		}
	}
}
