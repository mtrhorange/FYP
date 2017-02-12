using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour {

	public int playerNo = 1;
    Player p;

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

    Text PointsTxt;

	GameManager manager;
	// Use this for initialization
	void Start () {
		manager = GameManager.instance;

        if (playerNo == 1) { p = manager.player1; }
        else if (playerNo == 2) { p = manager.player2; }

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

        //Points
        PointsTxt = transform.Find("PointsTxt").GetChild(0).GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		if (this.gameObject.activeInHierarchy) {
			LevelTxt.text = p.Level.ToString();

            //Health
			HealthTxt.text = p.Health.ToString("F0") + "/" + p.MaxHealth.ToString();
			HealthBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (HealthBarMaxWidth * (p.Health / p.MaxHealth), HealthBar.GetComponent<RectTransform> ().sizeDelta.y); 

            //Stamina
            StaminaTxt.text = p.Stamina.ToString("F0") + "/" + p.MaxStamina.ToString();
            StaminaBar.GetComponent<RectTransform>().sizeDelta = new Vector2(StaminaBarMaxWidth * (p.Stamina / p.MaxStamina), StaminaBar.GetComponent<RectTransform>().sizeDelta.y);

            //Exp
            ExpTxt.text = p.Exp.ToString("F0") + "/" + (50 + (50 * p.Level)).ToString("F0");
            ExpBar.GetComponent<RectTransform>().sizeDelta = new Vector2(ExpBarMaxWidth * (p.Exp / (50 + (50 * p.Level))), ExpBar.GetComponent<RectTransform>().sizeDelta.y);

            //Points
            PointsTxt.text = p.Points.ToString();
		}
	}
}
