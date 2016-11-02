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
    

	//Start
	void Start ()
    {
        //Tentacle Boss properties
        health = 500;
        damage = 12;
        //player reference
        player = GameObject.FindGameObjectWithTag("Player");

        //tentacles
        tentacles = new List<Tentacle>();
        tentacles.AddRange(GetComponentsInChildren<Tentacle>());

	}
	
	//Update
	void Update ()
    {
        if (spawn == true)
        {
            spawnTentacle();
            spawn = false;
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
        temp.transform.parent = transform;

    }
}
