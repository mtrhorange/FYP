using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	Room[,] rooms = new Room[10,10];

	// Use this for initialization
	void Start () {
		rooms [0, 0] = new Room ();
	}

	// Update is called once per frame
	void Update () {
	
	}

}
