using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsBtn : MenuButton {

	public enum SettingsType {Slider, Button}
	public SettingsType type;

	public override void Awake () {
		btnType = ButtonTypes.Settings;
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
		Active ();

	}

	public override void Deselect() {
		base.Deselect ();
		Inactive ();

	}

	public override void Submit() {
		if (SubmitBtn != null) {
			SubmitBtn.Select ();
			selected = false;
			Inactive ();

		}
	}

	public override void Cancel() {

		if (CancelBtn != null) {
			CancelBtn.Select ();
			Deselect ();
			CancelBtn.SubmitBtn = this;
		}
	}

	public override bool MoveLeft() {

		GetComponent<Slider> ().value -= 5f;
		return false;
	}

	public override bool MoveRight() {

		GetComponent<Slider> ().value += 5f;
		return false;

	}

	public void setBGM(float val)
	{
		BGMManager.instance.setBGM(val);
		PlayerPrefs.SetFloat("BGM", val);
	}

	public void setSFX(float val)
	{
		SFXManager.instance.setSFX(val);
		PlayerPrefs.SetFloat("SFX", val);
	}

}
