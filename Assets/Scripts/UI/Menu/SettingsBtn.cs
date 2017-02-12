using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsBtn : MenuButton {

	public enum SettingsType {Slider, Button}
	public SettingsType type;


	public GameObject border;

	float updateTimer = 2.0f;
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


		if (type == SettingsType.Slider) {
			updateTimer -= Time.deltaTime * 10000;
			if (updateTimer < 0) {
				if (gameObject.name == "BGM Slide" && PlayerPrefs.HasKey ("BGM")) {
					GetComponent<Slider> ().value = PlayerPrefs.GetFloat ("BGM");
				} else if (gameObject.name == "SFX Slide" && PlayerPrefs.HasKey ("SFX")) {
					GetComponent<Slider> ().value = PlayerPrefs.GetFloat ("SFX");
				}
				updateTimer = 2.0f;
			}
		}
	}

	public override void Select(){
		base.Select ();
		if (type == SettingsType.Slider)
			border.SetActive (true);
		else
			GetComponent<Image> ().color = Color.red;
		//Active ();

	}

	public override void Deselect() {
		base.Deselect ();
		if (type == SettingsType.Slider)
			border.SetActive (false);
		else
			GetComponent<Image> ().color = Color.white;
		//Inactive ();

	}

	public override void Submit() {
		if (SubmitBtn != null) {
			SubmitBtn.Select ();
			selected = false;
			Inactive ();
		}

		if (type == SettingsType.Button) {
			if (name == "quit") {
				GameMenuManager.instance.closeMenu ();
				LevelManager.instance.LoadMainMenu ();
			} else {
				GameMenuManager.instance.closeMenu ();
			}

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
		if (type == SettingsType.Slider) {
			GetComponent<Slider> ().value -= 0.05f;
			if (name == "BGM Slide")
				setBGM ();
			else
				setSFX ();
		} else {
			base.MoveLeft ();
		}
		

		return false;
	}

	public override bool MoveRight() {
		if (type == SettingsType.Slider) {
			GetComponent<Slider> ().value += 0.05f;
			if (name == "BGM Slide")
				setBGM ();
			else
				setSFX ();
		} else {
			base.MoveRight ();
		}
		return false;

	}

	public void setBGM()
	{
		float val = GetComponent<Slider> ().value;
		BGMManager.instance.setBGM(val);
		PlayerPrefs.SetFloat("BGM", val);
	}

	public void setSFX()
	{
		float val = GetComponent<Slider> ().value;
		SFXManager.instance.setSFX(val);
		PlayerPrefs.SetFloat("SFX", val);
	}

}
