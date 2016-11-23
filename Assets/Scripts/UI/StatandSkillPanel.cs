using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatandSkillPanel : MonoBehaviour {

    // Use this for initialization
    public GameObject SkillTab;
    public GameObject StatTab;
	public GameObject SettingTab;
	public GameObject ActiveSkillTab;
	public GameObject PassiveSkillTab;
	public GameObject ActiveSkillDesc;
	public GameObject PassiveSkillDesc;
	public Text ActiveSkillText;
	public Text PassiveSkillText;
	public GameObject Skb;
	public GameObject Stb;
	public GameObject Seb;
	public GameObject Asb;
	public GameObject Psb;
	public GameObject As1b;
	public GameObject As2b;
	public GameObject As3b;
	public GameObject As4b;
	public GameObject Ps1b;
	public GameObject Ps2b;
	public GameObject Ps3b;
	public GameObject Ps4b;
	public GameObject Image1;
	public GameObject Image2; 
	public GameObject Image3;
	public GameObject Image4;
	public GameObject Image5; 
	public GameObject Image6;

	private Button SkillBtn;
	private Button StatBtn;
	private Button SettingBtn;
	private Button ActiveSkillBtn;
	private Button PassiveSkillBtn;
	private Button Askill1Btn;
	private Button Askill2Btn;
	private Button Askill3Btn;
	private Button Askill4Btn;
	private Button Pskill1Btn;
	private Button Pskill2Btn;
	private Button Pskill3Btn;
	private Button Pskill4Btn;
	private ColorBlock SKCB;
	private ColorBlock STCB;
	private ColorBlock SECB;
	private ColorBlock ASCB;
	private ColorBlock PSCB;
	private ColorBlock AS1CB;
	private ColorBlock AS2CB;
	private ColorBlock AS3CB;
	private ColorBlock AS4CB;
	private ColorBlock PS1CB;
	private ColorBlock PS2CB;
	private ColorBlock PS3CB;
	private ColorBlock PS4CB;



	private Color selectedColor;
	private int curTab = 0;
	private int inTabNo = 0;
	private int curRow = 0;
	private int curCol = 0;
	private bool inTab = false;
	private bool inSkillsTab = false;

	// 0 = stats, 1 = skills , 2 = settings
	void Start () {
		SettingBtn = Seb.GetComponent<Button> ();
		SkillBtn = Skb.GetComponent<Button>(); 
		StatBtn = Stb.GetComponent<Button> ();
		ActiveSkillBtn = Asb.GetComponent<Button> ();
		PassiveSkillBtn = Psb.GetComponent<Button> ();
		Askill1Btn = As1b.GetComponent<Button> ();
		Askill2Btn = As2b.GetComponent<Button> ();
		Askill3Btn = As3b.GetComponent<Button> ();
		Askill4Btn = As4b.GetComponent<Button> ();
		Pskill1Btn = Ps1b.GetComponent<Button> ();
		Pskill2Btn = Ps2b.GetComponent<Button> ();
		Pskill3Btn = Ps3b.GetComponent<Button> ();
		Pskill4Btn = Ps4b.GetComponent<Button> ();
		SKCB = SkillBtn.colors;
		STCB = StatBtn.colors;	
		SECB = SettingBtn.colors;
		ASCB = ActiveSkillBtn.colors;
		PSCB = PassiveSkillBtn.colors;
		AS1CB = Askill1Btn.colors;
		AS2CB = Askill2Btn.colors;
		AS3CB = Askill3Btn.colors;
		AS4CB = Askill4Btn.colors;
		PS1CB = Pskill1Btn.colors;
		PS2CB = Pskill2Btn.colors;
		PS3CB = Pskill3Btn.colors;
		PS4CB = Pskill4Btn.colors;


		STCB.normalColor = Color.red;
		StatBtn.colors = STCB;
		StatsTab ();

	}
	
	// Update is called once per frame
	void Update () {


		if (curTab == 1 && !inTab && Input.GetKeyDown(KeyCode.RightArrow)) {
			SettingsTab ();
		}
		if (curTab == 0 &&  !inTab && Input.GetKeyDown (KeyCode.RightArrow)) {
			SkillsTab ();
		}
		if (curTab == 1 && !inTab && Input.GetKeyDown (KeyCode.LeftArrow)) {
			StatsTab ();
		}
		if (curTab == 2 && !inTab && Input.GetKeyDown (KeyCode.LeftArrow)) {
			SkillsTab ();
		}
		if (curTab == 1 && inTab && !inSkillsTab && Input.GetKeyDown (KeyCode.RightArrow)) {
			inTabNo = 1;
			Passiveskills ();
		}
		if (curTab == 1 && !inTab && !inSkillsTab && Input.GetKeyDown (KeyCode.DownArrow)) {
			inTab = true;
			Activeskills ();
		}

		if (curTab == 1 && inTab && !inSkillsTab && Input.GetKeyDown (KeyCode.UpArrow)) {
			inTab = false;
			SkillsTab ();
			//Activeskills ();
		}

		if (curTab == 1 && inTab && inTabNo == 1 && !inSkillsTab && Input.GetKeyDown (KeyCode.LeftArrow)) {
			Activeskills ();
			inTabNo = 0;
		}
		if (inTab && inTabNo == 0 && Input.GetButtonDown ("AttackL")) 
		{
			inSkillsTab = true;
			ActiveSkillsPanel ();

		}
		if (inTab && inTabNo == 1 && Input.GetButtonDown ("AttackL")) 
		{
			inSkillsTab = true;
			PassiveSkillsPanel ();

		}


		if (inTabNo == 0 && inSkillsTab && Input.GetButtonDown ("AttackR")) 
		{
			inSkillsTab = false;
			Activeskills ();
		}

		if (inTabNo == 1 && inSkillsTab && Input.GetButtonDown ("AttackR")) 
		{
			inSkillsTab = false;
			Passiveskills ();
		}

		if (inSkillsTab) 
		{
			if (curRow < 1 && Input.GetKeyDown (KeyCode.DownArrow)) 
			{
				curRow++;
			}

			if (curRow > 0 && Input.GetKeyDown (KeyCode.UpArrow)) 
			{
				curRow--;
			}

			if (curCol < 1 && Input.GetKeyDown (KeyCode.RightArrow)) 
			{
				curCol++;
			}

			if (curCol > 0 && Input.GetKeyDown (KeyCode.LeftArrow)) 
			{
				curCol--;
			}


			if (curRow == 0 && curCol == 0) {
				ActiveSkill1 ();
			}

			if (curRow == 0 && curCol == 1) {
				ActiveSkill2 ();
			}

			if (curRow == 1 && curCol == 0) {
				ActiveSkill3 ();
			}
			if (curRow == 1 && curCol == 1) {
				ActiveSkill4 ();
			}
		}

		if (inSkillsTab) 
		{
			if (curRow < 1 && Input.GetKeyDown (KeyCode.DownArrow)) 
			{
				curRow++;
			}

			if (curRow > 0 && Input.GetKeyDown (KeyCode.UpArrow)) 
			{
				curRow--;
			}

			if (curCol < 1 && Input.GetKeyDown (KeyCode.RightArrow)) 
			{
				curCol++;
			}

			if (curCol > 0 && Input.GetKeyDown (KeyCode.LeftArrow)) 
			{
				curCol--;
			}


			if (curRow == 0 && curCol == 0) {
				PassiveSkill1 ();
			}

			if (curRow == 0 && curCol == 1) {
				PassiveSkill2 ();
			}

			if (curRow == 1 && curCol == 0) {
				PassiveSkill3 ();
			}
			if (curRow == 1 && curCol == 1) {
				PassiveSkill4 ();
			}
		}
			
	}



    public void StatsTab()
    {
		StatTab.SetActive (true);
		SkillTab.SetActive (false);
		SettingTab.SetActive (false);
		curTab = 0;
		SKCB.normalColor = Color.white;
		SkillBtn.colors = SKCB;
		STCB.normalColor = Color.red;
		StatBtn.colors = STCB;
		SECB.normalColor = Color.white;
		SettingBtn.colors = SECB;
		Image1.SetActive(true);
		Image2.SetActive(false);
		Image3.SetActive(false);



    }
	public void SkillsTab() {

		SkillTab.SetActive (true);
		StatTab.SetActive (false);
		SettingTab.SetActive (false);
		curTab = 1;
		SKCB.normalColor = Color.red;
		SkillBtn.colors = SKCB;
		STCB.normalColor = Color.white;
		StatBtn.colors = STCB;
		SECB.normalColor = Color.white;
		SettingBtn.colors = SECB;
		ASCB.normalColor = Color.white;
		ActiveSkillBtn.colors = ASCB;
		PSCB.normalColor = Color.white;
		PassiveSkillBtn.colors = PSCB;
		Image1.SetActive(false);
		Image2.SetActive(true);
		Image3.SetActive(false);
		Image4.SetActive (false);
		Image5.SetActive (false);

	}
	public void SettingsTab()
	{
		StatTab.SetActive (false);
		SkillTab.SetActive (false);
		SettingTab.SetActive (true);
		curTab = 2;
		SKCB.normalColor = Color.white;
		SkillBtn.colors = SKCB;
		STCB.normalColor = Color.white;
		StatBtn.colors = STCB;
		SECB.normalColor = Color.red;
		SettingBtn.colors = SECB;
		Image1.SetActive(false);
		Image2.SetActive(false);
		Image3.SetActive(true);


	}

	public void Activeskills()
	{
		PassiveSkillDesc.SetActive (false);
		ActiveSkillDesc.SetActive (false);
		PassiveSkillTab.SetActive (false);
		ActiveSkillTab.SetActive (true);
		SKCB.normalColor = Color.white;
		SkillBtn.colors = SKCB;
		STCB.normalColor = Color.white;
		StatBtn.colors = STCB;
		SECB.normalColor = Color.white;
		SettingBtn.colors = SECB;
		ASCB.normalColor = Color.black;
		ActiveSkillBtn.colors = ASCB;
		PSCB.normalColor = Color.white;
		PassiveSkillBtn.colors = PSCB;
		AS1CB.normalColor = Color.white;
		Askill1Btn.colors = AS1CB;
		AS2CB.normalColor = Color.white;
		Askill2Btn.colors = AS2CB;
		AS3CB.normalColor = Color.white;
		Askill3Btn.colors = AS3CB;
		AS4CB.normalColor = Color.white;
		Askill4Btn.colors = AS4CB;
		curRow = 0;
		curCol = 0;
		ActiveSkillTab.GetComponent<Image> ().color =new Color(1,1,1, 0.25f);
		Image1.SetActive(false);
		Image2.SetActive(false);
		Image3.SetActive(false);
		Image4.SetActive (true);
		Image5.SetActive (false);
	}

	public void Passiveskills()
	{
		PassiveSkillDesc.SetActive (false);
		ActiveSkillDesc.SetActive (false);
		PassiveSkillTab.SetActive (true);
		ActiveSkillTab.SetActive (false);
		SKCB.normalColor = Color.white;
		SkillBtn.colors = SKCB;
		STCB.normalColor = Color.white;
		StatBtn.colors = STCB;
		SECB.normalColor = Color.white;
		SettingBtn.colors = SECB;
		ASCB.normalColor = Color.white;
		ActiveSkillBtn.colors = ASCB;
		PSCB.normalColor = Color.black;
		PassiveSkillBtn.colors = PSCB;
		PS1CB.normalColor = Color.white;
		Pskill1Btn.colors = PS1CB;
		PS2CB.normalColor = Color.white;
		Pskill2Btn.colors = PS2CB;
		PS3CB.normalColor = Color.white;
		Pskill3Btn.colors = PS3CB;
		PS4CB.normalColor = Color.white;
		Pskill4Btn.colors = PS4CB;
		PassiveSkillTab.GetComponent<Image>().color = new Color(1,1,1, 0.25f);
		Image4.SetActive (false);
		Image5.SetActive (true);

	}

	public void ActiveSkillsPanel()
	{
		ActiveSkillTab.SetActive (true);
		ActiveSkillDesc.SetActive (true);
		ActiveSkillTab.GetComponent<Image> ().color =new Color(1,1,1, 1.0f);
	}
	public void PassiveSkillsPanel()
	{
		PassiveSkillTab.SetActive (true);
		PassiveSkillDesc.SetActive (true);
		PassiveSkillTab.GetComponent<Image>().color = new Color(1,1,1, 1.0f);
	}

	public void ActiveSkill1()
	{
		AS1CB.normalColor = Color.magenta;
		Askill1Btn.colors = AS1CB;
		AS2CB.normalColor = Color.white;
		Askill2Btn.colors = AS2CB;
		AS3CB.normalColor = Color.white;
		Askill3Btn.colors = AS3CB;
		AS4CB.normalColor = Color.white;
		Askill4Btn.colors = AS4CB;
		ActiveSkillText.text = "This is Active skill 1";
	}

	public void ActiveSkill2()
	{
		AS1CB.normalColor = Color.white;
		Askill1Btn.colors = AS1CB;
		AS2CB.normalColor = Color.magenta;
		Askill2Btn.colors = AS2CB;
		AS3CB.normalColor = Color.white;
		Askill3Btn.colors = AS3CB;
		AS4CB.normalColor = Color.white;
		Askill4Btn.colors = AS4CB;
		ActiveSkillText.text = "This is Active skill 2";

	}


	public void ActiveSkill3()
	{
		AS1CB.normalColor = Color.white;
		Askill1Btn.colors = AS1CB;
		AS2CB.normalColor = Color.white;
		Askill2Btn.colors = AS2CB;
		AS3CB.normalColor = Color.magenta;
		Askill3Btn.colors = AS3CB;
		AS4CB.normalColor = Color.white;
		Askill4Btn.colors = AS4CB;
		ActiveSkillText.text = "This is Active skill 3";
	}


	public void ActiveSkill4()
	{
		AS1CB.normalColor = Color.white;
		Askill1Btn.colors = AS1CB;
		AS2CB.normalColor = Color.white;
		Askill2Btn.colors = AS2CB;
		AS3CB.normalColor = Color.white;
		Askill3Btn.colors = AS3CB;
		AS4CB.normalColor = Color.magenta;
		Askill4Btn.colors = AS4CB;
		ActiveSkillText.text = "This is Active skill 4";
	}

	public void PassiveSkill1()
	{
		PS1CB.normalColor = Color.magenta;
		Pskill1Btn.colors = PS1CB;
		PS2CB.normalColor = Color.white;
		Pskill2Btn.colors = PS2CB;
		PS3CB.normalColor = Color.white;
		Pskill3Btn.colors = PS3CB;
		PS4CB.normalColor = Color.white;
		Pskill4Btn.colors = PS4CB;
		PassiveSkillText.text = "This is Passive skill 1";
	}

	public void PassiveSkill2()
	{
		PS1CB.normalColor = Color.white;
		Pskill1Btn.colors = PS1CB;
		PS2CB.normalColor = Color.magenta;
		Pskill2Btn.colors = PS2CB;
		PS3CB.normalColor = Color.white;
		Pskill3Btn.colors = PS3CB;
		PS4CB.normalColor = Color.white;
		Pskill4Btn.colors = PS4CB;
		PassiveSkillText.text = "This is Passive skill 2";
	}


	public void PassiveSkill3()
	{
		PS1CB.normalColor = Color.white;
		Pskill1Btn.colors = PS1CB;
		PS2CB.normalColor = Color.white;
		Pskill2Btn.colors = PS2CB;
		PS3CB.normalColor = Color.magenta;
		Pskill3Btn.colors = PS3CB;
		PS4CB.normalColor = Color.white;
		Pskill4Btn.colors = PS4CB;
		PassiveSkillText.text = "This is Passive skill 3";
	}


	public void PassiveSkill4()
	{
		PS1CB.normalColor = Color.white;
		Pskill1Btn.colors = PS1CB;
		PS2CB.normalColor = Color.white;
		Pskill2Btn.colors = PS2CB;
		PS3CB.normalColor = Color.white;
		Pskill3Btn.colors = PS3CB;
		PS4CB.normalColor = Color.magenta;
		Pskill4Btn.colors = PS4CB;
		PassiveSkillText.text = "This is Passive skill 4";
	}
}
