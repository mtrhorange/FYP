using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

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
