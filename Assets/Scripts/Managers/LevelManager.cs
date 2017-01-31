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
		SceneManager.LoadScene(2); //Load base camp scene
		//SceneManager.activeSceneChanged += SpawnPlayer; //Methods to call after scene changes

	}

	public void LoadMainMenu() {

		SceneManager.LoadScene ("MainMenu");
		GameManager.instance.Reset ();
	}

	void SpawnPlayer(Scene a, Scene b) {
		Floor.instance.NewRoom ();
		Floor.instance.SpawnNextRoom ();
		GameManager.instance.SpawnPlayer ();
		SceneManager.activeSceneChanged -= SpawnPlayer;
	}

	public void ChangeRoom(Door nextDoor) {



	}

	public void QuitGame() {

		Application.Quit ();

	}
}
