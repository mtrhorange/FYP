using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillsPanel : MonoBehaviour {

	Text lvlTxt;
	public int player = 1;
	// Use this for initialization
	void Start () {
		lvlTxt = transform.Find("SkillPoints").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player == 1)
			lvlTxt.text = GameManager.instance.player1.SkillPoints.ToString();
		else 
			lvlTxt.text = GameManager.instance.player1.SkillPoints.ToString();
	}
}
