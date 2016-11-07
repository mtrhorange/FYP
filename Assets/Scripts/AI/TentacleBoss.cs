using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

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
    void Start ()
    {
        //Tentacle Boss properties
        health = 500;
        damage = 12;

        //tentacles
        tentacles = new List<Tentacle>();
        tentacles.AddRange(GetComponentsInChildren<Tentacle>());

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
    }

    void spawnTentacle()
    {
        //spawn a tentacle within a given area of of the player.
        float randomX = Random.Range(player.transform.position.x + SpawnX, player.transform.position.x - SpawnX);
        float randomZ = Random.Range(player.transform.position.z + SpawnZ, player.transform.position.z - SpawnZ);
        Vector3 spawnLocation = new Vector3(randomX, 1.4f, randomZ);

        GameObject temp = (GameObject) Instantiate(tentaclePref, spawnLocation, Quaternion.identity);
        temp.SetActive(true);

    }

    protected override void Idle()
    {
        //TODO: LEPAK
        if (Vector3.Distance(this.transform.position, player.transform.position) < 10f)
        {
            Vector3 sight = (player.transform.position - transform.position);
            sight.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(sight);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 15f)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                spawnTentacle();
                spawnTimer = 8f;
            }
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            myState = States.Attack;
            attacking = true;
        }

        


    }

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

    public void OnDrawGizmos()
    {
        if (attacking)
        {
            Gizmos.color = new Color32(0, 255, 0, 90);
            Gizmos.DrawSphere(this.transform.position, 5f);
        }
    }
}
