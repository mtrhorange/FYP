using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class KeyAssignPanel : MenuButton {

	public Skills assignSkill;
	public int player = 1;

	public Image firePillar, iceSpikes, ChainLightning;

	public override void Awake () {
		
		base.Awake ();

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

		if (Input.GetKeyDown (KeyCode.C)) {
			if (player == 1)
				GameManager.instance.player1.SwapSkillC (assignSkill);
			else
				GameManager.instance.player2.SwapSkillC (assignSkill);
			Cancel ();

		} else if (Input.GetKeyDown (KeyCode.V)) {
			if (player == 1)
				GameManager.instance.player1.SwapSkillV (assignSkill);
			else
				GameManager.instance.player2.SwapSkillV (assignSkill);
				
			Cancel ();
		}


	}
}
