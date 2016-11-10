using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Player : MonoBehaviour {


	public string name = " ";
	public int saveId = -1;
	public int playerNo = 1;
	public float baseMaxHealth = 100f;
	public float maxHealth = 100f;
	public float health = 100f;
	public float baseMaxStamina = 100f;
	public float maxStamina = 100f;
	public float stamina = 100f;
	public bool recoverStamina = true; //Can stamina be recovered
	public int livesRemaining = 5;
	public int skillPoints = 0;
	bool isDead = false;
	bool isPermaDead = false;

	public int level = 1;
	public float exp = 0;

	public delegate void SkillKey();
	public SkillKey skillC;


	public Weapon currentWeapon;
	public Weapon nextWeapon;
	public GameObject rightHand;
	public GameObject leftHand;

	public HealthBar healthBar;

	public StaminaBar staminaBar;

	GameObject attackTrigger;
	GameObject enemyTargetHover;

	RPGCharacterControllerFREE controller;

	public GameObject spellFirePillar;
	public GameObject spellFireTransmutation;
	public GameObject spellIceBall;

	public PlayerSkills skills;

	public Player(string n, int sId) {

		name = n;
		saveId = sId;

	}


	void Awake() {


	}

	// Use this for initialization
	void Start () {

		if (GetComponent<RPGCharacterControllerFREE> ())
			controller = GetComponent<RPGCharacterControllerFREE> ();

		skillC = CastFirePillar;
		//attackTrigger = transform.Find ("AttackTrigger").gameObject;
		//enemyTargetHover = transform.Find ("Target").gameObject;

		if (currentWeapon != null)
			currentWeapon.player = this;
		if (nextWeapon != null)
			nextWeapon.player = this;

		skills = GetComponent<PlayerSkills> ();
	}
	
	// Update is called once per frame
	void Update () {

		UpdateHealth ();

	}

	public void AttackTrigger(int i)
	{
		currentWeapon.AttackTrigger (i);


	}

	#region MISC

	/*
	*	Search for nearest enemy to target
	*
	*/
	public GameObject FindTarget() {

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		GameObject closest = null;
		foreach (GameObject g in enemies) {
			if (closest == null)
				closest = g;
			else if (Vector3.Distance (transform.position, closest.transform.position) > Vector3.Distance (transform.position, g.transform.position))
				closest = g;


		}

		if (closest != null) {
			enemyTargetHover.SetActive (true);
			enemyTargetHover.transform.parent = closest.transform;
			enemyTargetHover.transform.position = new Vector3 (closest.transform.position.x, enemyTargetHover.transform.position.y, closest.transform.position.z);

			return closest;
		} else
			return null;


	}

	public void ResetTarget() {

		enemyTargetHover.SetActive (false);
		enemyTargetHover.transform.parent = transform;

	}

	#endregion



	//Player gets damage, reduces health
	public void ReceiveDamage(float f) {

		float dmg = f;

		dmg = dmg * (1f - ((0.05f*skills.defenseBuffLevel)>0.5f?5:(0.05f*skills.defenseBuffLevel)));
        Health -= f;
	}

	//Player recovers health
	public void RecoverHealth(float f) {

		Health += f;
	}

	public void LevelUp() {

		Level++;
		MaxHealth += 10;
		Health = MaxHealth;
		SkillPoints++;

		//UpdateHealth ();


	}

	public void SwapWeapon() {

		Weapon temp = currentWeapon;
		currentWeapon = nextWeapon;
		nextWeapon = temp;

		currentWeapon.gameObject.SetActive (true);
		nextWeapon.gameObject.SetActive (false);

	}

	public void UpdateHealth() {

		maxHealth = Mathf.Floor(baseMaxHealth * (1f + 0.05f * skills.maxHealthLevel));
		if (health > maxHealth)
			health = maxHealth;
		if (healthBar != null)
			healthBar.SetHealth ();

	}

	public void PlayerDeath() {

		health = 0;
		if (!isDead) {
			isDead = true;
			controller.PlayerDeath ();
		}
	}

	public void PlayerRevive() {

		Health = MaxHealth;
		isDead = false;
		controller.PlayerRevive ();

	}

	#region GetSet

	//Player's current health
	public float Health {
		get {
			return health;
		}
		set {
			if (value > maxHealth)
				health = maxHealth;
			else
				health = value;

			if (health <= 0) {
				PlayerDeath ();
			}

			if (healthBar != null)
				healthBar.SetHealth ();
		}

	}

	public float BaseMaxHealth {

		get { return baseMaxHealth; }

	}

	//Player max health
	public float MaxHealth {
		get {
			return maxHealth;
		}

		set {
			baseMaxHealth = value;
			UpdateHealth ();
		}
	}

	//Player stamina
	public float Stamina {
		get { return stamina; }
		set {
			if (value > maxStamina)
				stamina = maxStamina;
			else
				stamina = value;
		}


	}

	//Player Max Stamina
	public float MaxStamina{ get; set; }

	//Player current level
	public int Level {

		get { return level; }
		set {
			if (value < 0)
				level = 1;
			else
				level = value;
		}

	}

	//Player exp & levelup
	public float Exp {

		get { return exp; }
		set {
			exp = value;
			float expLimit = 50 + (50 * level);
			if (exp >= expLimit) {
				LevelUp ();
				exp -= expLimit;

			}
		}

	}

	public int SkillPoints {

		get { return skillPoints; }
		set {
			skillPoints = value;
		}

	}

	public int Lives {

		get { return livesRemaining; }
		set { livesRemaining = value; }
	}

	#endregion

	#region Spells

	public void CastFirePillar() {

		Instantiate (spellFireTransmutation, transform.position + transform.forward * 8f, Quaternion.identity);
		GameObject firePillar = (GameObject)Instantiate (spellFirePillar, transform.position + transform.forward * 8f, Quaternion.identity);
		firePillar.GetComponent<fire_pillar> ().player = this;

	}

	public void CastIceBolt() {

		Instantiate (spellIceBall, transform.position + transform.up * 2f + transform.forward, transform.localRotation);

	}

	public void CastIceSpike() {

//		StartCoroutine (_IceSpike());
//		StopCoroutine (_IceSpike ());
		GameObject iceSpike = (GameObject)Resources.Load ("Skills/IceSpikeSpell");
		iceSpike = (GameObject)Instantiate (iceSpike, transform.position, transform.localRotation);
		iceSpike.GetComponent<IceSpikeSpell> ().player = this;
	}

	IEnumerator _IceSpike()
	{
		GameObject iceSpike = (GameObject)Resources.Load ("Skills/IceSpike");

		Vector3 pos = transform.position;
		Vector3 forward = transform.forward;

		Instantiate(iceSpike, pos + forward * 2, transform.rotation * Quaternion.Euler(0, 90, 0));
		yield return new WaitForSeconds(0.05f);
		Instantiate(iceSpike, pos + forward * 3 , transform.rotation * Quaternion.Euler(0, 300, 0));
		yield return new WaitForSeconds(0.05f);
		Instantiate(iceSpike, pos + forward * 4 , transform.rotation * Quaternion.Euler(0, 126, 0));
		yield return new WaitForSeconds(0.05f);
		Instantiate(iceSpike, pos + forward * 5 , transform.rotation * Quaternion.Euler(0, 0, 0));
		yield return new WaitForSeconds(0.05f);
		Instantiate(iceSpike, pos + forward * 6 , transform.rotation * Quaternion.Euler(0, 40, 0));
		yield return new WaitForSeconds(0.05f);
		Instantiate(iceSpike, pos + forward * 7 , transform.rotation * Quaternion.Euler(0, 185, 0));

	}

	#endregion

	#region GUI

	void OnGUI() {

		if(GUI.Button(new Rect(250, 15, 100, 30), "c FirePillar"))
		{
			skillC = CastFirePillar;
		}

		if(GUI.Button(new Rect(250, 45, 100, 30), "c IceSpikes"))
		{
			skillC = CastIceSpike;
		}

		if (GUI.Button (new Rect (250, 75, 100, 30), "-10 Health")) {
			ReceiveDamage (10);
		}

		if (isDead) {

			if (playerNo == 1) {
				if (GUI.Button (new Rect (250, 105, 100, 30), "1P Revive")) {
					PlayerRevive ();
				}

			} else {

				if (GUI.Button (new Rect (250, 135, 100, 30), "2P Revive")) {
					PlayerRevive ();
				}

			}


		}
	}

	#endregion
}
