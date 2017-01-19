using UnityEngine;
using System.Collections;

public class Ice_Spike : MonoBehaviour {

    // Use this for initialization
    public GameObject Ice_spike;
    private float Ice_spike_lifespan = 2f;
    private float lifespan = 3f;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ice_spike_lifespan -= Time.deltaTime;
        lifespan -= Time.deltaTime;
        if (Ice_spike_lifespan <= 0)
        {
            Destroy(Ice_spike);
        }
        if (lifespan <= 0)
        {
            Destroy(this.gameObject);
        }
    }

	void OnTriggerEnter(Collider other) {

        if (other.GetComponent<Enemy>() && other.GetType() == typeof(CapsuleCollider))
        {
			bool hit = true;
			foreach (Enemy enemy in transform.parent.GetComponent<IceSpikeSpell>().enemiesHit) {
				if (other.GetComponent<Enemy> () == enemy) {

					hit = false;

				}
			}
			if (hit == true) {
				transform.parent.GetComponent<IceSpikeSpell> ().EnemyHit (other.GetComponent<Enemy> ());

			}
		}
	}

}
