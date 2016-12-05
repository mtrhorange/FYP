﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//mob types
public enum mobType
{
    Slime,
    SlimeBig,
    Goblin,
    SkeletalWarrior,
    Zombie,
    MaskedOrc,
    Plant,
    Flower,
    Mushroom,
    CatBat,
    Hornet,
    Bug,
    Magma,
    Dragon,
    DragonUndead,
    Tentacle,
    TentacleBoss,
    DragonBoss,
    SlimeKing,
    LichBoss,
    TreantGuard
}

public class AIManager : MonoBehaviour
{
    //public instance
    public static AIManager instance;

    public List<GameObject> mobPrefabs, weakGuys, medGuys, strongGuys, bossGuys;
    private List<GameObject> bossPrefabs;
    private int roomEnemyPoints; //total enemy points in current room
    public List<GameObject> enemyList;
    public List<GameObject> roomSpawnPoints;
    private const int WEAK = 1, MEDIUM = 2, STRONG = 4; //enemy points each strength category is worth
    private List<int> mobPrefStrengths, spawnTheseStrengths;
    private bool spawning = false, isBossRoom = false;


    //Awake
    void Awake()
    {
        instance = this;
    }

    //Start
    void Start()
    {
        mobPrefabs = new List<GameObject>();
        weakGuys = new List<GameObject>();
        medGuys = new List<GameObject>();
        strongGuys = new List<GameObject>();
        bossPrefabs = new List<GameObject>();
        mobPrefStrengths = new List<int>();
        spawnTheseStrengths = new List<int>();
        roomSpawnPoints = new List<GameObject>();

        mobPrefabs.Add((GameObject)Resources.Load("Enemies/BugMonster"));
        mobPrefStrengths.Add(WEAK);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Cat Bat"));
        mobPrefStrengths.Add(WEAK);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Dragon"));
        mobPrefStrengths.Add(STRONG);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Dragon Undead"));
        mobPrefStrengths.Add(STRONG);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/FlowerMonster"));
        mobPrefStrengths.Add(MEDIUM);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Hornet"));
        mobPrefStrengths.Add(WEAK);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Magma Demon"));
        mobPrefStrengths.Add(MEDIUM);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Masked Orc"));
        mobPrefStrengths.Add(MEDIUM);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Mushroom Monster"));
        mobPrefStrengths.Add(WEAK);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Plant Monster"));
        mobPrefStrengths.Add(MEDIUM);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Slime(Animated)"));
        mobPrefStrengths.Add(WEAK);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Slime(AnimatedBigger)"));
        mobPrefStrengths.Add(MEDIUM);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/ZombieClothes"));
        mobPrefStrengths.Add(WEAK);

        mobLists();

        bossPrefabs.Add((GameObject)Resources.Load("Enemies/Dragon Boss"));
        bossPrefabs.Add((GameObject)Resources.Load("Enemies/Tentacle Monster-Yellow"));

        bossGuys = new List<GameObject>(bossPrefabs);

        Debug.Log(bossGuys.Count);

        enemyList = new List<GameObject>();
        enemyList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        //testing
        //mobType temp = mobType.Bug;
    }

    //Update
    void Update()
    {
        //spawn if spawning lul
        if (spawning)
        {
            fillUpRoom();
        }
    }

