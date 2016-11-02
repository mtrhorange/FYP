using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TentacleBoss : Enemy {
    //tentacle prefab
    public bool spawn;
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
        float randomX = Random.Range(player.transform.position.x + 2f, player.transform.position.x - 2f);
        float randomZ = Random.Range(player.transform.position.z + 2f, player.transform.position.z - 2f);
        Vector3 spawnLocation = new Vector3(randomX, 0, randomZ);

        GameObject temp = (GameObject) Instantiate(tentaclePref, spawnLocation, Quaternion.identity);
        temp.SetActive(true);

    }
}
