using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	void Awake() {
		DontDestroyOnLoad (transform.gameObject);
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
