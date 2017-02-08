using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	// Use this for initialization
	public float shakeTime = 0.5f;
	public float shakePower = 0.2f;
	private float shakeTimer;
	private float shakeAmount;
	void Start () {
	
	}
	
	// Update is called once per frame


	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			ShakeCamera (shakePower, shakeTime);
		}

		if (shakeTimer >= 0) 
		{
			Vector3 ShakePos = Random.insideUnitSphere * shakeAmount;

			transform.position = new Vector3 (transform.position.x + ShakePos.x, transform.position.y, transform.position.z+ ShakePos.z);

			shakeTimer -= Time.deltaTime;

		}
	
	}


	public void ShakeCamera(float shakeAmt, float shakeTime)
	{
		shakeAmount = shakeAmt;
		shakeTimer = shakeTime;
	}

}