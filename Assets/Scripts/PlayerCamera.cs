﻿using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	public Transform player;

	Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = transform.position - player.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = player.position + offset;
	}
}
