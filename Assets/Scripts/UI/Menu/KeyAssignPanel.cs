using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class KeyAssignPanel : MenuButton {

	public Skills assignSkill;

	public Image firePillar, iceSpikes, chainLightning, drainHeal, aoeLightning, groundSmash, verticalStrike, frontSlash;

	public override void Awake () {
		
		base.Awake ();

	}

	// Use this for initialization
	public override void Start () {
		base.Start ();

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
			}

		}

		if (player == 2) {
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
			}

		}
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
		case Skills.FrontSlash:
			transform.Find (key).GetChild (0).GetComponent<Image> ().sprite = frontSlash.sprite;
			transform.Find (key).GetChild (0).GetComponent<Image> ().color = Color.white;
			break;
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
