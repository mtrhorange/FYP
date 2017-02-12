using UnityEngine;
using System.Collections;

public class SpearBreaker : Spell {

	// Use this for initialization
	public GameObject Ice, Rocks, spawnPoint1, spawnPoint2;
	Camera maincam;
	public float spawnTime = 0.05f;
	public GameObject Aura1, Aura2, Smoke, End, Splatter, EndSmoke;

	private Color32 fire = new Color32 (130, 0, 0,255);
	private Color32 ice = new Color32 (0, 153, 161,255);
	private Color32 poison = new Color32 (3, 107, 0,255);
	private Color32 iceaura = new Color32 (0, 110, 255, 200);
	private Color32 poisonaura = new Color32( 4,180,78,255);
	private GameObject rocks1, rocks2;
	private bool spawned = false;
	private int state;
	private float lifespan = 3;

	float colliderSpan = 0.3f;
	void Start () {
		state = Random.Range (0, 3);// 0 = ice 1 = fire 2 = poison
		maincam = FindObjectOfType<Camera>();
	}

	// Update is called once per frame
	void Update () {
	
		spawnTime -= Time.deltaTime;
		lifespan -= Time.deltaTime;
		if (spawnTime <= 0 && !spawned) {

			if (state == 0) {
				spawn (state);
			} else if (state == 1) {
				spawn (state);
			} else if (state == 2) {
				spawn (state);
			}

		}
			
		if (lifespan <= 0) 
		{
			spawnend (state);
			Destroy (this.gameObject);
		}

		if (GetComponent<BoxCollider> ().enabled) {
			colliderSpan -= Time.deltaTime;
			if (colliderSpan < 0)
				GetComponent<BoxCollider> ().enabled = false;

		}
	}

	private void spawn(int statecheck)
	{
		if (statecheck == 0) { 
			rocks1 = Instantiate (Ice, spawnPoint1.transform.position, spawnPoint1.transform.rotation) as GameObject;

			rocks2 = Instantiate (Ice, spawnPoint2.transform.position, spawnPoint2.transform.rotation) as GameObject;

			GetComponentInChildren<Renderer> ().material.color = ice;
			Aura1.GetComponent<ParticleSystem> ().startColor = iceaura;
			Aura2.GetComponent<ParticleSystem> ().startColor = iceaura;
			Smoke.GetComponent<ParticleSystem> ().startColor = ice;
		} else if (statecheck == 1 || statecheck == 2) 
		{
			rocks1 = Instantiate (Rocks, spawnPoint1.transform.position, spawnPoint1.transform.rotation) as GameObject;
			rocks2 = Instantiate (Rocks, spawnPoint2.transform.position, spawnPoint2.transform.rotation) as GameObject;

			if (statecheck == 1) {
				GetComponentInChildren<Renderer> ().material.color = fire;
				Aura1.GetComponent<ParticleSystem> ().startColor = Color.white;
				Aura2.GetComponent<ParticleSystem> ().startColor = Color.white;
				Smoke.GetComponent<ParticleSystem> ().startColor = fire;
			} else if (statecheck == 2) 
			{
				GetComponentInChildren<Renderer> ().material.color = poison;
				Aura1.GetComponent<ParticleSystem> ().startColor = poisonaura;
				Aura2.GetComponent<ParticleSystem> ().startColor = poisonaura;
				Smoke.GetComponent<ParticleSystem> ().startColor = poison;
			}
		}
		rocks1.transform.parent = this.transform;
		rocks2.transform.parent = this.transform;
		rocks1.transform.localScale = spawnPoint1.transform.localScale;
		rocks2.transform.localScale = spawnPoint2.transform.localScale;
		maincam.GetComponent<CameraShake> ().ShakeCamera (0.4f, 0.7f);
		spawned = true;

		GetComponent<BoxCollider> ().enabled = true;

	}

	private void spawnend(int statecheck)
	{
		maincam.GetComponent<CameraShake> ().ShakeCamera (0.25f, 0.3f);
		Instantiate (End, this.transform.position, this.transform.rotation);
		/*if (statecheck == 0) {
			EndSmoke.GetComponent<ParticleSystem> ().startColor = ice;
			Splatter.GetComponent<ParticleSystem> ().startColor = ice;
		} else if (statecheck == 1) {
			EndSmoke.GetComponent<ParticleSystem> ().startColor = fire;
			Splatter.GetComponent<ParticleSystem> ().startColor = fire;
		} else if (statecheck == 2) {
			EndSmoke.GetComponent<ParticleSystem> ().startColor = poison;
			Splatter.GetComponent<ParticleSystem> ().startColor = poison;*/
	}

	void OnTriggerEnter(Collider other) {

		if (other.GetComponent<Enemy> () && other.GetType () == typeof(CapsuleCollider)) {

			float dmg = GetDamage ();
			dmg *= 2.5f;

			other.GetComponent<Enemy> ().ReceiveDamage (dmg, player);
		}

	}
}
