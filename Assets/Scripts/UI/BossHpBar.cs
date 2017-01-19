using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHpBar : MonoBehaviour {

    public Text hpText;
    public Image bar;
    public GameObject boss;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //update ui hp bar
    public void UpdateHPBar()
    {
        //text
        hpText.text = boss.GetComponent<Enemy>().health.ToString("F0") + " / " + boss.GetComponent<Enemy>().maxHealth.ToString("F0");
        //bar
        bar.fillAmount = boss.GetComponent<Enemy>().health / boss.GetComponent<Enemy>().maxHealth;
    }
}
