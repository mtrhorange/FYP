using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	List<Player> playerCharacters;

	void Awake() {
		DontDestroyOnLoad (transform.gameObject);
		instance = this;
	}

	// Use this for initialization
	void Start () {
		playerCharacters = new List<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
