using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{   
    public Transform spawnPoint1, spawnPoint2;
    public GameObject[] doors, Objects;
    public bool trailActive = false;
    private float trailTimer = 0.5f;
    public List<GameObject> spawnedObjects;

    // Use this for initialization
    void Start () {
		//to find all spawn point
        List<GameObject> spons = new List<GameObject>();

        spawnedObjects = new List<GameObject>();

        spons.AddRange(GameObject.FindGameObjectsWithTag("ObjectSpawnPoint"));

        GameObject[] hehehe = GameObject.FindGameObjectsWithTag("RoomObjects");

        if (spons.Count > 0)
        {
            //decide how many to spawn (1 - max number of spawn points)
            int setOfObjects = Random.Range(1, spons.Count);
            int numSpawned = 0;

            do
            {
                //Spawn the object
                int spIndex = Random.Range(0, spons.Count);
                GameObject obj = (GameObject)Instantiate(Objects[Random.Range(0, Objects.Length)], spons[spIndex].transform.position + transform.up + transform.up, Quaternion.Euler(-90f, 0, 0));
                obj.transform.SetParent(this.gameObject.transform);
                spons.RemoveAt(spIndex);
                spawnedObjects.Add(obj);
                numSpawned++;
            }
            while (numSpawned < setOfObjects);
        }

        if (Floor.instance.currentTheme == Floor.Themes.Cave)
        {
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 0.18f;
        }
        else if (Floor.instance.currentTheme == Floor.Themes.Castle || Floor.instance.currentTheme == Floor.Themes.Hell)
        {
            GameObject.Find("Directional Light").GetComponent<Light>().intensity = 0.3f;
        }

        foreach (GameObject gg in hehehe)
        {
            if (spawnedObjects.Contains(gg))
            {
                Destroy(gg);
            }
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

                GameObject Trale = (GameObject)Instantiate((GameObject)Resources.Load("GuideTrail"), spawnHere, GameManager.instance.player1.transform.rotation);
                //set the start and the end points for the trail to fly
                Trale.GetComponent<GuideTrail>().start = spawnHere;
                Vector3 closestDor = doors[0].transform.position;
                foreach (GameObject dor in doors)
                {
                    if ((dor.transform.position - GameManager.instance.player1.transform.position).magnitude < (closestDor - GameManager.instance.player1.transform.position).magnitude)
                    {
                        closestDor = dor.transform.position;
                    }
                }
                Trale.GetComponent<GuideTrail>().end = closestDor;

                trailTimer = 2f;
            }
            else
            {
                trailTimer -= Time.deltaTime;
            }
        }
	}

    //destroy items
    public void DestroyLingeringItems()
    {
        //destroy all spawned objects
        GameObject[] items = GameObject.FindGameObjectsWithTag("RoomObjects");

        for (int i = items.Length - 1; i >= 0; i--)
        {
            Destroy(items[i]);
        }

        //remove any remaining health orbs
        HealthOrb[] HO = FindObjectsOfType<HealthOrb>();
        for (int i = HO.Length - 1; i >= 0; i--)
        {
            Destroy(HO[i].gameObject);
        }
    }
}