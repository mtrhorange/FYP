using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Room currentRoom;

	public Transform playerSpawn;

	public enum Directions {Up, Down, Left, Right};
	public Directions direction;

	public bool canExit = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {

		if (other.GetComponent<Player> () && canExit) {

			Floor.instance.NextRoom ();



		}

	}

}
