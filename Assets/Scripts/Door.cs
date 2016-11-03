using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Door nextDoor;
	public Room currentRoom;

	public Transform playerSpawn;

	public enum Directions {Up, Down, Left, Right};
	public Directions direction;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {

		if (other.GetComponent<Player> ()) {

			nextDoor.currentRoom.gameObject.SetActive (true);



		}

	}

}
