using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyAssignPanel : MenuButton {



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
}
