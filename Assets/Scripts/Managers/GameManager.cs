using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public int player1Id = 0, player2Id = 0;

	public Player player1, player2;

	public PlayerData player1Data, player2Data;

	public GameObject playerPrefab;

	public bool twoPlayers = false;


	void Awake() {
		DontDestroyOnLoad (transform.gameObject);
		if (instance && instance != this.gameObject)
			Destroy (this.gameObject);
		else
		instance = this;
	}

	// Use this for initialization
	void Start () {
		SaveLoad.Load ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnPlayer() {

        Vector3 spawnPos = Vector3.zero;
        Vector3 spawnPos2 = Vector3.zero;
        //if is game scene (rooms / floors)
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Scene1"))
        {
            spawnPos = Floor.instance.currentRoom.GetComponent<Room>().spawnPoint1.position;
            spawnPos2 = Floor.instance.currentRoom.GetComponent<Room>().spawnPoint2.position;
        }
        //if is base camp scene
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BaseCamp"))
        {
            spawnPos = GameObject.Find("Player1Spawn").transform.position;
            spawnPos2 = GameObject.Find("Player2Spawn").transform.position;
        }

        GameObject playerModel =
            (GameObject)Instantiate(playerPrefab,
                spawnPos, Quaternion.identity);

        player1 = playerModel.GetComponent<Player>();
		UpdatePlayer (player1, player1Data);

        playerModel.name = "Player1";
        playerModel.transform.Find("p1").gameObject.SetActive(true);

        if (player2Data.saveId != 0)
        {
            GameObject player2Model =
                (GameObject)Instantiate(playerPrefab,
                    spawnPos2, Quaternion.identity);
            player2 = player2Model.GetComponent<Player>();
			UpdatePlayer (player2, player2Data);

            player2Model.name = "Player2";
            player2Model.transform.Find("p2").gameObject.SetActive(true);

            twoPlayers = true;
        }
	}

	public void SelectPlayer(SaveSlot s) {

		foreach (PlayerData pd in SaveLoad.savedCharacters) {

			if (s.saveId == pd.saveId) {
				if (s.playerNo == 1)
					player1Data = pd;
				else
					player2Data = pd;
				break;
			}

		}

		if (player1Data.saveId != 0 && GameObject.Find ("Player1Info")) {

			GameObject.Find ("Player1Info").transform.GetChild (0).GetComponent<Text> ().text = 
				"Player 1\nName: " + player1Data.name + "\nLevel: " + player1Data.Level;

		}

		if (player2Data.saveId != 0 && GameObject.Find ("Player2Info")) {

			GameObject.Find ("Player2Info").transform.GetChild (0).GetComponent<Text> ().text = 
				"Player 2\nName: " + player2Data.name + "\nLevel: " + player2Data.Level;

		}


	}


	public void NewCharacter(SaveSlot s, string name) {

		PlayerData player = new PlayerData (name, s.saveId);

		for (int i = 0; i < SaveLoad.savedCharacters.Count; i++) {

			if (player.saveId == SaveLoad.savedCharacters [i].saveId) {
				SaveLoad.savedCharacters.RemoveAt (i);


			}

		}


		SaveLoad.NewCharacter (player);


	}

	public void SavePlayers() {
		if (GameManager.instance.player1Data.saveId != 0)
			GameManager.instance.UpdatePlayerData (player1, player1Data);
		if (GameManager.instance.player2Data.saveId != 0)
			GameManager.instance.UpdatePlayerData (player2, player2Data);
	}

	public void UpdatePlayerData(Player p, PlayerData pd) {

		pd.name = p.name;
		pd.saveId = p.saveId;
		pd.Level = p.Level;
		pd.Exp = p.Exp;
		pd.MaxHealth = p.MaxHealth;
		pd.baseMaxHealth = p.baseMaxHealth;
		pd.Health = p.Health;
		pd.maxStamina = p.maxStamina;
		pd.baseMaxStamina = p.baseMaxStamina;
		pd.Stamina = p.MaxStamina;
		pd.Lives = p.Lives;
		pd.SkillPoints = p.SkillPoints;
        pd.Points = p.Points;
		pd.spellStaminaDrain = p.spellStaminaDrain;
		pd.isDead = p.isDead;
		pd.isPermaDead = p.isPermaDead;


		pd.skillCType = p.skillCType;
		pd.skillVType = p.skillVType;
		pd.skillAType = p.skillAType;
		pd.skillSType = p.skillSType;
		pd.skillDType = p.skillDType;


		pd.firePillarLevel = p.skills.firePillarLevel;
		pd.iceSpikesLevel = p.skills.iceSpikesLevel;
		pd.chainLightningLevel = p.skills.chainLightningLevel;
		pd.drainHealLevel = p.skills.drainHealLevel;
		pd.aoeLightningLevel = p.skills.aoeLightningLevel;
		pd.groundSmashLevel = p.skills.groundSmashLevel;
		pd.verticalStrikeLevel = p.skills.verticalStrikeLevel;

		pd.maxHealthLevel = p.skills.maxHealthLevel;
		pd.minDmgLevel = p.skills.minDmgLevel;
		pd.maxDmgLevel = p.skills.maxDmgLevel;
		pd.weaponBuffLevel = p.skills.weaponBuffLevel;
		pd.spellBuffLevel = p.skills.spellBuffLevel;
		pd.defenseBuffLevel = p.skills.defenseBuffLevel;
		pd.frontSlashLevel = p.skills.frontSlashLevel;
		pd.iceBoltSpikeLevel = p.skills.iceBoltSpikeLevel;

		for (int i = 0; i < SaveLoad.savedCharacters.Count; i++) {
			if (pd.saveId == SaveLoad.savedCharacters [i].saveId) {
				SaveLoad.savedCharacters.RemoveAt (i);
			}
		}

		SaveLoad.NewCharacter (pd);

	}

	public void UpdatePlayer(Player p, PlayerData pd) {
		p.name = pd.name;
		p.saveId = pd.saveId;
		p.Level = pd.Level;
		p.Exp = pd.Exp;
		p.MaxHealth = pd.MaxHealth;
		p.baseMaxHealth = pd.baseMaxHealth;
		p.Health = pd.Health;
		if (p.Health <= 0) { p.PlayerRevive (); }
		p.maxStamina = pd.maxStamina;
		p.baseMaxStamina = pd.baseMaxStamina;
		p.Stamina = p.MaxStamina;
		p.Lives = pd.Lives;
		p.SkillPoints = pd.SkillPoints;
        p.Points = pd.Points;
		p.spellStaminaDrain = pd.spellStaminaDrain;
		p.recoverStamina = true;
        p.controller.rollSpeed = 10;
        p.controller.rollduration = 0.6f;

		p.isPermaDead = pd.isPermaDead;


		p.skillCType = pd.skillCType;
		p.skillVType = pd.skillVType;
		p.skillAType = pd.skillAType;
		p.skillSType = pd.skillSType;
		p.skillDType = pd.skillDType;

		p.skills.firePillarLevel = pd.firePillarLevel;
		p.skills.iceSpikesLevel = pd.iceSpikesLevel;
		p.skills.chainLightningLevel = pd.chainLightningLevel;
		p.skills.drainHealLevel = pd.drainHealLevel;
		p.skills.aoeLightningLevel = pd.aoeLightningLevel;
		p.skills.groundSmashLevel = pd.groundSmashLevel;
		p.skills.verticalStrikeLevel = pd.verticalStrikeLevel;

		p.skills.maxHealthLevel = pd.maxHealthLevel;
		p.skills.minDmgLevel = pd.minDmgLevel;
		p.skills.maxDmgLevel = pd.maxDmgLevel;
		p.skills.weaponBuffLevel = pd.weaponBuffLevel;
		p.skills.spellBuffLevel = pd.spellBuffLevel;
		p.skills.defenseBuffLevel = pd.defenseBuffLevel;
		p.skills.frontSlashLevel = pd.frontSlashLevel;
		p.skills.iceBoltSpikeLevel = pd.iceBoltSpikeLevel;

		if (p == player1)
			p.playerNo = 1;
		else
			p.playerNo = 2;
	}

	public void Reset() {

		player1Data = null;
		player2Data = null;
		player1 = null;
		player2 = null;

		twoPlayers = false;
	}
}
