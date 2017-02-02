using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TabBtn : MenuButton {

	public GameObject panel;

	public override void Awake () {
		btnType = ButtonTypes.Tab;
		base.Awake ();
	}

	// Use this for initialization
	public override void Start () {
		selectedImg = transform.Find ("Selection").gameObject;
		base.Start ();

	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}

	public override void Select(){
		base.Select ();
		panel.SetActive (true);
		selectedImg.SetActive (true);
		Active ();
		SelectionActive ();
	}

	public override void Deselect() {
		base.Deselect ();
		panel.SetActive (false);
		selectedImg.SetActive (false);
		Inactive ();
	}

	public override void Submit() {
		if (SubmitBtn != null) {
			SubmitBtn.Select ();
			selected = false;
			Inactive ();
			SelectionInactive ();
		}
	}

	public override void Cancel() {

		if (CancelBtn != null) {
			CancelBtn.Select ();
			Deselect ();
			CancelBtn.SubmitBtn = this;
		}

	}
}
