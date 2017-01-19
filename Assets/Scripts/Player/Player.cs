using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Player : MonoBehaviour {

	#region Variables

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
	public bool canBeHit = true;

	public int level = 1;
	public float exp = 0;

	public delegate void SkillKey();
	public SkillKey skillC;
	public SkillKey skillV;
	public SkillKey skillA;
	public SkillKey skillS;
	public SkillKey skillD;

	public delegate float SkillCastTime();
	public SkillCastTime skillCTime;
	public SkillCastTime skillVTime;

	public Weapon currentWeapon;
	public Weapon nextWeapon;
	public GameObject rightHand;
	public GameObject leftHand;

	public HealthBar healthBar;
	public StaminaBar staminaBar;


	GameObject attackTrigger;
	GameObject enemyTargetHover;
	GameObject damageText;

	PlayerController controller;


	public PlayerSkills skills;

	//Status Effects
	bool isBurning = false;
	bool isStrongBurning = false;
	float burnTime = 0f;
	float strongBurnTime = 0f;
	bool isSlowed = false;
	float slowTime = 0f;
	bool isPoisoned = false;
	bool isStrongPoisoned = false;
	float poisonTime = 0f;
	float strongPoisonTime = 0f;

	#endregion

	public Player(string n, int sId) {

		name = n;
		saveId = sId;

	}


	void Awake() {

		if (GetComponent<PlayerController> ())
			controller = GetComponent<PlayerController> ();

		skillC = CastChainLightning;
		skillV = CastIceSpike;
		skillCTime = GetChainLightningTime;
		skillVTime = GetIceSpikeTime;
		//attackTrigger = transform.Find ("AttackTrigger").gameObject;
		//enemyTargetHover = transform.Find ("Target").gameObject;

		if (controller.firstWep != null) {
			currentWeapon = controller.firstWep.GetComponent<Weapon>();
			currentWeapon.player = this;
		}
		if (controller.secondWep != null) {
			nextWeapon = controller.secondWep.GetComponent<Weapon>();
			nextWeapon.player = this;
		}

		skills = GetComponent<PlayerSkills> ();
		damageText = (GameObject)Resources.Load ("DamageText");
	}

	// Use this for initialization
	void Start () {

		PlayerCamera cam = FindObjectOfType<PlayerCamera> ();

		if (playerNo == 1)
			cam.cameraTarget1 = this.gameObject;
		else
			cam.cameraTarget2 = this.gameObject;

		HealthBar[] hpbars = FindObjectsOfType<HealthBar> ();

		foreach (HealthBar bar in hpbars) {

			if (bar.playerNo == playerNo)
				bar.SetPlayer (this);
			if (GameManager.instance.twoPlayers == false && bar.playerNo == 2) 
				bar.transform.parent.gameObject.SetActive (false);
		}

		StaminaBar[] stambars = FindObjectsOfType<StaminaBar> ();

		foreach (StaminaBar bar in stambars) {

			if (bar.playerNo == playerNo)
				bar.SetPlayer (this);
			if (GameManager.instance.twoPlayers == false && bar.playerNo == 2) 
				bar.transform.parent.gameObject.SetActive (false);
		}

		canBeHit = true;

	}

	#region Update

	// Update is called once per frame
	void Update () {

		UpdateHealth ();

		StatusEffectsUpdate ();

		if (recoverStamina && Stamina < MaxStamina) {
			RecoverStamina (Time.deltaTime * 2f);
			Debug.Log ("Staminaaaaaaaaaaaaaa");
		}

		UpdateStamina ();

		CheatCodes ();
	}

	#endregion


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

	#region Combat

	public void AttackTrigger(int i)
	{
		currentWeapon.AttackTrigger (i);
	}

	//Player gets damage, reduces health
	public void ReceiveDamage(float f) {
		if (canBeHit) {
			float dmg = f;

			dmg = dmg * (1f - ((0.05f * skills.defenseBuffLevel) > 0.5f ? 0.5f : (0.05f * skills.defenseBuffLevel)));

			Camera camera = FindObjectOfType<Camera> ();
			Vector3 screenPos = camera.WorldToScreenPoint (transform.position);
			GameObject txt = (GameObject)Instantiate (damageText, screenPos, Quaternion.identity);
			txt.transform.SetParent (GameObject.Find ("Canvas").transform);
			txt.GetComponent<Text> ().text = dmg.ToString ("F0");
			txt.GetComponent<DamageText> ().target = transform;
			txt.GetComponent<Text> ().color = Color.red;
			Health -= f;
			if (Health > 0)
				controller.GetHit ();
		}
	}

	public void ReceiveHeal(float f) {

		Health += f;
	}

	public void ReceiveExp(float f) {

		Exp += f;

	}

	//Player recovers health
	public void RecoverHealth(float f) {

		Health += f;
	}

	public void RecoverStamina(float f) {

		Stamina += f;
	}

	public void LevelUp() {

		Level++;
		MaxHealth += 10;
		Health = MaxHealth;
		SkillPoints++;
		currentWeapon.UpdateDamage ();
		nextWeapon.UpdateDamage ();
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

	public void UpdateStamina() {

//		if (stamina < maxStamina)
//			stamina += 3f;
//		else if (stamina > maxStamina)
//			stamina = maxStamina;

		if (staminaBar != null)
			staminaBar.SetStamina ();

	}

	public void PlayerDeath() {
		
		health = 0;
		if (!isDead) {
			isDead = true;
			canBeHit = false;
			controller.PlayerDeath ();
		}
	}

	public void PlayerRevive() {

		Health = MaxHealth;
		isDead = false;
		canBeHit = true;
		controller.PlayerRevive ();

	}

	#endregion

	#region StatusEffects

	public void ApplyBurn(float t) {
		if (!isBurning && !isStrongBurning)
			Invoke ("BurnDamage", 1.0f);

		isBurning = true;
		burnTime = t;
	}

	public void ApplyStrongBurn(float t) {
		if (!isBurning && !isStrongBurning)
			Invoke ("StrongBurnDamage", 1.0f);

		isStrongBurning = true;
		strongBurnTime = t;
	}

	public void ApplySlow(float t) {
		isSlowed = true;
		slowTime = t;
		controller.runSpeed = 3;
	}

	public void ApplyPoison(float t) {
		if (!isPoisoned && !isStrongPoisoned)
			Invoke ("PoisonDamage", 1.0f);


		isPoisoned = true;
		poisonTime = t;
	}

	public void ApplyStrongPoison(float t) {


		isStrongPoisoned = true;
		isPoisoned = false;
		poisonTime = t;
	}

	void BurnDamage() {
		StatusDamage (MaxHealth * 0.01f);
		if (isStrongBurning)
			Invoke ("StrongBurnDamage", 1.0f);
		else if (isBurning)
			Invoke ("BurnDamage", 1.0f);
	}

	void StrongBurnDamage() {
		StatusDamage (MaxHealth * 0.025f);
		if (isStrongBurning)
			Invoke ("StrongBurnDamage", 1.0f);
		else if (isBurning)
			Invoke ("BurnDamage", 1.0f);
	}

	void PoisonDamage() {
		StatusDamage (MaxHealth * 0.01f);
		if (isStrongPoisoned)
			Invoke ("StrongPoisonDamage", 1.0f);
		else if (isPoisoned)
			Invoke ("PoisonDamage", 1.0f);
	}

	void StrongPoisonDamage() {
		StatusDamage (MaxHealth * 0.025f);
		if (isStrongPoisoned)
			Invoke ("StrongPoisonDamage", 1.0f);
		else if (isPoisoned)
			Invoke ("PoisonDamage", 1.0f);
	}

	void StatusDamage(float dmg) {
		Health -= dmg;

		Camera camera = FindObjectOfType<Camera>();
		Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
		GameObject txt = (GameObject)Instantiate(damageText, screenPos, Quaternion.identity);
		txt.transform.SetParent(GameObject.Find("Canvas").transform);
		txt.GetComponent<Text>().text = dmg.ToString("F0");
		txt.GetComponent<DamageText> ().target = transform;
		txt.GetComponent<Text> ().color = Color.blue;
	}

	void StatusEffectsUpdate() {

		if (isStrongBurning) {
			strongBurnTime -= Time.deltaTime;
			if (strongBurnTime <= 0) {
				isStrongBurning = false;
			}

		}
		if (isBurning) {
			burnTime -= Time.deltaTime;
			if (burnTime <= 0) {
				isBurning = false;
			}
		}

		if (isSlowed) {
			slowTime -= Time.deltaTime;
			if (slowTime <= 0) {
				isSlowed = false;
				controller.runSpeed = 6;
			}
		}

		if (isStrongPoisoned) {
			strongPoisonTime -= Time.deltaTime;
			if (strongPoisonTime <= 0) {
				isStrongPoisoned = false;
			}

		}
		if (isPoisoned) {
			poisonTime -= Time.deltaTime;
			if (poisonTime <= 0) {
				isPoisoned = false;
			}
		}

	}

	#endregion

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
			stamina = value;
			if (stamina > maxStamina)
				stamina = maxStamina;
			else if (stamina < 0)
				stamina = 0;
		}


	}

	//Player Max Stamina
	public float MaxStamina { 
		get { return maxStamina; }
		set { 
			maxStamina = value; 
			if (stamina > maxStamina)
				stamina = maxStamina;
		}
	}

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
		GameObject spellFireTransmutation = (GameObject)Resources.Load ("Skills/TransmutationFire");
		GameObject spellFirePillar = (GameObject)Resources.Load ("Skills/FirePillar");
		Instantiate (spellFireTransmutation, transform.position + transform.forward * 8f, Quaternion.identity);
		GameObject firePillar = (GameObject)Instantiate (spellFirePillar, transform.position + transform.forward * 8f, Quaternion.identity);
		firePillar.GetComponent<fire_pillar> ().player = this;

	}

	public float GetFirePillarTime() {
		int lvl = transform.GetComponent<PlayerSkills> ().firePillarLevel;
		float time = 4f;
		return time;
	}

	public void CastChainLightning() {




		Enemy[]	enemiesA = FindObjectsOfType<Enemy> ();
		List<Enemy> enemies = new List<Enemy> ();
		foreach (Enemy e in enemiesA) {

			if (Vector3.Angle (transform.forward, (e.transform.position - transform.position).normalized) < 45 ||
			   Vector3.Angle (transform.forward, (e.transform.position - transform.position).normalized) > 315) {
				enemies.Add (e);
			}
		}

		Transform closest = null;

		foreach (Enemy e in enemies) {

			if (Vector3.Distance (transform.position, e.transform.position) < 8 && (closest == null ||
			    Vector3.Distance (transform.position, e.transform.position) < Vector3.Distance (transform.position, closest.position)))
				closest = e.transform;

		}

		if (closest != null) {
			GameObject spellLightning = (GameObject)Resources.Load ("Skills/Lightning/ChainLightning");
			GameObject lightning = (GameObject)Instantiate (spellLightning, transform.position, Quaternion.identity);

			lightning.GetComponent<ChainLightning> ().StartPosition = transform.position + transform.up * 4f;
			lightning.GetComponent<ChainLightning> ().EndObject = closest;
		}
			
	}

	public float GetChainLightningTime() {

		float time = 1f;
		return time;

	}

	public void CastIceBolt() {

		GameObject spellIceBall = (GameObject)Resources.Load ("Skills/Ice_Ball");
		Instantiate (spellIceBall, transform.position + transform.up * 2f + transform.forward, transform.localRotation);

	}

	public void CastIceSpike() {

//		StartCoroutine (_IceSpike());
//		StopCoroutine (_IceSpike ());
		GameObject iceSpike = (GameObject)Resources.Load ("Skills/IceSpikeSpell");
		iceSpike = (GameObject)Instantiate (iceSpike, transform.position, transform.localRotation);
		iceSpike.GetComponent<IceSpikeSpell> ().player = this;
	}

	public float GetIceSpikeTime() {
		int lvl = transform.GetComponent<PlayerSkills> ().firePillarLevel;
		float time = 2f;
		return time;
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

	#region Swapping

	public void SwapSkillC(Skills s) {

		if (s == Skills.FirePillar) {
			skillC = CastFirePillar;
			skillCTime = GetFirePillarTime;
		} else if (s == Skills.IceSpike) {
			skillC = CastIceSpike;
			skillCTime = GetIceSpikeTime;
		}

	}

	public void SwapSkillV(Skills s) {

		if (s == Skills.FirePillar) {
			skillV = CastFirePillar;
			skillVTime = GetFirePillarTime;
		} else if (s == Skills.IceSpike) {
			skillV = CastIceSpike;
			skillVTime = GetIceSpikeTime;
		}

	}

	#endregion

	#region Cheats

	public void CheatCodes() {
		if (playerNo == 1) {
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
			
				if (skillC == CastIceSpike) {
					SwapSkillC (Skills.FirePillar);
				} else if (skillC == CastFirePillar) {
					SwapSkillC (Skills.IceSpike);
				}

			}

			if (Input.GetKeyDown (KeyCode.Alpha2)) {
			
				if (skillV == CastIceSpike) {
					SwapSkillV (Skills.FirePillar);
				} else if (skillV == CastFirePillar) {
					SwapSkillV (Skills.IceSpike);
				}

			}

			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				ReceiveDamage (10f);
			}

			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				ReceiveHeal (10f);
			}
		}

		if (Input.GetKeyDown(KeyCode.Equals)) {
			if (isDead)
				PlayerRevive ();
		}
	}

	#endregion

	#region GUI

