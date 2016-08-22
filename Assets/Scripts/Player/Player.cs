using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	List<Vector3> movements;

	bool recording;
	bool reverse;

	public int reverseSpeed = 5;

	public float health = 100;
	public float damageMin = 10;
	public float damageMax = 13;



	GameObject attackTrigger;
	GameObject enemyTargetHover;

	// Use this for initialization
	void Start () {
		movements = new List<Vector3> ();
		recording = true;

		attackTrigger = transform.Find ("AttackTrigger").gameObject;
		enemyTargetHover = transform.Find ("Target").gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		/*Movement ();

		if (Input.GetKey (KeyCode.Q)) {
			reverse = true;
			recording = false;
			ReverseTime();
		} else if (Input.GetKeyUp (KeyCode.Q)) {
			reverse = false;
			recording = true;
		}*/



	}

	public void AttackTrigger(int i)
	{
		if (i == 1)
			attackTrigger.SetActive (true);
		else
			attackTrigger.SetActive (false);

	}

	public GameObject FindTarget() {

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		GameObject closest = null;
		foreach (GameObject g in enemies) {
			if (closest == null)
				closest = g;
			else if (Vector3.Distance (transform.position, closest.transform.position) > Vector3.Distance (transform.position, g.transform.position))
				closest = g;


		}

		if (closest != null) {
			enemyTargetHover.SetActive (true);
			enemyTargetHover.transform.parent = closest.transform;
			enemyTargetHover.transform.position = new Vector3 (closest.transform.position.x, enemyTargetHover.transform.position.y, closest.transform.position.z);

			return closest;
		} else
			return null;


	}

	public void ResetTarget() {

		enemyTargetHover.SetActive (false);
		enemyTargetHover.transform.parent = transform;

	}

	void ReverseTime() {


			
		int i = movements.Count;
		if (i - reverseSpeed >= 0) {
			transform.position = movements[i-reverseSpeed];
			movements.RemoveRange(i-reverseSpeed, reverseSpeed);

		}else if (i > 0) {
			transform.position = movements [i - 1];
			movements.RemoveAt (i - 1);
		}

	}


	void Movement() {

		if (!reverse) {

			if (Input.GetKey(KeyCode.W)) {
				transform.position += Vector3.forward * 0.1f;
				recording = true;
			}
			if (Input.GetKey (KeyCode.S)) {
				transform.position += Vector3.back * 0.1f;
				recording = true;
			}
			if (Input.GetKey (KeyCode.A)) {
				transform.position += Vector3.left * 0.1f;
				recording = true;
			}
			if (Input.GetKey (KeyCode.D)) {
				transform.position += Vector3.right * 0.1f;
				recording = true;
			}

			if (recording) {
				
				movements.Add(transform.position);
				Debug.Log (movements.Count);
				if (movements.Count > 50)
					movements.RemoveAt(0);
				recording = false;
			}
//			if (Input.GetKeyDown (KeyCode.Space)){
//				recording = !recording;
//				movements.Clear();
//			}
		}
	}
}
