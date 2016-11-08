﻿using UnityEngine;
using System.Collections;

public class EnemyProjectiles : MonoBehaviour {

    //prefabs
    public GameObject poisonPool;

    //different types of projectiles
    public enum type
    {
        AcidSpit, //(flower monster, poison D.o.T)
        WebShot,
        DragonBreath,
        FireBlast
    }
    public type projectileType;

    //particle systems instantiated from this projectile (if any)
    private GameObject leftBehinds;
    //life time
    public bool hasLifeSpan = true;
    public float timeOut = 12f;

    //damage values
    public float damage = 0f;
    private bool p1Hit = false, p2Hit = false;

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update ()
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
        //if has lifespan
        else if (hasLifeSpan)
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
                    p1Hit = false;
                    p2Hit = false;
                }
            }
            else if (GetComponentInChildren<ParticleSystem>())
            {
                if (!GetComponentInChildren<ParticleSystem>().IsAlive(true))
                {
                    this.gameObject.SetActive(false);
                    p1Hit = false;
                    p2Hit = false;
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
                    GetComponent<ParticleSystem>().Stop();
                    GetComponent<MeshRenderer>().enabled = false;
                }
                break;
            //if type is web shot
            case type.WebShot:
                //if hit player, slowdown player
                if (other.gameObject.tag == "Player")
                {
                    //TODO: make player slow
                    Debug.Log("ZJI ZJI ALEDY");
                    Destroy(this.gameObject);
                }
                //if hit environment, destroy
                else if (other.gameObject.layer == 8)
                {
                    Destroy(this.gameObject);
                }
                break;
            //if type is dragon breath
            case type.DragonBreath:
                //damage
                if (other.gameObject.tag == "Player")
                {
                    //only hit each player once
                    if (!p1Hit && other.gameObject.GetComponent<Player>().playerNo == 1)
                    {
                        p1Hit = true;
                        other.gameObject.GetComponent<Player>().ReceiveDamage(damage);
                    }
                    if (!p2Hit && other.gameObject.GetComponent<Player>().playerNo == 2)
                    {
                        p2Hit = true;
                        other.gameObject.GetComponent<Player>().ReceiveDamage(damage);
                    }
                }
                break;
        }
    }
}
