using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class demo1 : MonoBehaviour {
    public List<Transform> spawnPoints = new List<Transform>();
    public int count = 0;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if(count <= 7){
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AIManager.instance.spawnMob(mobType.Zombie,spawnPoints[count % 4].position);
                count ++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AIManager.instance.spawnMob(mobType.Bug,spawnPoints[count % 4].position);
                count ++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AIManager.instance.spawnMob(mobType.Flower,spawnPoints[count % 4].position);
                count ++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AIManager.instance.spawnMob(mobType.SlimeBig,spawnPoints[count % 4].position);
                count ++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                AIManager.instance.spawnMob(mobType.Dragon,spawnPoints[count % 4].position);
                count ++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                AIManager.instance.spawnMob(mobType.DragonUndead,spawnPoints[count % 4].position);
                count ++;
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                AIManager.instance.spawnBoss(mobType.DragonBoss,spawnPoints[count % 4].position);
                count ++;
            }
        }

	}
}
