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
	int StaminaBarMaxWidth;
	Text StaminaTxt;

	Image ExpBar;
	int ExpBarMaxWidth;
	Text ExpTxt;

	Text LivesTxt;

	GameManager manager;
	// Use this for initialization
	void Start () {
		manager = GameManager.instance;
		LevelTxt = transform.Find ("LevelTxt").GetChild (0).GetComponent<Text> ();

		HealthTxt = transform.Find ("HealthTxt").Find ("Health").GetComponent<Text>();
		HealthBar = transform.Find ("HealthTxt").Find ("Bar").Find ("InnerBar").GetComponent<Image> ();
		HealthBarMaxWidth = HealthBar.GetComponent<RectTransform> ().sizeDelta.x;
	}

	// Update is called once per frame
	void Update () {
		if (this.gameObject.activeInHierarchy) {
			LevelTxt.text = manager.player1.Level.ToString();

			HealthTxt.text = manager.player1.Health.ToString() + "/" + manager.player1.MaxHealth.ToString();

			HealthBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (HealthBarMaxWidth * (manager.player1.Health / manager.player1.MaxHealth), HealthBar.GetComponent<RectTransform> ().sizeDelta.y); 
		
		
		}
	}
}
