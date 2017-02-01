using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{   
    public Transform spawnPoint1, spawnPoint2;
    public GameObject[] doors, Objects;
    System.Random ran = new System.Random();

    public bool trailActive = false;
    private float trailTimer = 0.5f;

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

        if (Floor.instance.currentTheme == Floor.Themes.Cave)
        {
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 0.18f;
        }
        else if (Floor.instance.currentTheme == Floor.Themes.Castle || Floor.instance.currentTheme == Floor.Themes.Hell)
        {
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 0.3f;
        }


    }
	
	// Update is called once per frame
	void Update () {
		if (trailActive)
        {
            if (trailTimer <= 0)
            {
                //spawn new trail
                Vector3 spawnHere = new Vector3(GameManager.instance.player1.transform.position.x, GameManager.instance.player1.transform.position.y + 0.5f, GameManager.instance.player1.transform.position.z);

                GameObject Trale = (GameObject)Instantiate((GameObject)Resources.Load("GuideTrail"), spawnHere, Quaternion.identity);
                //set the start and the end points for the trail to fly
                Trale.GetComponent<GuideTrail>().start = spawnHere;
                Trale.GetComponent<GuideTrail>().end = doors[0].transform.position;

                trailTimer = 2f;
            }
            else
            {
                trailTimer -= Time.deltaTime;
            }
        }
	}

	public void SpawnAdjRooms() {
		

	}
}