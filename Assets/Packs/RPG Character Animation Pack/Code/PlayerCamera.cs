using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
	public GameObject cameraTarget1, cameraTarget2;
	public float smoothTime = 0.1f;
	Vector2 velocity;
	public float offsetZ = -5f;
	public float offsetY = -15f;
	private Transform thisTransform;

	public bool canUp = true, canDown = true, canLeft = true, canRight = true;
	Vector3 targetPos;

	void Start()
	{
		cameraTarget1 = GameObject.Find ("Player1");
		cameraTarget2 = GameObject.Find ("Player2");
		thisTransform = transform;

	}

	void Update()
	{
		if (cameraTarget2 == null)
			thisTransform.position = new Vector3 (Mathf.SmoothDamp (thisTransform.position.x, cameraTarget1.transform.position.x, ref velocity.x, smoothTime), Mathf.SmoothDamp (thisTransform.position.y, cameraTarget1.transform.position.y - offsetY, ref velocity.y, smoothTime * 2), cameraTarget1.transform.position.z + offsetZ);
		else {
			//thisTransform.position = new Vector3 (Mathf.SmoothDamp (thisTransform.position.x, Vector3.Lerp (cameraTarget1.transform.position, cameraTarget2.transform.position, 0.5f).x, ref velocity.x, smoothTime), Mathf.SmoothDamp (thisTransform.position.y, cameraTarget1.transform.position.y - offsetY, ref velocity.y, smoothTime * 2), (Vector3.Lerp (cameraTarget1.transform.position, cameraTarget2.transform.position, 0.5f).z + offsetZ));
		
			targetPos = Vector3.Lerp (cameraTarget1.transform.position, cameraTarget2.transform.position, 0.5f);

			if (transform.position.x < targetPos.x && canRight)
				transform.position = new Vector3 (Mathf.SmoothDamp (transform.position.x, targetPos.x, ref velocity.x, smoothTime), Mathf.SmoothDamp (thisTransform.position.y, cameraTarget1.transform.position.y - offsetY, ref velocity.y, smoothTime * 2), transform.position.z);
			else if (transform.position.x > targetPos.x && canLeft)
				transform.position = new Vector3 (Mathf.SmoothDamp (transform.position.x, targetPos.x, ref velocity.x, smoothTime), Mathf.SmoothDamp (thisTransform.position.y, cameraTarget1.transform.position.y - offsetY, ref velocity.y, smoothTime * 2), transform.position.z);

			if (transform.position.z < targetPos.z && canUp)
				transform.position = new Vector3 (transform.position.x, Mathf.SmoothDamp (thisTransform.position.y, cameraTarget1.transform.position.y - offsetY, ref velocity.y, smoothTime * 2), targetPos.z + offsetZ);
			else if (transform.position.z > targetPos.z && canDown)
				transform.position = new Vector3 (transform.position.x, Mathf.SmoothDamp (thisTransform.position.y, cameraTarget1.transform.position.y - offsetY, ref velocity.y, smoothTime * 2), targetPos.z + offsetZ);

		}
		
	}
}