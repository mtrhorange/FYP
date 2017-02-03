﻿using UnityEngine;
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
        UpdatePlayer1();

        playerModel.name = "Player1";
        playerModel.transform.Find("p1").gameObject.SetActive(true);

        if (player2Data.saveId != 0)
        {
            GameObject player2Model =
                (GameObject)Instantiate(playerPrefab,
                    spawnPos2, Quaternion.identity);
            player2 = player2Model.GetComponent<Player>();
            UpdatePlayer2();

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
		pd.spellStaminaDrain = p.spellStaminaDrain;

		pd.skillCType = p.skillCType;
		pd.skillVType = p.skillVType;

		pd.firePillarLevel = p.skills.firePillarLevel;
		pd.iceSpikesLevel = p.skills.iceSpikesLevel;
		pd.chainLightningLevel = p.skills.chainLightningLevel;

		pd.maxHealthLevel = p.skills.maxHealthLevel;
		pd.minDmgLevel = p.skills.minDmgLevel;
		pd.maxDmgLevel = p.skills.maxDmgLevel;
		pd.weaponBuffLevel = p.skills.weaponBuffLevel;
		pd.spellBuffLevel = p.skills.spellBuffLevel;

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
		p.maxStamina = pd.maxStamina;
		p.baseMaxStamina = pd.baseMaxStamina;
		p.Stamina = p.MaxStamina;
		p.Lives = pd.Lives;
		p.SkillPoints = pd.SkillPoints;
		p.spellStaminaDrain = pd.spellStaminaDrain;
		p.recoverStamina = true;

		p.skillCType = pd.skillCType;
		p.skillVType = pd.skillVType;

		p.skills.firePillarLevel = pd.firePillarLevel;
		p.skills.iceSpikesLevel = pd.iceSpikesLevel;
		p.skills.chainLightningLevel = pd.chainLightningLevel;

		p.skills.maxHealthLevel = pd.maxHealthLevel;
		p.skills.minDmgLevel = pd.minDmgLevel;
		p.skills.maxDmgLevel = pd.maxDmgLevel;
		p.skills.weaponBuffLevel = pd.weaponBuffLevel;
		p.skills.spellBuffLevel = pd.spellBuffLevel;

		if (p == player1)
			p.playerNo = 1;
		else
			p.playerNo = 2;
	}

	public void UpdatePlayer1Data() {

		player1Data.name = player1.name;
		player1Data.saveId = player1.saveId;
		player1Data.Level = player1.Level;
		player1Data.Exp = player1.Exp;
		player1Data.MaxHealth = player1.MaxHealth;
		player1Data.baseMaxHealth = player1.baseMaxHealth;
		player1Data.Health = player1.Health;
		player1Data.maxStamina = player1.maxStamina;
		player1Data.baseMaxStamina = player1.baseMaxStamina;
		player1Data.Stamina = player1.MaxStamina;
		player1Data.Lives = player1.Lives;
		player1Data.SkillPoints = player1.SkillPoints;
		player1Data.spellStaminaDrain = player1.spellStaminaDrain;

		player1Data.skillCType = player1.skillCType;
		player1Data.skillVType = player1.skillVType;

		player1Data.firePillarLevel = player1.skills.firePillarLevel;
		player1Data.iceSpikesLevel = player1.skills.iceSpikesLevel;
		player1Data.chainLightningLevel = player1.skills.chainLightningLevel;

		player1Data.maxHealthLevel = player1.skills.maxHealthLevel;
		player1Data.minDmgLevel = player1.skills.minDmgLevel;
		player1Data.maxDmgLevel = player1.skills.maxDmgLevel;
		player1Data.weaponBuffLevel = player1.skills.weaponBuffLevel;
		player1Data.spellBuffLevel = player1.skills.spellBuffLevel;

		for (int i = 0; i < SaveLoad.savedCharacters.Count; i++) {
			if (player1Data.saveId == SaveLoad.savedCharacters [i].saveId) {
				SaveLoad.savedCharacters.RemoveAt (i);
			}
		}

		SaveLoad.NewCharacter (player1Data);

	}

	public void UpdatePlayer1() {
		player1.name = player1Data.name;
		player1.saveId = player1Data.saveId;
		player1.Level = player1Data.Level;
		player1.Exp = player1Data.Exp;
		player1.MaxHealth = player1Data.MaxHealth;
		player1.baseMaxHealth = player1Data.baseMaxHealth;
		player1.Health = player1Data.Health;
		player1.maxStamina = player1Data.maxStamina;
		player1.baseMaxStamina = player1Data.baseMaxStamina;
		player1.Stamina = player1.MaxStamina;
		player1.Lives = player1Data.Lives;
		player1.SkillPoints = player1Data.SkillPoints;
		player1.spellStaminaDrain = player1Data.spellStaminaDrain;
		player1.recoverStamina = true;

		player1.skillCType = player1Data.skillCType;
		player1.skillVType = player1Data.skillVType;

		player1.skills.firePillarLevel = player1Data.firePillarLevel;
		player1.skills.iceSpikesLevel = player1Data.iceSpikesLevel;
		player1.skills.chainLightningLevel = player1Data.chainLightningLevel;

		player1.skills.maxHealthLevel = player1Data.maxHealthLevel;
		player1.skills.minDmgLevel = player1Data.minDmgLevel;
		player1.skills.maxDmgLevel = player1Data.maxDmgLevel;
		player1.skills.weaponBuffLevel = player1Data.weaponBuffLevel;
		player1.skills.spellBuffLevel = player1Data.spellBuffLevel;

		player1.playerNo = 1;
	}

	public void UpdatePlayer2Data() {

		player2Data.name = player2.name;
		player2Data.saveId = player2.saveId;
		player2Data.Level = player2.Level;
		player2Data.Exp = player2.Exp;
		player2Data.MaxHealth = player2.MaxHealth;
		player2Data.baseMaxHealth = player2.baseMaxHealth;
		player2Data.Health = player2.Health;
		player2Data.maxStamina = player2.maxStamina;
		player2Data.baseMaxStamina = player2.baseMaxStamina;
		player2Data.Stamina = player2.MaxStamina;
		player2Data.Lives = player2.Lives;
		player2Data.SkillPoints = player2.SkillPoints;
		player2Data.spellStaminaDrain = player2.spellStaminaDrain;

		player2Data.skillCType = player2.skillCType;
		player2Data.skillVType = player2.skillVType;

		player2Data.maxHealthLevel = player2.skills.maxHealthLevel;
		player2Data.minDmgLevel = player2.skills.minDmgLevel;
		player2Data.maxDmgLevel = player2.skills.maxDmgLevel;
		player2Data.weaponBuffLevel = player2.skills.weaponBuffLevel;
		player2Data.spellBuffLevel = player2.skills.spellBuffLevel;

		for (int i = 0; i < SaveLoad.savedCharacters.Count; i++) {
			if (player2Data.saveId == SaveLoad.savedCharacters [i].saveId) {
				SaveLoad.savedCharacters.RemoveAt (i);
			}
		}

		SaveLoad.NewCharacter (player2Data);
	}

	public void UpdatePlayer2() {
		player2.name = player2Data.name;
		player2.saveId = player2Data.saveId;
		player2.Level = player2Data.Level;
		player2.Exp = player2Data.Exp;
		player2.MaxHealth = player2Data.MaxHealth;
		player2.baseMaxHealth = player2Data.baseMaxHealth;
		player2.Health = player2Data.Health;
		player2.maxStamina = player2Data.maxStamina;
		player2.baseMaxStamina = player2Data.baseMaxStamina;
		player2.Stamina = player2.MaxStamina;
		player2.Lives = player2Data.Lives;
		player2.SkillPoints = player2Data.SkillPoints;
		player2.spellStaminaDrain = player2Data.spellStaminaDrain;
		player2.recoverStamina = true;

		player2.skillCType = player2Data.skillCType;
		player2.skillVType = player2Data.skillVType;

		player2.skills.maxHealthLevel = player2Data.maxHealthLevel;
		player2.skills.minDmgLevel = player2Data.minDmgLevel;
		player2.skills.maxDmgLevel = player2Data.maxDmgLevel;
		player2.skills.weaponBuffLevel = player2Data.weaponBuffLevel;
		player2.skills.spellBuffLevel = player2Data.spellBuffLevel;

		player2.playerNo = 2;
	}

	public void Reset() {

		player1Data = null;
		player2Data = null;
		player1 = null;
		player2 = null;

		twoPlayers = false;

	}
}
