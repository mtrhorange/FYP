using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class KeyAssignPanel : MenuButton {

	public Skills assignSkill;

	public Image firePillar, iceSpikes, chainLightning, drainHeal, aoeLightning, groundSmash, verticalStrike, spearBreaker, fireBall;

	public GameObject skill1, skill2, skill3, skill4, skill5;

	public override void Awake () {
		
		base.Awake ();
		if (player == 1) {
			switch (GameManager.instance.player1.skillCType) {
			case Skills.ChainLightning:
				ChangeKeyImage ("C", Skills.ChainLightning);
				break;
			case Skills.FirePillar:
				ChangeKeyImage ("C", Skills.FirePillar);
				break;
			case Skills.IceSpike:
				ChangeKeyImage ("C", Skills.IceSpike);
				break;
			case Skills.DrainHeal:
				ChangeKeyImage ("C", Skills.DrainHeal);
				break;
			case Skills.AoeLightning:
				ChangeKeyImage ("C", Skills.AoeLightning);
				break;
			case Skills.GroundSmash:
				ChangeKeyImage ("C", Skills.GroundSmash);
				break;
			case Skills.VerticalStrike:
				ChangeKeyImage ("C", Skills.VerticalStrike);
				break;
			case Skills.SpearBreaker:
				ChangeKeyImage ("C", Skills.SpearBreaker);
				break;

			}

			switch (GameManager.instance.player1.skillVType) {
			case Skills.ChainLightning:
				ChangeKeyImage ("V", Skills.ChainLightning);
				break;
			case Skills.FirePillar:
				ChangeKeyImage ("V", Skills.FirePillar);
				break;
			case Skills.IceSpike:
				ChangeKeyImage ("V", Skills.IceSpike);
				break;
			case Skills.DrainHeal:
				ChangeKeyImage ("V", Skills.DrainHeal);
				break;
			case Skills.AoeLightning:
				ChangeKeyImage ("V", Skills.AoeLightning);
				break;
			case Skills.GroundSmash:
				ChangeKeyImage ("V", Skills.GroundSmash);
				break;
			case Skills.VerticalStrike:
				ChangeKeyImage ("V", Skills.VerticalStrike);
				break;
			case Skills.SpearBreaker:
				ChangeKeyImage ("V", Skills.SpearBreaker);
				break;
			}

			switch (GameManager.instance.player1.skillAType) {
			case Skills.ChainLightning:
				ChangeKeyImage ("A", Skills.ChainLightning);
				break;
			case Skills.FirePillar:
				ChangeKeyImage ("A", Skills.FirePillar);
				break;
			case Skills.IceSpike:
				ChangeKeyImage ("A", Skills.IceSpike);
				break;
			case Skills.DrainHeal:
				ChangeKeyImage ("A", Skills.DrainHeal);
				break;
			case Skills.AoeLightning:
				ChangeKeyImage ("A", Skills.AoeLightning);
				break;
			case Skills.GroundSmash:
				ChangeKeyImage ("A", Skills.GroundSmash);
				break;
			case Skills.VerticalStrike:
				ChangeKeyImage ("A", Skills.VerticalStrike);
				break;
			case Skills.SpearBreaker:
				ChangeKeyImage ("A", Skills.SpearBreaker);
				break;
			}

			switch (GameManager.instance.player1.skillSType) {
			case Skills.ChainLightning:
				ChangeKeyImage ("S", Skills.ChainLightning);
				break;
			case Skills.FirePillar:
				ChangeKeyImage ("S", Skills.FirePillar);
				break;
			case Skills.IceSpike:
				ChangeKeyImage ("S", Skills.IceSpike);
				break;
			case Skills.DrainHeal:
				ChangeKeyImage ("S", Skills.DrainHeal);
				break;
			case Skills.AoeLightning:
				ChangeKeyImage ("S", Skills.AoeLightning);
				break;
			case Skills.GroundSmash:
				ChangeKeyImage ("S", Skills.GroundSmash);
				break;
			case Skills.VerticalStrike:
				ChangeKeyImage ("S", Skills.VerticalStrike);
				break;
			case Skills.SpearBreaker:
				ChangeKeyImage ("S", Skills.SpearBreaker);
				break;
			}

			switch (GameManager.instance.player1.skillDType) {
			case Skills.ChainLightning:
				ChangeKeyImage ("D", Skills.ChainLightning);
				break;
			case Skills.FirePillar:
				ChangeKeyImage ("D", Skills.FirePillar);
				break;
			case Skills.IceSpike:
				ChangeKeyImage ("D", Skills.IceSpike);
				break;
			case Skills.DrainHeal:
				ChangeKeyImage ("D", Skills.DrainHeal);
				break;
			case Skills.AoeLightning:
				ChangeKeyImage ("D", Skills.AoeLightning);
				break;
			case Skills.GroundSmash:
				ChangeKeyImage ("D", Skills.GroundSmash);
				break;
			case Skills.VerticalStrike:
				ChangeKeyImage ("D", Skills.VerticalStrike);
				break;
			case Skills.SpearBreaker:
				ChangeKeyImage ("D", Skills.SpearBreaker);
				break;
			}

		}

		if (player == 2) {
			switch (GameManager.instance.player2.skillCType) {
			case Skills.ChainLightning:
				ChangeKeyImage ("C", Skills.ChainLightning);
				break;
			case Skills.FirePillar:
				ChangeKeyImage ("C", Skills.FirePillar);
				break;
			case Skills.IceSpike:
				ChangeKeyImage ("C", Skills.IceSpike);
				break;
			case Skills.DrainHeal:
				ChangeKeyImage ("C", Skills.DrainHeal);
				break;
			case Skills.AoeLightning:
				ChangeKeyImage ("C", Skills.AoeLightning);
				break;
			case Skills.GroundSmash:
				ChangeKeyImage ("C", Skills.GroundSmash);
				break;
			case Skills.VerticalStrike:
				ChangeKeyImage ("C", Skills.VerticalStrike);
				break;
			case Skills.SpearBreaker:
				ChangeKeyImage ("C", Skills.SpearBreaker);
				break;

			}

			switch (GameManager.instance.player2.skillVType) {
			case Skills.ChainLightning:
				ChangeKeyImage ("V", Skills.ChainLightning);
				break;
			case Skills.FirePillar:
				ChangeKeyImage ("V", Skills.FirePillar);
				break;
			case Skills.IceSpike:
				ChangeKeyImage ("V", Skills.IceSpike);
				break;
			case Skills.DrainHeal:
				ChangeKeyImage ("V", Skills.DrainHeal);
				break;
			case Skills.AoeLightning:
				ChangeKeyImage ("V", Skills.AoeLightning);
				break;
			case Skills.GroundSmash:
				ChangeKeyImage ("V", Skills.GroundSmash);
				break;
			case Skills.VerticalStrike:
				ChangeKeyImage ("V", Skills.VerticalStrike);
				break;
			case Skills.SpearBreaker:
				ChangeKeyImage ("V", Skills.SpearBreaker);
				break;
			}

			switch (GameManager.instance.player2.skillAType) {
			case Skills.ChainLightning:
				ChangeKeyImage ("A", Skills.ChainLightning);
				break;
			case Skills.FirePillar:
				ChangeKeyImage ("A", Skills.FirePillar);
				break;
			case Skills.IceSpike:
				ChangeKeyImage ("A", Skills.IceSpike);
				break;
			case Skills.DrainHeal:
				ChangeKeyImage ("A", Skills.DrainHeal);
				break;
			case Skills.AoeLightning:
				ChangeKeyImage ("A", Skills.AoeLightning);
				break;
			case Skills.GroundSmash:
				ChangeKeyImage ("A", Skills.GroundSmash);
				break;
			case Skills.VerticalStrike:
				ChangeKeyImage ("A", Skills.VerticalStrike);
				break;
			case Skills.SpearBreaker:
				ChangeKeyImage ("A", Skills.SpearBreaker);
				break;
			}

			switch (GameManager.instance.player2.skillSType) {
			case Skills.ChainLightning:
				ChangeKeyImage ("S", Skills.ChainLightning);
				break;
			case Skills.FirePillar:
				ChangeKeyImage ("S", Skills.FirePillar);
				break;
			case Skills.IceSpike:
				ChangeKeyImage ("S", Skills.IceSpike);
				break;
			case Skills.DrainHeal:
				ChangeKeyImage ("S", Skills.DrainHeal);
				break;
			case Skills.AoeLightning:
				ChangeKeyImage ("S", Skills.AoeLightning);
				break;
			case Skills.GroundSmash:
				ChangeKeyImage ("S", Skills.GroundSmash);
				break;
			case Skills.VerticalStrike:
				ChangeKeyImage ("S", Skills.VerticalStrike);
				break;
			case Skills.SpearBreaker:
				ChangeKeyImage ("S", Skills.SpearBreaker);
				break;
			}

			switch (GameManager.instance.player2.skillDType) {
			case Skills.ChainLightning:
				ChangeKeyImage ("D", Skills.ChainLightning);
				break;
			case Skills.FirePillar:
				ChangeKeyImage ("D", Skills.FirePillar);
				break;
			case Skills.IceSpike:
				ChangeKeyImage ("D", Skills.IceSpike);
				break;
			case Skills.DrainHeal:
				ChangeKeyImage ("D", Skills.DrainHeal);
				break;
			case Skills.AoeLightning:
				ChangeKeyImage ("D", Skills.AoeLightning);
				break;
			case Skills.GroundSmash:
				ChangeKeyImage ("D", Skills.GroundSmash);
				break;
			case Skills.VerticalStrike:
				ChangeKeyImage ("D", Skills.VerticalStrike);
				break;
			case Skills.SpearBreaker:
				ChangeKeyImage ("D", Skills.SpearBreaker);
				break;
			}

		}

		RefreshSkillKeyIcons ();
	}

	// Use this for initialization
	public override void Start () {
		base.Start ();


	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();
		AssignKey ();
	}

	public override void Select(){
		base.Select ();
		gameObject.SetActive (true);
		//selectedImg.SetActive (true);
		//Active ();
		//SelectionActive ();
	}

	public override void Deselect() {
		base.Deselect ();
		//selectedImg.SetActive (false);
		//Inactive ();
	}

	public override void Submit() {
		if (SubmitBtn != null) {
			SubmitBtn.Select ();
			selected = false;
			//Inactive ();
			//SelectionInactive ();
		}


	}

	public override void Cancel() {

		if (CancelBtn != null) {
			CancelBtn.Select ();
			Deselect ();
			gameObject.SetActive (false);
			CancelBtn.SubmitBtn = this;
		}

	}

	public void AssignKey() {
		if (player == 1) {
			if (Input.GetKeyDown (KeyCode.C)) {
				GameManager.instance.player1.SwapSkillC (assignSkill);
				ChangeKeyImage ("C", assignSkill);
				Cancel ();

			} else if (Input.GetKeyDown (KeyCode.V)) {
				GameManager.instance.player1.SwapSkillV (assignSkill);
				ChangeKeyImage ("V", assignSkill);
				Cancel ();
			} else if (Input.GetKeyDown (KeyCode.A)) {
				GameManager.instance.player1.SwapSkillA (assignSkill);
				ChangeKeyImage ("A", assignSkill);
				Cancel ();
			} else if (Input.GetKeyDown (KeyCode.S)) {
				GameManager.instance.player1.SwapSkillS (assignSkill);
				ChangeKeyImage ("S", assignSkill);
				Cancel ();
			} else if (Input.GetKeyDown (KeyCode.D)) {
				GameManager.instance.player1.SwapSkillD (assignSkill);
				ChangeKeyImage ("D", assignSkill);
				Cancel ();
			}
		}

		if (player == 2) {
			if (Input.GetButtonDown ("YButtonCtrl1")) {
				GameManager.instance.player2.SwapSkillC (assignSkill);
				ChangeKeyImage ("C", assignSkill);
				Cancel ();
			} else if (Input.GetButtonDown ("LButtonCtrl1")) {
				GameManager.instance.player2.SwapSkillV (assignSkill);
				ChangeKeyImage ("V", assignSkill);
				Cancel ();
			} else if (Input.GetButtonDown ("RButtonCtrl1")) {
				GameManager.instance.player2.SwapSkillA (assignSkill);
				ChangeKeyImage ("A", assignSkill);
				Cancel ();
			} else if (LTAxis()) {
				GameManager.instance.player2.SwapSkillS (assignSkill);
				ChangeKeyImage ("S", assignSkill);
				Cancel ();
			} else if (RTAxis()) {
				GameManager.instance.player2.SwapSkillD (assignSkill);
				ChangeKeyImage ("D", assignSkill);
				Cancel ();
			}



		}

	}

	public void ChangeKeyImage(string key, Skills skill) {

		switch (skill) {
		case Skills.FirePillar:
			transform.Find (key).GetChild (0).GetComponent<Image> ().sprite = firePillar.sprite;
			transform.Find (key).GetChild (0).GetComponent<Image> ().color = Color.white;
			break;
		case Skills.IceSpike:
			transform.Find (key).GetChild (0).GetComponent<Image> ().sprite = iceSpikes.sprite;
			transform.Find (key).GetChild (0).GetComponent<Image> ().color = Color.white;
			break;
		case Skills.ChainLightning:
			transform.Find (key).GetChild (0).GetComponent<Image> ().sprite = chainLightning.sprite;
			transform.Find (key).GetChild (0).GetComponent<Image> ().color = Color.white;
			break;
		case Skills.DrainHeal:
			transform.Find (key).GetChild (0).GetComponent<Image> ().sprite = drainHeal.sprite;
			transform.Find (key).GetChild (0).GetComponent<Image> ().color = Color.white;
			break;
		case Skills.AoeLightning:
			transform.Find (key).GetChild (0).GetComponent<Image> ().sprite = aoeLightning.sprite;
			transform.Find (key).GetChild (0).GetComponent<Image> ().color = Color.white;
			break;
		case Skills.GroundSmash:
			transform.Find (key).GetChild (0).GetComponent<Image> ().sprite = groundSmash.sprite;
			transform.Find (key).GetChild (0).GetComponent<Image> ().color = Color.white;
			break;
		case Skills.VerticalStrike:
			transform.Find (key).GetChild (0).GetComponent<Image> ().sprite = verticalStrike.sprite;
			transform.Find (key).GetChild (0).GetComponent<Image> ().color = Color.white;
			break;
		case Skills.SpearBreaker:
			transform.Find (key).GetChild (0).GetComponent<Image> ().sprite = spearBreaker.sprite;
			transform.Find (key).GetChild (0).GetComponent<Image> ().color = Color.white;
			break;
		}

		RefreshSkillKeyIcons ();

	}

	public void RefreshSkillKeyIcons() {

		int noOfSkills = 0;

		skill1.SetActive (false);
		skill2.SetActive (false);
		skill3.SetActive (false);
		skill4.SetActive (false);
		skill5.SetActive (false);

		if (player == 1) {
			if (GameManager.instance.player1.skillCType != Skills.None) {
				noOfSkills++;
				skill1.SetActive (true);
				skill1.GetComponent<Image> ().sprite = transform.Find ("C").GetChild (0).GetComponent<Image> ().sprite;
				skill1.transform.Find ("Text").GetComponent<Text> ().text = "C";
			} if (GameManager.instance.player1.skillVType != Skills.None) {
				noOfSkills++;
				if (noOfSkills == 1) {
					skill1.SetActive (true);
					skill1.GetComponent<Image> ().sprite = transform.Find ("V").GetChild (0).GetComponent<Image> ().sprite;
					skill1.transform.Find ("Text").GetComponent<Text> ().text = "V";
				} else if (noOfSkills == 2) {
					skill2.SetActive (true);
					skill2.GetComponent<Image> ().sprite = transform.Find ("V").GetChild (0).GetComponent<Image> ().sprite;
					skill2.transform.Find ("Text").GetComponent<Text> ().text = "V";
				}
			} if (GameManager.instance.player1.skillAType != Skills.None) {
				noOfSkills++;
				if (noOfSkills == 1) {
					skill1.SetActive (true);
					skill1.GetComponent<Image> ().sprite = transform.Find ("A").GetChild (0).GetComponent<Image> ().sprite;
					skill1.transform.Find ("Text").GetComponent<Text> ().text = "A";
				} else if (noOfSkills == 2) {
					skill2.SetActive (true);
					skill2.GetComponent<Image> ().sprite = transform.Find ("A").GetChild (0).GetComponent<Image> ().sprite;
					skill2.transform.Find ("Text").GetComponent<Text> ().text = "A";
				} else if (noOfSkills == 3) {
					skill3.SetActive (true);
					skill3.GetComponent<Image> ().sprite = transform.Find ("A").GetChild (0).GetComponent<Image> ().sprite;
					skill3.transform.Find ("Text").GetComponent<Text> ().text = "A";
				}
			} if (GameManager.instance.player1.skillSType != Skills.None) {
				noOfSkills++;
				if (noOfSkills == 1) {
					skill1.SetActive (true);
					skill1.GetComponent<Image> ().sprite = transform.Find ("S").GetChild (0).GetComponent<Image> ().sprite;
					skill1.transform.Find ("Text").GetComponent<Text> ().text = "S";
				} else if (noOfSkills == 2) {
					skill2.SetActive (true);
					skill2.GetComponent<Image> ().sprite = transform.Find ("S").GetChild (0).GetComponent<Image> ().sprite;
					skill2.transform.Find ("Text").GetComponent<Text> ().text = "S";
				} else if (noOfSkills == 3) {
					skill3.SetActive (true);
					skill3.GetComponent<Image> ().sprite = transform.Find ("S").GetChild (0).GetComponent<Image> ().sprite;
					skill3.transform.Find ("Text").GetComponent<Text> ().text = "S";
				} else if (noOfSkills == 4) {
					skill4.SetActive (true);
					skill4.GetComponent<Image> ().sprite = transform.Find ("S").GetChild (0).GetComponent<Image> ().sprite;
					skill4.transform.Find ("Text").GetComponent<Text> ().text = "S";
				}
			} if (GameManager.instance.player1.skillDType != Skills.None) {
				noOfSkills++;
				if (noOfSkills == 1) {
					skill1.SetActive (true);
					skill1.GetComponent<Image> ().sprite = transform.Find ("D").GetChild (0).GetComponent<Image> ().sprite;
					skill1.transform.Find ("Text").GetComponent<Text> ().text = "D";
				} else if (noOfSkills == 2) {
					skill2.SetActive (true);
					skill2.GetComponent<Image> ().sprite = transform.Find ("D").GetChild (0).GetComponent<Image> ().sprite;
					skill2.transform.Find ("Text").GetComponent<Text> ().text = "D";
				} else if (noOfSkills == 3) {
					skill3.SetActive (true);
					skill3.GetComponent<Image> ().sprite = transform.Find ("D").GetChild (0).GetComponent<Image> ().sprite;
					skill3.transform.Find ("Text").GetComponent<Text> ().text = "D";
				} else if (noOfSkills == 4) {
					skill4.SetActive (true);
					skill4.GetComponent<Image> ().sprite = transform.Find ("D").GetChild (0).GetComponent<Image> ().sprite;
					skill4.transform.Find ("Text").GetComponent<Text> ().text = "D";
				} else if (noOfSkills == 5) {
					skill5.SetActive (true);
					skill5.GetComponent<Image> ().sprite = transform.Find ("D").GetChild (0).GetComponent<Image> ().sprite;
					skill5.transform.Find ("Text").GetComponent<Text> ().text = "D";
				}
			}
		}
	}

	bool LTAxis() 
	{
		float ltAxis = Input.GetAxis ("LTAxisCtrl1");

		if (ltAxis >= 0.5f)
			return true;
		else
			return false;
	}

	bool RTAxis()
	{
		float rtAxis = Input.GetAxis ("RTAxisCtrl1");
		if (rtAxis >= 0.5f)
			return true;
		else
			return false;
	}
}
