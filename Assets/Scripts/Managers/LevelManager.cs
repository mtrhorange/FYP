using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	void Awake() {
		DontDestroyOnLoad (transform.gameObject);
		if (instance && instance != this.gameObject)
			Destroy (this.gameObject);
		else
			instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadGame() {
		
		SceneManager.LoadScene ("Scene1"); //Load game scene
		SceneManager.activeSceneChanged += SpawnPlayer; //Methods to call after scene changes

	}

	public void LoadMainMenu() {

		SceneManager.LoadScene ("MainMenu");

	}

	void SpawnPlayer(Scene a, Scene b) {
		GameManager.instance.SpawnPlayer ();
		SceneManager.activeSceneChanged -= SpawnPlayer;
	}
}
