using UnityEngine;
using System.Collections;

public class fire_pillar : Spell {

    // Use this for initialization
    public float lifeSpan = 2.5f;
	public float triggerDelay;
    void Start()
    {
		damage = 20;

    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
		triggerDelay -= Time.deltaTime;

		if (triggerDelay <= 0 && GetComponent<Collider>().enabled == false)
			GetComponent<Collider> ().enabled = true;

        if (lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }

	void OnTriggerEnter(Collider other) {
		if (other.transform.GetComponent<Enemy> ()) {
			GetDamage ();
			other.GetComponent<Enemy> ().ReceiveDamage (damage, player);
		}

	}
}
