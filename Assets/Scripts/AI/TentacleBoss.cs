﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TentacleBoss : Enemy {
    //tentacle prefab
    public bool spawn;
    public float SpawnX = 4f, SpawnZ = 4f;
    public GameObject tentaclePref;
    //list to keep track of tentacles
    private List<Tentacle> tentacles;
    private bool attacking;
    private float spawnTimer = 5f;


    //Start
    protected override void Start()
    {
        base.Start();
        //Tentacle Boss properties
        health = 500;
        damage = 12;

        //tentacles
        tentacles = new List<Tentacle>();
        tentacles.AddRange(GetComponentsInChildren<Tentacle>());

        //targetting style
        tgtStyle = targetStyle.ClosestPlayer;
        player = base.reacquireTgt(tgtStyle, this.gameObject);
	}
	
	//Update
	void Update ()
    {
        if (spawn)
        {
            spawnTentacle();
            spawn = false;
        }

        if (myState == States.Idle)
        {
            Idle();
        }
        else if (myState == States.Attack)
        {
            Attack();
        }

        spawnTimer -= Time.deltaTime;
    }

    //Spawn tentacle
    void spawnTentacle()
    {
        //spawn a tentacle within a given area of of the player.
        float randomX = Random.Range(player.transform.position.x + SpawnX, player.transform.position.x - SpawnX);
        float randomZ = Random.Range(player.transform.position.z + SpawnZ, player.transform.position.z - SpawnZ);
        Vector3 spawnLocation = new Vector3(randomX, 1.4f, randomZ);

        GameObject temp = (GameObject) Instantiate(tentaclePref, spawnLocation, Quaternion.identity);
        temp.GetComponent<Tentacle>().Boss = this;
        tentacles.Add(temp.GetComponent<Tentacle>());

        //reacquire closer target
        player = base.reacquireTgt(tgtStyle, this.gameObject);
    }

    protected override void Idle()
    {
        //look at player
        if (Vector3.Distance(this.transform.position, player.transform.position) < 10f)
        {
            Vector3 sight = (player.transform.position - transform.position);
            sight.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(sight);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
        }

        //spawn tentacles (maximum of 6) when spawn timer is up
        if (Vector3.Distance(transform.position, player.transform.position) < 15f)
        {
            if (spawnTimer <= 0)
            {
                if (tentacles.Count < 6)
                {
                    spawnTentacle();
                }
                spawnTimer = 8f;
            }
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            myState = States.Attack;
            attacking = true;
        }
    }

    //Attack
    protected override void Attack()
    {
        Vector3 sight = (player.transform.position - transform.position);
        sight.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(sight);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);

        if (Vector3.Distance(transform.position, player.transform.position) > 5f)
        {

            myState = States.Idle;
            attacking = false;
        }
    }

    //tentacle death
    public void TentacleDeath(Tentacle me)
    {
        tentacles.Remove(me);
        Destroy(me.gameObject);
    }

    public void OnDrawGizmos()
    {
        if (attacking)
        {
            Gizmos.color = new Color32(0, 255, 0, 90);
            Gizmos.DrawSphere(this.transform.position, 5f);
        }
    }
}
