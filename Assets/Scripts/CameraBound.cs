using UnityEngine;
using System.Collections;

public class CameraBound : MonoBehaviour {

	public enum Sides {Left, Right, Up, Down};
	public Sides side;

	PlayerCamera playerCamera;
	// Use this for initialization
	void Start () {
		playerCamera = GameObject.FindObjectOfType<PlayerCamera>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision other) {

		if (other.transform.GetComponent<Player> ()) {
			switch (side) {

			case Sides.Down:
				playerCamera.canUp = false;
				break;
			case Sides.Up:
				playerCamera.canDown = false;
				break;
			case Sides.Left:
				playerCamera.canRight = false;
				break;
			case Sides.Right:
				playerCamera.canLeft = false;
				break;
			}
		}
	}

	void OnCollisionExit(Collision other) {

		if (other.transform.GetComponent<Player> ()) {
			switch (side) {

			case Sides.Down:
				playerCamera.canUp = true;
				break;
			case Sides.Up:
				playerCamera.canDown = true;
				break;
			case Sides.Left:
				playerCamera.canRight = true;
				break;
			case Sides.Right:
				playerCamera.canLeft = true;
				break;
			}
		}

	}

	void OnTriggerExit(Collider other) {
		
		if (other.tag == "RoomBound") {

			switch (side) {

			case Sides.Down:
				playerCamera.canDown = true;
				break;
			case Sides.Up:
				playerCamera.canUp = true;
				break;
			case Sides.Left:
				playerCamera.canRight = true;
				break;
			case Sides.Right:
				playerCamera.canLeft = true;
				break;
			}

		}

	}

	void OnTriggerStay(Collider other) {

		if (other.tag == "RoomBound") {

			switch (side) {

			case Sides.Down:
				playerCamera.canDown = false;
				break;
			case Sides.Up:
				playerCamera.canUp = false;
				break;
			case Sides.Left:
				playerCamera.canRight = false;
				break;
			case Sides.Right:
				playerCamera.canLeft = false;
				break;
			}

		}


	}
}
