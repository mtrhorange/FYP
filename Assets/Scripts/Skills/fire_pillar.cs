using UnityEngine;
using System.Collections;

public class fire_pillar : Spell {

    // Use this for initialization
    public float lifeSpan = 2.5f;
	public float triggerDelay;
    void Start()
    {
		int lvl = player.skills.firePillarLevel;
		int rad = lvl < 15 ? Mathf.CeilToInt (((float)lvl / 5f)) : 3;
		int rate = lvl < 15 ? 100 * Mathf.CeilToInt ((float)(lvl / 5f)) : 500;
		ParticleSystem.ShapeModule shapeMod = transform.Find ("Fire").GetComponent<ParticleSystem> ().shape;
		ParticleSystem.EmissionModule emiMod = transform.Find ("Fire").GetComponent<ParticleSystem> ().emission;
		shapeMod.radius = rad;
		emiMod.rate = rate;
		GetComponent<CapsuleCollider> ().radius = rad;

    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
		triggerDelay -= Time.deltaTime;

        if (triggerDelay <= 0 && GetComponent<Collider>().enabled == false)
        {
            SFXManager.instance.playSFX(sounds.firePillar);
            GetComponent<Collider>().enabled = true;
        }

        if (lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }

	void OnTriggerEnter(Collider other) {
        if (other.transform.GetComponent<Enemy>() && other.GetType() == typeof(CapsuleCollider))
        {
			GetDamage ();
			int lvl = player.skills.firePillarLevel;
			damage *= 0.8f + lvl * 0.05f;
			other.GetComponent<Enemy> ().ReceiveDamage (damage, player);
		}
	}
}
