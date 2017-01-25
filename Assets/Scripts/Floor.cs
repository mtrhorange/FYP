using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour {

	public static Floor instance;

	public int floorLevel = 1;
	public int roomsToBoss = 6;
	public GameObject currentRoom, nextRoom;
    //room prefab lists
	private List<GameObject> caveRooms, castleRooms, hellRooms;
    private List<List<GameObject>> roomTypes;
    //Room Theme
    public enum Themes { Cave, Castle, Hell }
    //the theme currently in use
    public Themes currentTheme;


	bool changingRooms = false;
	bool fadeOut = true;
	float fadeTime = 0f;
	GameObject blackOverlay;

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start() {
		caveRooms = new List<GameObject>();
		caveRooms.Add((GameObject)Resources.Load("Rooms/Cave/caveRoom2"));
		caveRooms.Add((GameObject)Resources.Load("Rooms/Cave/caveRoom3"));
		caveRooms.Add((GameObject)Resources.Load("Rooms/Cave/caveRoom4"));
        caveRooms.Add((GameObject)Resources.Load("Rooms/Cave/caveRoom5"));

        castleRooms = new List<GameObject>();
        castleRooms.Add((GameObject)Resources.Load("Rooms/Castle/castleRoom1"));
        castleRooms.Add((GameObject)Resources.Load("Rooms/Castle/castleRoom2"));
        castleRooms.Add((GameObject)Resources.Load("Rooms/Castle/castleRoom3"));
        castleRooms.Add((GameObject)Resources.Load("Rooms/Castle/castleRoom4"));

        hellRooms = new List<GameObject>();
        hellRooms.Add((GameObject)Resources.Load("Rooms/Hell/hellRoom1"));
        hellRooms.Add((GameObject)Resources.Load("Rooms/Hell/hellRoom2"));
        hellRooms.Add((GameObject)Resources.Load("Rooms/Hell/hellRoom3"));
        hellRooms.Add((GameObject)Resources.Load("Rooms/Hell/hellRoom4"));

        currentTheme = (Themes)Random.Range(0, 3);

        roomTypes = new List<List<GameObject>>();
        roomTypes.Add(caveRooms);
        roomTypes.Add(castleRooms);
        roomTypes.Add(hellRooms);

		blackOverlay = GameObject.Find("Canvas").transform.Find("BlackOverlay").gameObject;
		NewRoom();
		SpawnNextRoom();
		GameManager.instance.SpawnPlayer();
	}

	// Update is called once per frame
	void Update() {

		if (changingRooms) {
			if (fadeOut) {
				fadeTime += Time.deltaTime;

				//Camera Black overlay fade in
				blackOverlay.GetComponent<Image>().color = new Color(0, 0, 0, fadeTime);

				if (fadeTime > 1) {
					SpawnNextRoom();
					MovePlayers();
					GameManager.instance.SavePlayers();
					fadeOut = false;
					fadeTime = 1;
				}
			} else {
				fadeTime -= Time.deltaTime;

				//Camera black overlay fade out
				blackOverlay.GetComponent<Image>().color = new Color(0, 0, 0, fadeTime);

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
		if (currentRoom != null) 
        {
            //check if current room is boss, if yes, cycle the theme for the new rooms
            if (currentRoom.name.ToUpper().Contains("BOSS"))
            {
                Themes TT;
                do
                {
                    TT = currentTheme = (Themes)Random.Range(0, 3);
                }
                while(TT == currentTheme);
                currentTheme = TT;
            }

            //if currently 1 room before boss room
			if (roomsToBoss == 1)
            {
                switch (currentTheme)
                {
                    case Themes.Cave:
                        nextRoom = (GameObject)Resources.Load("Rooms/Cave/caveBossRoom");
                        break;
                    case Themes.Castle:
                        nextRoom = (GameObject)Resources.Load("Rooms/Castle/castleBossRoom");
                        break;
                    case Themes.Hell:
                        nextRoom = (GameObject)Resources.Load("Rooms/Hell/hellBossRoom");
                        break;
                }
            }
            //if not is normal rooms
            else
            {
                do
                {
                    int rand = Random.Range(0, roomTypes[(int)currentTheme].Count);
                    nextRoom = roomTypes[(int)currentTheme][rand];
                } while (nextRoom.name == currentRoom.name);
            }

		} else {
			int rand = Random.Range (0, roomTypes[(int)currentTheme].Count);
            nextRoom = roomTypes[(int)currentTheme][rand];
		}

	}

	public void NextRoom() {
		
		changingRooms = true;
	}

	public void SpawnNextRoom() {
		if (currentRoom != null)
        {
            //check if current room (before setting) is boss, if yes, update floor values
            if (currentRoom.name.ToUpper().Contains("BOSS"))
            {
                NextFloor();
            }

            Destroy(currentRoom);
        }

		GameObject room = (GameObject)Instantiate (nextRoom, Vector3.zero, nextRoom.transform.rotation);
		room.name = nextRoom.name;
		currentRoom = room;

        //decrement rooms to boss counter
        roomsToBoss--;
        Debug.Log("Rooms to boss: " + roomsToBoss);
		NewRoom();

	}

    //Next Floor
    public void NextFloor()
    {
        //increment the floor counter
        floorLevel++;
        //reset the number of rooms before boss room (6~8)
        roomsToBoss = Random.Range((int)6, (int)9);
    }

	public void MovePlayers() {

		GameManager.instance.player1.transform.position = currentRoom.GetComponent<Room>().spawnPoint1.position;
		GameManager.instance.player1.transform.rotation = currentRoom.GetComponent<Room>().spawnPoint1.rotation;
		if (GameManager.instance.twoPlayers) {
			GameManager.instance.player2.transform.position = currentRoom.GetComponent<Room>().spawnPoint2.position;
			GameManager.instance.player2.transform.rotation = currentRoom.GetComponent<Room>().spawnPoint2.rotation;
		}

	}

}
