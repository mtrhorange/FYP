using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public Door nextDoor;
	Room currentRoom;

	public enum Directions {Up, Down, Left, Right};
	public Directions direction;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
