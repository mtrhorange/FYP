using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Player : MonoBehaviour {

	#region Variables

	#region Stats
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
	public float spellStaminaDrain = 20f;
	public int livesRemaining = 5;
	public int skillPoints = 0;
	bool isDead = false;
	bool isPermaDead = false;
	public bool canBeHit = true;
	public bool isMelee = true;

	public int level = 1;
	public float exp = 0;

	#endregion

	#region SkillKeys

	public delegate void SkillKey();
	public SkillKey skillC, skillV, skillA, skillS, skillD;

	public delegate float SkillCastTime();
	public SkillCastTime skillCTime, skillVTime, skillATime, skillSTime, skillDTime;

	public Skills skillCType, skillVType, skillAType, skillSType, skillDType;

	public delegate float SkillCost();
	public SkillCost skillCCost, skillVCost, skillACost, skillSCost, skillDCost;

	#endregion

	#region Etc

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

	#endregion

	#region Status Effects

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

    #endregion

	#region Initialization

	public Player(string n, int sId) {

		name = n;
		saveId = sId;

	}

	void Awake() {

		if (GetComponent<PlayerController> ())
			controller = GetComponent<PlayerController> ();


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
                bar.SetPlayer(this);
            if (GameManager.instance.twoPlayers == false && bar.playerNo == 2)
                bar.transform.parent.gameObject.SetActive(false);
		}

		StaminaBar[] stambars = FindObjectsOfType<StaminaBar> ();

		foreach (StaminaBar bar in stambars) {

			if (bar.playerNo == playerNo)
				bar.SetPlayer (this);
			if (GameManager.instance.twoPlayers == false && bar.playerNo == 2) 
				bar.transform.parent.gameObject.SetActive (false);
		}

		canBeHit = true;

		switch (skillCType) {
		case Skills.ChainLightning:
			SwapSkillC (Skills.ChainLightning);
			break;
		case Skills.FirePillar:
			SwapSkillC (Skills.FirePillar);
			break;
		case Skills.IceSpike:
			SwapSkillC (Skills.IceSpike);
			break;
		case Skills.DrainHeal:
			SwapSkillC (Skills.DrainHeal);
			break;
		case Skills.AoeLightning:
			SwapSkillC (Skills.AoeLightning);
			break;
		case Skills.GroundSmash:
			SwapSkillC (Skills.GroundSmash);
			break;
		case Skills.VerticalStrike:
			SwapSkillC (Skills.VerticalStrike);
			break;
		case Skills.FrontSlash:
			SwapSkillC (Skills.FrontSlash);
			break;
		}

		switch (skillVType) {
		case Skills.ChainLightning:
			SwapSkillV (Skills.ChainLightning);
			break;
		case Skills.FirePillar:
			SwapSkillV (Skills.FirePillar);
			break;
		case Skills.IceSpike:
			SwapSkillV (Skills.IceSpike);
			break;
		case Skills.DrainHeal:
			SwapSkillV (Skills.DrainHeal);
			break;
		case Skills.AoeLightning:
			SwapSkillV (Skills.AoeLightning);
			break;
		case Skills.GroundSmash:
			SwapSkillV (Skills.GroundSmash);
			break;
		case Skills.VerticalStrike:
			SwapSkillV (Skills.VerticalStrike);
			break;
		case Skills.FrontSlash:
			SwapSkillV (Skills.FrontSlash);
			break;

		}

	}

	#endregion

	#region Update

	// Update is called once per frame
	void Update () {

		UpdateHealth ();

		StatusEffectsUpdate ();

		if (recoverStamina && Stamina < MaxStamina && !controller.isCasting) {
			RecoverStamina (Time.deltaTime * 10f);

		}

		UpdateStamina ();

		CheatCodes ();
	}

    //LateUpdate
    void LateUpdate()
    {
        if (playerNo == 1)
        {
            transform.Find("p1").transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        else if (playerNo == 2)
        {
            transform.Find("p2").transform.rotation = Quaternion.Euler(90, 0, 0);
        }
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

			if (isMelee)
				dmg *= 0.5f;

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
		Camera camera = FindObjectOfType<Camera> ();
		Vector3 screenPos = camera.WorldToScreenPoint (transform.position);
		GameObject txt = (GameObject)Instantiate (damageText, screenPos, Quaternion.identity);
		txt.transform.SetParent (GameObject.Find ("Canvas").transform);
		txt.GetComponent<Text> ().text = f.ToString ("F0");
		txt.GetComponent<DamageText> ().target = transform;
		txt.GetComponent<Text> ().color = Color.green;
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
        //add level up bling bling effect
        SFXManager.instance.playSFX(sounds.levelup);
        GameObject levelUpBeam = (GameObject)Instantiate(Resources.Load("LevelUpEffect"), transform.position, Quaternion.identity);
        levelUpBeam.GetComponent<LevelUp>().p = this.gameObject;
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

	#region FirePillar
	public void CastFirePillar() {
		GameObject spellFireTransmutation = (GameObject)Resources.Load ("Skills/TransmutationFire");
		GameObject spellFirePillar = (GameObject)Resources.Load ("Skills/FirePillar");
		Instantiate (spellFireTransmutation, transform.position + transform.forward * 8f, Quaternion.identity);
		GameObject firePillar = (GameObject)Instantiate (spellFirePillar, transform.position + transform.forward * 8f, Quaternion.identity);
		firePillar.GetComponent<fire_pillar> ().player = this;

	}

	public float GetFirePillarTime() {
		int lvl = transform.GetComponent<PlayerSkills> ().firePillarLevel;
		float time = 1f;
		return time;
	}

	public float GetFirePillarCost() {

		float cost = 10f;
		return cost;
	}
	#endregion

	#region ChainLightning
	public void CastChainLightning() {

		Enemy[]	enemiesA = FindObjectsOfType<Enemy> ();
		List<Enemy> enemies = new List<Enemy> ();
		foreach (Enemy e in enemiesA) {

			if (e.myState != Enemy.States.Dead && Vector3.Angle (transform.forward, (e.transform.position - transform.position).normalized) < 45 ||
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

			lightning.GetComponent<ChainLightning> ().StartPosition = transform.position + transform.up * 3f;
			lightning.GetComponent<ChainLightning> ().EndObject = closest;
			lightning.GetComponent<ChainLightning> ().player = this;
		}
			
	}

	public float GetChainLightningTime() {

		float time = 0.7f;
		return time;

	}

	public float GetChainLightningCost() {

		float cost = 5f;
		return cost;
	}
	#endregion

	#region IceBolt
	public void CastIceBolt() {

		GameObject spellIceBall = (GameObject)Resources.Load ("Skills/Ice_Ball");
		Instantiate (spellIceBall, transform.position + transform.up * 2f + transform.forward, transform.localRotation);

	}

	#endregion

	#region IceSpike
	public void CastIceSpike() {

//		StartCoroutine (_IceSpike());
//		StopCoroutine (_IceSpike ());
        SFXManager.instance.playSFX(sounds.iceSpike);
        GameObject iceSpike = (GameObject)Resources.Load ("Skills/IceSpikeSpell");
		iceSpike = (GameObject)Instantiate (iceSpike, transform.position, transform.localRotation);
		iceSpike.GetComponent<IceSpikeSpell> ().player = this;
	}

	public float GetIceSpikeTime() {
		int lvl = transform.GetComponent<PlayerSkills> ().firePillarLevel;
		float time = 1.3f;
		return time;
	}

	public float GetIceSpikeCost() {

		float cost = 5f;
		return cost;
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

	#region DrainHeal
	public void CastDrainHeal() {

		GameObject spellDrainHeal = (GameObject)Resources.Load ("Skills/DrainHeal");
		spellDrainHeal = (GameObject)Instantiate (spellDrainHeal, transform.position + transform.forward * 6f + transform.up * 0.5f, transform.rotation * Quaternion.Euler(-90,0,0));
		spellDrainHeal.GetComponent<DrainHeal> ().player = this;
	}

	public float GetDrainHealTime() {

		float time = 2.5f;
		return time;

	}

	public float GetDrainHealCost() {

		float cost = 5f;
		return cost;
	}
	#endregion

	#region AoeLightning
	public void CastAoeLightning() {

		GameObject spellGroundSmash = (GameObject)Resources.Load ("Skills/GroundSmash");
		spellGroundSmash = (GameObject)Instantiate (spellGroundSmash, transform.position + transform.forward * 5f, Quaternion.identity);
		spellGroundSmash.GetComponent<Spell> ().player = this;

	}

	public float GetAoeLightningTime() {

		float time = 0.3f;
		return time;

	}

	public float GetAoeLightningCost() {

		float cost = 20f;
		return cost;
	}
	#endregion

	#region GroundSmash
	public void CastGroundSmash() {

		GameObject spellGroundSmash = (GameObject)Resources.Load ("Skills/GroundSmash");
		spellGroundSmash = (GameObject)Instantiate (spellGroundSmash, transform.position + transform.forward * 5f, Quaternion.identity);
		spellGroundSmash.GetComponent<Spell> ().player = this;

	}

	public float GetGroundSmashTime() {

		float time = 0.3f;
		return time;

	}

	public float GetGroundSmashCost() {

		float cost = 20f;
		return cost;
	}
	#endregion

	#region VerticalStrike

	public void CastVerticalStrike() {

		GameObject spellGroundSmash = (GameObject)Resources.Load ("Skills/GroundSmash");
		spellGroundSmash = (GameObject)Instantiate (spellGroundSmash, transform.position + transform.forward * 5f, Quaternion.identity);
		spellGroundSmash.GetComponent<Spell> ().player = this;

	}

	public float GetVerticalStrikeTime() {

		float time = 0.3f;
		return time;

	}

	public float GetVerticalStrikeCost() {

		float cost = 20f;
		return cost;
	}

	#endregion

	#region FrontSlash
	public void CastFrontSlash() {

		GameObject spellGroundSmash = (GameObject)Resources.Load ("Skills/GroundSmash");
		spellGroundSmash = (GameObject)Instantiate (spellGroundSmash, transform.position + transform.forward * 5f, Quaternion.identity);
		spellGroundSmash.GetComponent<Spell> ().player = this;

	}

	public float GetFrontSlashTime() {

		float time = 0.3f;
		return time;

	}

	public float GetFrontSlashCost() {

		float cost = 20f;
		return cost;
	}
	#endregion

	#endregion

	#region SwapSpells

	public void SwapSkillC(Skills s) {

		if (s == Skills.FirePillar) {
			skillC = CastFirePillar;
			skillCTime = GetFirePillarTime;
			skillCType = Skills.FirePillar;
			skillCCost = GetFirePillarCost;
		} else if (s == Skills.IceSpike) {
			skillC = CastIceSpike;
			skillCTime = GetIceSpikeTime;
			skillCType = Skills.IceSpike;
			skillCCost = GetIceSpikeCost;
		} else if (s == Skills.ChainLightning) {
			skillC = CastChainLightning;
			skillCTime = GetChainLightningTime;
			skillCType = Skills.ChainLightning;
			skillCCost = GetChainLightningCost;
		} else if (s == Skills.DrainHeal) {
			skillC = CastDrainHeal;
			skillCTime = GetDrainHealTime;
			skillCType = Skills.DrainHeal;
			skillCCost = GetDrainHealCost;
		} else if (s == Skills.AoeLightning) {
			skillC = CastAoeLightning;
			skillCTime = GetAoeLightningTime;
			skillCType = Skills.AoeLightning;
			skillCCost = GetAoeLightningCost;

		} else if (s == Skills.GroundSmash) {
			skillC = CastGroundSmash;
			skillCTime = GetGroundSmashTime;
			skillCType = Skills.GroundSmash;
			skillCCost = GetGroundSmashCost;
		} else if (s == Skills.VerticalStrike) {
			skillC = CastVerticalStrike;
			skillCTime = GetVerticalStrikeTime;
			skillCType = Skills.DrainHeal;
			skillCCost = GetVerticalStrikeCost;
		} else if (s == Skills.FrontSlash) {
			skillC = CastFrontSlash;
			skillCTime = GetFrontSlashTime;
			skillCType = Skills.FrontSlash;
			skillCCost = GetFrontSlashCost;
		}
	}

	public void SwapSkillV(Skills s) {

		if (s == Skills.FirePillar) {
			skillV = CastFirePillar;
			skillVTime = GetFirePillarTime;
			skillVType = Skills.FirePillar;
			skillVCost = GetFirePillarCost;
		} else if (s == Skills.IceSpike) {
			skillV = CastIceSpike;
			skillVTime = GetIceSpikeTime;
			skillVType = Skills.IceSpike;
			skillVCost = GetIceSpikeCost;
		} else if (s == Skills.ChainLightning) {
			skillV = CastChainLightning;
			skillVTime = GetChainLightningTime;
			skillVType = Skills.ChainLightning;
			skillVCost = GetChainLightningCost;
		} else if (s == Skills.DrainHeal) {
			skillV = CastDrainHeal;
			skillVTime = GetDrainHealTime;
			skillVType = Skills.DrainHeal;
			skillVCost = GetDrainHealCost;
		} else if (s == Skills.AoeLightning) {
			skillV = CastAoeLightning;
			skillVTime = GetAoeLightningTime;
			skillVType = Skills.AoeLightning;
			skillVCost = GetAoeLightningCost;
		} else if (s == Skills.GroundSmash) {
			skillV = CastGroundSmash;
			skillVTime = GetGroundSmashTime;
			skillVType = Skills.GroundSmash;
			skillVCost = GetGroundSmashCost;
		} else if (s == Skills.VerticalStrike) {
			skillV = CastVerticalStrike;
			skillVTime = GetVerticalStrikeTime;
			skillVType = Skills.VerticalStrike;
			skillVCost = GetVerticalStrikeCost;
		} else if (s == Skills.FrontSlash) {
			skillV = CastFrontSlash;
			skillVTime = GetFrontSlashTime;
			skillVType = Skills.FrontSlash;
			skillVCost = GetFrontSlashCost;
		}
	}

	#endregion

	#region Cheats

	public void CheatCodes() {
	

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			ReceiveDamage (10f);
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			ReceiveHeal (50f);
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			RecoverStamina (1000f);
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			SkillPoints++;
		}

		if (Input.GetKeyDown (KeyCode.Alpha5)) {

			LevelUp ();
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
