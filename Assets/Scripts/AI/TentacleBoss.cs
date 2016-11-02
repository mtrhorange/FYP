using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TentacleBoss : Enemy {
    //tentacle prefab
    public GameObject tentaclePref;
    public float tentacleSpawnAreaX = 4f, tentacleSpawnAreaY = 4f;
    //list to keep track of tentacles
    private List<Tentacle> tentacles;

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
        
	}
}
