using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomEnemyInfo : MonoBehaviour {
    
    //room capacity size
    public int RoomEnemStr = 10;
    public bool IsBossRoom = false;
	

    void Start()
    {
        List<GameObject> spawns = new List<GameObject>();
        GameObject[] spons = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach(GameObject gg in spons)
        {
            if (!AIManager.instance.roomSpawnPoints.Contains(gg))
            {
                spawns.Add(gg);
            }
        }

        AIManager.instance.NewRoom(RoomEnemStr, IsBossRoom, spawns);
    }

}
