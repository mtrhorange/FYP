using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public static Player instance;

	float maxHealth = 100f;
	float health = 100f;
	float maxStamina = 100f;
	float stamina = 100f;
	bool recoverStamina = true; //Can stamina be recovered

	int level = 1;
	float exp = 0;

	public Weapon currentWeapon;

	public HealthBar healthBar;
	public StaminaBar staminaBar;

	GameObject attackTrigger;
	GameObject enemyTargetHover;

	RPGCharacterControllerFREE controller;

	void Awake() {

		instance = this;

	}

	// Use this for initialization
	void Start () {

		if (GetComponent<RPGCharacterControllerFREE> ())
			controller = GetComponent<RPGCharacterControllerFREE> ();

		//attackTrigger = transform.Find ("AttackTrigger").gameObject;
		//enemyTargetHover = transform.Find ("Target").gameObject;


	}
	
	// Update is called once per frame
	void Update () {



	}

	public void AttackTrigger(int i)
	{
		currentWeapon.AttackTrigger (i);


	}

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
		}

	}

	//Player max health
	public float MaxHealth {
		get {
			return maxHealth;
		}

		set {
			maxHealth = value;
			if (health > maxHealth)
				health = maxHealth;

		}


	}

	//Player Max Stamina
	public float MaxStamina{ get; set; }

	public int Level {

		get { return level; }
		set {
			if (value < 0)
				level = 1;
			else
				level = value;
		}

	}

	public float Exp {

		get { return exp; }
		set {
			exp = value;

		}

	}


}
