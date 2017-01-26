using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class KeyAssignPanel : MenuButton {

	public Skills assignSkill;

	public Image firePillar, iceSpikes, ChainLightning;

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

		if (Input.GetKeyDown (KeyCode.C)) {
			if (player == 1) {
				GameManager.instance.player1.SwapSkillC (assignSkill);
				ChangeKeyImage ("C", assignSkill);
			} else {
				GameManager.instance.player2.SwapSkillC (assignSkill);
				ChangeKeyImage ("C", assignSkill);
			}
			Cancel ();

		} else if (Input.GetKeyDown (KeyCode.V)) {
			if (player == 1) {
				GameManager.instance.player1.SwapSkillV (assignSkill);
				ChangeKeyImage ("V", assignSkill);
			} else {
				GameManager.instance.player2.SwapSkillV (assignSkill);
				ChangeKeyImage ("V", assignSkill);
			}
			Cancel ();
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
			transform.Find (key).GetChild (0).GetComponent<Image> ().sprite = ChainLightning.sprite;
			transform.Find (key).GetChild (0).GetComponent<Image> ().color = Color.white;
			break;
		}
	}
}