//	void OnGUI() {
//
//		if (playerNo == 1) {
//			if (GUI.Button (new Rect (250, 15, 100, 30), "1P c switch")) {
//				if (skillC == CastIceSpike) {
//					SwapSkillC (Skills.FirePillar);
//				} else if (skillC == CastFirePillar) {
//					SwapSkillC (Skills.IceSpike);
//				}
//			}
//
//			if (GUI.Button (new Rect (250, 45, 100, 30), "1P v switch")) {
//				if (skillV == CastIceSpike)
//					skillV = CastFirePillar;
//				else if (skillV == CastFirePillar)
//					skillV = CastIceSpike;
//			}
//		} else {
//			if (GUI.Button (new Rect (360, 15, 100, 30), "2P c switch")) {
//				if (skillC == CastIceSpike)
//					skillC = CastFirePillar;
//				else if (skillC == CastFirePillar)
//					skillC = CastIceSpike;
//			}
//
//			if (GUI.Button (new Rect (360, 45, 100, 30), "2P v switch")) {
//				if (skillV == CastIceSpike)
//					skillV = CastFirePillar;
//				else if (skillV == CastFirePillar)
//					skillV = CastIceSpike;
//			}
//
//
//		}
//		if (playerNo == 1) {
//			if (GUI.Button (new Rect (250, 75, 100, 30), "1P -10 Health")) {
//				ReceiveDamage (10);
//			}
//		} else {
//			if (GUI.Button (new Rect (360, 75, 100, 30), "2P -10 Health")) {
//				ReceiveDamage (10);
//			}
//		}
//
//		if (isDead) {
//
//			if (playerNo == 1) {
//				if (GUI.Button (new Rect (250, 105, 100, 30), "1P Revive")) {
//					PlayerRevive ();
//				}
//
//			} else {
//
//				if (GUI.Button (new Rect (250, 135, 100, 30), "2P Revive")) {
//					PlayerRevive ();
//				}
//
//			}
//
//		}
//	}

	#endregion
}
