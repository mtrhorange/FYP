using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour {

	public static Floor instance;

	int floorLevel = 1;
	int roomsToBoss = 6;
	public GameObject currentRoom, nextRoom;

	List<GameObject> caveRooms;

	bool changingRooms = false;
	bool fadeOut = true;
	float fadeTime = 0f;
	GameObject blackOverlay;

	void Awake() {
		instance = this;

	}

	// Use this for initialization
	void Start () {
		caveRooms = new List<GameObject> ();
		caveRooms.Add((GameObject)Resources.Load("Rooms/Cave/caveRoom2"));
		caveRooms.Add((GameObject)Resources.Load("Rooms/Cave/caveRoom3"));
		caveRooms.Add((GameObject)Resources.Load("Rooms/Cave/caveRoom4"));

		blackOverlay = GameObject.Find ("Canvas").transform.Find ("BlackOverlay").gameObject;
		NewRoom ();
		SpawnNextRoom ();
		GameManager.instance.SpawnPlayer ();
	}

	// Update is called once per frame
	void Update () {

		if (changingRooms) {
			if (fadeOut) {
				fadeTime += Time.deltaTime;

				//Camera Black overlay fade in
				blackOverlay.GetComponent<Image> ().color = new Color (0, 0, 0, fadeTime);

				if (fadeTime > 1) {
					SpawnNextRoom ();
					MovePlayers ();
					GameManager.instance.SavePlayers ();
					fadeOut = false;
					fadeTime = 1;
				}
			} else {
				fadeTime -= Time.deltaTime;

				//Camera black overlay fade out
				blackOverlay.GetComponent<Image> ().color = new Color (0, 0, 0, fadeTime);

				if (fadeTime < 0) {
					fadeOut = true;
					fadeTime = 0;
					changingRooms = false;
				}


			}

		}

	}

	public void NewRoom() {

		GameObject room;
		if (currentRoom != null) {
			
			do {
				int rand = Random.Range (0, caveRooms.Count);
				nextRoom = caveRooms [rand];
			} while (nextRoom.name == currentRoom.name);

		} else {
			int rand = Random.Range (0, caveRooms.Count);
			nextRoom = caveRooms [rand];
		}

	}

	public void NextRoom() {
		
		changingRooms = true;


	}

	public void SpawnNextRoom() {
		if (currentRoom != null)
			Destroy (currentRoom);

		GameObject room = (GameObject)Instantiate (nextRoom, Vector3.zero, nextRoom.transform.rotation);
		room.name = nextRoom.name;
		currentRoom = room;
		NewRoom ();



	}

	public void MovePlayers() {

		GameManager.instance.player1.transform.position = currentRoom.GetComponent<Room> ().spawnPoint1.position;
		GameManager.instance.player1.transform.rotation = currentRoom.GetComponent<Room> ().spawnPoint1.rotation;
		if (GameManager.instance.twoPlayers) {
			GameManager.instance.player2.transform.position = currentRoom.GetComponent<Room> ().spawnPoint2.position;
			GameManager.instance.player2.transform.rotation = currentRoom.GetComponent<Room> ().spawnPoint2.rotation;
		}

	}




}
