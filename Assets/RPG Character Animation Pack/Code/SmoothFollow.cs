﻿using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	GameObject cameraTarget;
	public float smoothTime = 0.1f;
	Vector2 velocity;
	public float offsetX = -15f;
	public float offsetY = -15f;
	private Transform thisTransform;

	void Start()
	{
		cameraTarget = GameObject.FindGameObjectWithTag("Player");
		thisTransform = transform;
	}

	void Update()
	{
		thisTransform.position = new Vector3(Mathf.SmoothDamp(thisTransform.position.x, cameraTarget.transform.position.x, ref velocity.x, smoothTime), Mathf.SmoothDamp(thisTransform.position.y, cameraTarget.transform.position.y - offsetY, ref velocity.y, smoothTime * 2), (cameraTarget.transform.position.z + offsetX));
	}
}