    //spawn specific mob, given type of mob, and vector position
    public void spawnMob(mobType e, Vector3 l)
    {
        switch (e)
        {
            case mobType.Bug:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[0], l, Quaternion.identity));
                break;
            case mobType.CatBat:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[1], l, Quaternion.identity));
                break;
            case mobType.Dragon:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[2], l, Quaternion.identity));
                break;
            case mobType.DragonUndead:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[3], l, Quaternion.identity));
                break;
            case mobType.Flower:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[4], l, Quaternion.identity));
                break;
            case mobType.Goblin:
                break;
            case mobType.Slime:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[10], l, Quaternion.identity));
                break;
            case mobType.SlimeBig:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[11], l, Quaternion.identity));
                break;
            case mobType.Magma:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[6], l, Quaternion.identity));
                break;
            case mobType.MaskedOrc:
                break;
            case mobType.Mushroom:
                break;
            case mobType.Plant:
                break;
            case mobType.SkeletalWarrior:
                break;
            case mobType.Zombie:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[12], l, Quaternion.identity));
                break;
        }
        //set the mobtype
        enemyList[enemyList.Count - 1].GetComponent<Enemy>().myType = e;
        FlockingManager.instance.UpdateAgentArray();
    }

    //spawn specific boss, given type and vector position
    public void spawnBoss(mobType e, Vector3 l)
    {
        switch (e)
        {
            case mobType.DragonBoss:
                enemyList.Add((GameObject)Instantiate(bossPrefabs[0], l, Quaternion.identity));
                break;
            case mobType.LichBoss:
                break;
            case mobType.SlimeKing:
                break;
            case mobType.TentacleBoss:
                enemyList.Add((GameObject)Instantiate(bossPrefabs[1], l, Quaternion.identity));
                break;
            case mobType.TreantGuard:
                break;
        }
        //set the mobtype
        enemyList[enemyList.Count - 1].GetComponent<Enemy>().myType = e;
        FlockingManager.instance.UpdateAgentArray();
    }

    //New Room, reset values. Called by the room when it spawns
    public void NewRoom(int pts, bool bos, List<GameObject> spns)
    {
        //clear the enemy list
        enemyList.Clear();
        enemyList.TrimExcess();
        //clear room spawn points list
        roomSpawnPoints.Clear();
        roomSpawnPoints.TrimExcess();

        //get the room points
        roomEnemyPoints = pts;
        //get the is boss boolean
        isBossRoom = bos;
        //get the spawn points
        roomSpawnPoints = spns;

        //recalculate A*
        GetComponent<AstarPath>().Scan();

        //set the spawning to true
        spawning = true;
    }

    //fill up room
    public void fillUpRoom()
    {
        //first check if its boss room
        if (isBossRoom /*check if boss room)*/)
        {
            //spawn boss
            spawnNextBoss(roomSpawnPoints[0].transform.position);
        }
        else
        {
            spawnTheseStrengths.Clear();
            spawnTheseStrengths.TrimExcess();
            //check which strngth categories we can spawn from
            if (roomEnemyPoints >= WEAK)
            {
                spawnTheseStrengths.Add(WEAK);
                if (roomEnemyPoints >= MEDIUM)
                {
                    spawnTheseStrengths.Add(MEDIUM);
                    if (roomEnemyPoints >= STRONG)
                    {
                        spawnTheseStrengths.Add(STRONG);
                    }
                }

                //SPAWN, improvements: check spawn points and spread dem out and not overlap if possible
                spawnRandomMob(spawnTheseStrengths[UnityEngine.Random.Range(0, spawnTheseStrengths.Count)], roomSpawnPoints[UnityEngine.Random.Range(0, roomSpawnPoints.Count)].transform.position);
            }
        }
    }

    //spawn random mob with given strength
    public void spawnRandomMob(int str, Vector3 loc)
    {
        switch (str)
        {
            case WEAK:
                enemyList.Add((GameObject)Instantiate(weakGuys[UnityEngine.Random.Range(0, weakGuys.Count)], loc, Quaternion.identity));
                break;
            case MEDIUM:
                enemyList.Add((GameObject)Instantiate(medGuys[UnityEngine.Random.Range(0, medGuys.Count)], loc, Quaternion.identity));
                break;
            case STRONG:
                enemyList.Add((GameObject)Instantiate(strongGuys[UnityEngine.Random.Range(0, strongGuys.Count)], loc, Quaternion.identity));
                break;
        }

        //shift spawnpoints
        roomSpawnPoints = Shift(roomSpawnPoints);

        //deduct the points from roomEnemyPoints
        roomEnemyPoints -= str;
        if (roomEnemyPoints <= 0) { spawning = false; }
    }

    //spawn next boss in rotation
    public void spawnNextBoss(Vector3 l)
    {
        enemyList.Add((GameObject)Instantiate(bossGuys[0], l, Quaternion.identity));
        FlockingManager.instance.UpdateAgentArray();

        //shift boss prefabs
        bossGuys = Shift(bossGuys);

        //once spawned set spawning to false
        spawning = false;
    }

    //setup mob lists based on strength
    public void mobLists()
    {
        for (int i = 0; i < mobPrefabs.Count; i++)
        {
            //weak
            if (mobPrefStrengths[i] == WEAK)
            {
                weakGuys.Add(mobPrefabs[i]);
            }
            //medium
            if (mobPrefStrengths[i] == MEDIUM)
            {
                medGuys.Add(mobPrefabs[i]);
            }
            //strong
            if (mobPrefStrengths[i] == STRONG)
            {
                strongGuys.Add(mobPrefabs[i]);
            }
        }
    }

    //Remove enemy
    public void RemoveMe(GameObject me)
    {
        enemyList.Remove(me);
        FlockingManager.instance.UpdateAgentArray();
    }

    //shift array
    private GameObject[] Shift(GameObject[] myArray)
    {
        GameObject[] temp = new GameObject[myArray.Length];
        for (int i = 0; i < myArray.Length; i++)
        {
            if (i < myArray.Length - 1)
                temp[i] = myArray[i + 1];
            else
                temp[i] = myArray[0];
        }
        return temp;
    }

    //shift list
    private List<GameObject> Shift(List<GameObject> myArray)
    {
        List<GameObject> temp = new List<GameObject>();
        for (int i = 0; i < myArray.Count; i++)
        {
            if (i < myArray.Count - 1)
                temp.Add(myArray[i + 1]);
            else
                temp.Add(myArray[0]);
        }
        return temp;
    }
}