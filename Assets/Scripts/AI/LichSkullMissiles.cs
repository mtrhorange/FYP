using UnityEngine;
using System.Collections;

public class LichSkullMissiles : MonoBehaviour {


    public GameObject chaseThis, skullExplode;
    public float lifeTime = 5f, speed = 500f, damage = 0;
    private GameObject explosion;
    private bool exploded = false;

	//Start
	void Start ()
    {

	}
	
	//Update
	void Update ()
    {
        //explode once lifetime is over
        if (lifeTime <= 0f && !exploded)
        {
            exploded = true;
            explosion = (GameObject)Instantiate(skullExplode, transform.position, Quaternion.Euler(-90, 0, 0));
            explosion.GetComponent<EnemyLeftBehinds>().dmg = damage;
            GetComponent<ParticleSystem>().Stop();
            GetComponentInChildren<Light>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }

        //once explosion finishes playing, destroy
        if (explosion)
        {
            if (explosion.GetComponent<ParticleSystem>().time / explosion.GetComponent<ParticleSystem>().duration >= 0.5f)
            {
                explosion.GetComponent<BoxCollider>().enabled = false;
            }
            if (!explosion.GetComponent<ParticleSystem>().IsAlive(true))
            {
                Destroy(explosion);
                Destroy(this.gameObject);
            }
        }
        //keep moving while haven't exploded
        else
        {
            //slerp to face target
            Quaternion lookTgt = Quaternion.LookRotation(chaseThis.transform.position + chaseThis.transform.up - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookTgt, Time.deltaTime * 1.25f);

            //move forwards
            GetComponent<Rigidbody>().velocity = transform.forward * speed * Time.deltaTime;

            lifeTime -= Time.deltaTime;
        }
	}

    //trigger enter
    void OnTriggerEnter(Collider other)
    {
        //if the contacted object is not an enemy or another skull, explode
        if (!exploded && !other.gameObject.GetComponent<Enemy>() && !other.gameObject.GetComponent<LichSkullMissiles>() && !other.gameObject.GetComponent<EnemyLeftBehinds>())
        {
            exploded = true;
            explosion = (GameObject)Instantiate(skullExplode, transform.position, Quaternion.Euler(-90, 0, 0));
            explosion.GetComponent<EnemyLeftBehinds>().dmg = damage;
            GetComponent<ParticleSystem>().Stop();
            GetComponentInChildren<Light>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }
}
