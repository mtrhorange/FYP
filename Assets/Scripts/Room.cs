using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{   
    public Transform spawnPoint1, spawnPoint2;
    public GameObject[] doors, Objects;
    System.Random ran = new System.Random();

	// Use this for initialization
	void Start () {
		//to find all spawn point
        GameObject[] spons = GameObject.FindGameObjectsWithTag("ObjectSpawnPoint");
        //to obtain all the spawn point used
        List<int> SpawnedNum = new List<int>();
        //to get the set amount of objects 
        int SetOfObjects = ran.Next(1, spons.Length);

        for (int i = 0; i < SetOfObjects; i++)
        {
            int one;
            //to keep doing till a new spawn point is obtained 
            do
            {
                 one = ran.Next(1, spons.Length);
            }
            while (SpawnedNum.Contains(one));
            SpawnedNum.Add(one);
            // to instantiate the prefab
            GameObject obj = (GameObject)Instantiate(Objects[ran.Next(0,Objects.Length)], spons[one].transform.position, Objects[ran.Next(0, Objects.Length)].transform.rotation);
            obj.transform.SetParent(this.gameObject.transform,true);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnAdjRooms() {
		

	}
}