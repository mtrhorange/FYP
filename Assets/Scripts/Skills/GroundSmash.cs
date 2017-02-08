using UnityEngine;
using System.Collections;

public class GroundSmash : Spell {

	// Use this for initialization
	public float lifespan;
	public GameObject Sword;
	private bool SpawnedOnce = false;

	public float colliderTimer = 1.0f;
	bool collided = false;

	void Start () {

	}

	// Update is called once per frame
	void Update() {
		lifespan -= Time.deltaTime;
		if (lifespan <= 0) {
			Destroy(this.gameObject);
		}
		if (!SpawnedOnce){
			Instantiate(Sword, this.transform.position , this.transform.rotation);
			SpawnedOnce = true;
		}

		if (GetComponent<SphereCollider> ().enabled)
			GetComponent<SphereCollider> ().enabled = false;

		colliderTimer -= Time.deltaTime;
		if (colliderTimer <= 0 && !collided) {
			GetComponent<SphereCollider> ().enabled = true;
			collided = true;
			Camera cam = FindObjectOfType<Camera> ();
			cam.GetComponent<CameraShake> ().ShakeCamera (0.3f, 0.2f);

		}
	}

	void OnTriggerEnter(Collider other) {

		if (other.GetComponent<Enemy> () && other.GetType () == typeof(CapsuleCollider)) {

			float dmg = GetDamage ();
			dmg *= 1.5f;
			other.GetComponent<Enemy> ().ReceiveDamage (dmg, player);

		}

	}
}
