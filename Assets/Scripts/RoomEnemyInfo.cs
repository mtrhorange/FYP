using UnityEngine;
using System.Collections;

public class RoomEnemyInfo : MonoBehaviour {
    
    //room capacity size
    public int RoomEnemStr = 10;
    public bool IsBossRoom = false;
	

    void Start()
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Debug.Log(spawns.Length);
        AIManager.instance.NewRoom(RoomEnemStr, IsBossRoom, spawns);
    }

}
