using UnityEngine;
using System.Collections;

public class EnemyProjectiles : MonoBehaviour {

    //prefabs
    public GameObject poisonPool, stickyArea;

    //different types of projectiles
    public enum type
    {
        AcidSpit,
        WebShot,
        DragonBreathFire,
        DragonBreathPoison,
        FireBlast,
        NormalShot,
        SkullMissile,
        fireArrow
    }
    public type projectileType;

    //particle systems instantiated from this projectile (if any)
    private GameObject leftBehinds;
    //life time
    public bool hasLifeSpan = true;
    public float timeOut = 12f;

    public Vector3 target;

    //damage values
    public float damage = 0f;

	//Start
	void Start()
    {
	}

	//Update
	void Update()
    {
        //if this projectile left something behind (e.g. poison pool, flames, ice spikes?)
        //remove particle system when done then remove this
        if (leftBehinds)
        {
            if (!leftBehinds.GetComponent<ParticleSystem>().IsAlive(true))
            {
                Destroy(leftBehinds);
                Destroy(this.gameObject);
            }
        }
        //if has a target to explode at
        else if (target != Vector3.zero)
        {
            //explode if close enough
            if ((target - transform.position).magnitude <= 0.5f)
            {
                if (leftBehinds)
                {
                    leftBehinds =
                        (GameObject) Instantiate(stickyArea, transform.position, Quaternion.Euler(-90f, 0f, 0f));
                    leftBehinds.GetComponent<EnemyLeftBehinds>().dmg = 0;
                    leftBehinds.GetComponent<EnemyLeftBehinds>().typ = projectileType;
                    GetComponent<SphereCollider>().enabled = false;
                    GetComponent<ParticleSystem>().Stop();
                    GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        //if has lifespan
        if (hasLifeSpan)
        {
            //once life time is over
            if (timeOut <= 0)
            {
                Destroy(this.gameObject);
            }
            timeOut -= Time.deltaTime;
        }
        //if does not have lifespan
        else if (!hasLifeSpan)
        {
            //set object inactive after particle (if any) is done
            if (GetComponent<ParticleSystem>())
            {
                if (!GetComponent<ParticleSystem>().IsAlive(true))
                {
                    this.gameObject.SetActive(false);
                }
            }
            else if (GetComponentInChildren<ParticleSystem>())
            {
                if (!GetComponentInChildren<ParticleSystem>().IsAlive(true))
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
	}

    //trigger enter
    void OnTriggerEnter(Collider other)
    {
        switch (projectileType)
        {
            //if type is acid spit
            case type.AcidSpit:
                //if it hits the walls or ground (layers 8 and 9 respectively)
                if (other.gameObject.layer == 8)
                {
                    //drop to the ground
                    GetComponent<Rigidbody>().velocity *= -0.1f;
                    GetComponent<Rigidbody>().velocity += new Vector3(0, -2, 0);
                }
                else if (other.gameObject.layer == 9)
                {
                    //pool on the ground, poisons player if stepped on
                    leftBehinds = (GameObject)Instantiate(poisonPool, transform.position, Quaternion.Euler(-90f, 0f, 0f));
                    leftBehinds.GetComponent<EnemyLeftBehinds>().dmg = damage;
                    leftBehinds.GetComponent<EnemyLeftBehinds>().typ = projectileType;
                    GetComponent<SphereCollider>().enabled = false;
                    transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
                    transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
                }
                break;
            //if type is web shot
            case type.WebShot:
                //if hit environment (obstacle layer), destroy
                if (other.gameObject.layer == 8)
                {
                    Destroy(this.gameObject);
                }
                break;
            //if is normal shot (tentacle boss)
            case type.NormalShot:
                if (other.gameObject.tag == "Player")
                {
                    other.GetComponent<Player>().ReceiveDamage(damage);
                    Destroy(gameObject);
                }
                //if hit environment (obstacle layer), destroy
                if (other.gameObject.layer == 8)
                {
                    Destroy(this.gameObject);
                }
                break;
            //if is fire arrow
            case type.fireArrow:
                if (other.gameObject.tag == "Player")
                {
                    other.GetComponent<Player>().ReceiveDamage(damage);
                    Destroy(gameObject);
                }
                //if hit environment (obstacle layer), destroy
                if (other.gameObject.layer == 8)
                {
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    //trigger stay
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (projectileType)
            {
                //dragon breath fire
                case type.DragonBreathFire:
                    //call player's burn function
                    other.GetComponent<Player>().ApplyStrongBurn(5f);
                    break;
                //fire blast
                case type.FireBlast:
                    //call player's burn function
                    other.GetComponent<Player>().ApplyBurn(5f);
                    break;
                //dragon breath poison
                case type.DragonBreathPoison:
                    //call player's poison function
                    other.GetComponent<Player>().ApplyStrongPoison(5f);
                    break;
            }
        }
    }
}
