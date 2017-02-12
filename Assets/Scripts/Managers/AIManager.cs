using UnityEngine;
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
    SkeletalArcher,
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
    LichBoss,
    TreantGuard
}

public class AIManager : MonoBehaviour
{
    //public instance
    public static AIManager instance;
    private List<GameObject> mobPrefabs, weakGuys, medGuys, strongGuys, bossGuys;
    private List<GameObject> bossPrefabs;
    public int roomEnemyPoints; //total enemy points in current room
    public List<GameObject> enemyList;
    public List<GameObject> roomSpawnPoints;
    private const int WEAK = 1, MEDIUM = 2, STRONG = 4; //enemy points each strength category is worth
    private List<int> mobPrefStrengths, spawnTheseStrengths;
    private bool spawning = false, isBossRoom = false;
    public float doorCheckTimer = 3f;
    public int bugCount = 0;

    //temporary var for demonstration purposes
    private int enemPts;

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
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/GoblinWithClothes"));
        mobPrefStrengths.Add(MEDIUM);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/SkeletonArrow"));
        mobPrefStrengths.Add(WEAK);
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/SkeletonMelee"));
        mobPrefStrengths.Add(MEDIUM);

        mobLists();

        bossPrefabs.Add((GameObject)Resources.Load("Enemies/Dragon Boss"));
        bossPrefabs.Add((GameObject)Resources.Load("Enemies/Tentacle Monster-Yellow"));
        bossPrefabs.Add((GameObject)Resources.Load("Enemies/TreantGuard"));
        bossPrefabs.Add((GameObject)Resources.Load("Enemies/LichBoss"));

        bossGuys = new List<GameObject>(bossPrefabs);

        Shuffle(bossGuys);

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

        //check door
        checkDoor();

        //kill all
        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = enemyList.Count - 1; i >= 0; i--)
            {
                enemyList[i].GetComponent<Enemy>().ReceiveDamage(999, null);
            }
            spawning = false;
            roomEnemyPoints = 0;
        }
        //spawn monster
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (roomEnemyPoints <= 0 && enemyList.Count <= 0)
            {
                roomEnemyPoints = enemPts;
            }

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
            case mobType.Hornet:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[5], l, Quaternion.identity));
                break;
            case mobType.Magma:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[6], l, Quaternion.identity));
                break;
            case mobType.MaskedOrc:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[7], l, Quaternion.identity));
                break;
            case mobType.Mushroom:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[8], l, Quaternion.identity));
                break;
            case mobType.Plant:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[9], l, Quaternion.identity));
                break;
            case mobType.Slime:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[10], l, Quaternion.identity));
                break;
            case mobType.SlimeBig:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[11], l, Quaternion.identity));
                break;
            case mobType.Zombie:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[12], l, Quaternion.identity));
                break;
            case mobType.Goblin:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[13], l, Quaternion.identity));
                break;
            case mobType.SkeletalArcher:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[14], l, Quaternion.identity));
                break;
            case mobType.SkeletalWarrior:
                enemyList.Add((GameObject)Instantiate(mobPrefabs[15], l, Quaternion.identity));
                break;
        }
        //set the mobtype
        enemyList[enemyList.Count - 1].GetComponent<Enemy>().myType = e;
        FlockingManager.instance.UpdateAgentArray(enemyList);
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
                enemyList.Add((GameObject)Instantiate(bossPrefabs[3], l, Quaternion.identity));
                break;
            case mobType.TentacleBoss:
                enemyList.Add((GameObject)Instantiate(bossPrefabs[1], l, Quaternion.identity));
                break;
            case mobType.TreantGuard:
                enemyList.Add((GameObject)Instantiate(bossPrefabs[2], l, Quaternion.identity));
                break;
        }
        //set the mobtype
        enemyList[enemyList.Count - 1].GetComponent<Enemy>().myType = e;
        FlockingManager.instance.UpdateAgentArray(enemyList);
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

        enemPts = pts;

        //get the is boss boolean
        isBossRoom = bos;
        //get the spawn points
        roomSpawnPoints = spns;

        //recalculate A*
        StartCoroutine(UpdateGraph());

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
            //also check level requirements
            int highestLevel = 0;
            if (GameManager.instance.twoPlayers)
            {
                highestLevel = GameManager.instance.player1.Level >= GameManager.instance.player2.Level ? GameManager.instance.player1.Level : GameManager.instance.player2.Level;
            }
            else { highestLevel = GameManager.instance.player1.Level; }

            if (roomEnemyPoints >= WEAK)
            {
                //lazy weighted randoms LOL ezez
                spawnTheseStrengths.Add(WEAK);
                spawnTheseStrengths.Add(WEAK);
                spawnTheseStrengths.Add(WEAK);
                if (roomEnemyPoints >= MEDIUM && highestLevel >= 5)
                {
                    spawnTheseStrengths.Add(MEDIUM);
                    spawnTheseStrengths.Add(MEDIUM);
                    if (roomEnemyPoints >= STRONG && highestLevel >= 10)
                    {
                        spawnTheseStrengths.Add(STRONG);
                    }
                }

                //SPAWN
                spawnRandomMob(spawnTheseStrengths[UnityEngine.Random.Range(0, spawnTheseStrengths.Count)], roomSpawnPoints[0].transform.position);
            }
        }
    }

    //spawn random mob with given strength
    public void spawnRandomMob(int str, Vector3 loc)
    {
        //check if the spawnpoint has stuff. Layermasks ignores ground and camera bound colliders respectively
        if (!Physics.CheckCapsule(loc, loc + Vector3.up, 0.5f, ~((1 << 9) | (1 << 10))))
        {
            switch (str)
            {
                case WEAK:
                    GameObject d;
                    if (bugCount >= 2)
                    {
                        do
                        {
                            d = weakGuys[UnityEngine.Random.Range(0, weakGuys.Count)];
                        }
                        while (d.GetComponent<Enemy>().myType == mobType.Bug);
                    }
                    else
                    {
                        d = weakGuys[UnityEngine.Random.Range(0, weakGuys.Count)];
                    }

                    enemyList.Add((GameObject)Instantiate(d, loc, Quaternion.identity));
                    break;
                case MEDIUM:
                    enemyList.Add((GameObject)Instantiate(medGuys[UnityEngine.Random.Range(0, medGuys.Count)], loc, Quaternion.identity));
                    break;
                case STRONG:
                    enemyList.Add((GameObject)Instantiate(strongGuys[UnityEngine.Random.Range(0, strongGuys.Count)], loc, Quaternion.identity));
                    break;
            }

            FlockingManager.instance.UpdateAgentArray(enemyList);

            //deduct the points from roomEnemyPoints
            roomEnemyPoints -= str;
            if (roomEnemyPoints <= 0) { spawning = false; }
        }

        //shift spawnpoints
        roomSpawnPoints = Shift(roomSpawnPoints);
    }

    //spawn next boss in rotation
    public void spawnNextBoss(Vector3 l)
    {
        enemyList.Add((GameObject)Instantiate(bossGuys[0], l, Quaternion.identity));
        FlockingManager.instance.UpdateAgentArray(enemyList);

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
        FlockingManager.instance.UpdateAgentArray(enemyList);
        //when no more enemies left, door is activated
        if (enemyList.Count < 1 && roomEnemyPoints < 1)
        {
            //get floor manager > current room > door > enable exit
            foreach (GameObject g in Floor.instance.currentRoom.GetComponent<Room>().doors)
            {
                g.GetComponent<Door>().canExit = true;
                if (g.GetComponent<Door>().mesh == true)
                {
                    g.GetComponent<MeshRenderer>().enabled = false;
                    g.GetComponent<BoxCollider>().enabled = false;
                }
                Floor.instance.currentRoom.GetComponent<Room>().trailActive = true;
            }
        }
    }

    //check door
    private void checkDoor()
    {
        //check every 3 seconds
        if (doorCheckTimer < 0)
        {
            //check while door canExit is false
            if (Floor.instance.currentRoom.GetComponent<Room>().doors[0].GetComponent<Door>().canExit == false)
            {
                if (enemyList.Count < 1 && roomEnemyPoints < 1)
                {
                    //get floor manager > current room > door > enable exit
                    foreach (GameObject g in Floor.instance.currentRoom.GetComponent<Room>().doors)
                    {
                        g.GetComponent<Door>().canExit = true;
                        if (g.GetComponent<Door>().mesh == true)
                        {
                            g.GetComponent<MeshRenderer>().enabled = false;
                            g.GetComponent<BoxCollider>().enabled = false;
                        }
                        Floor.instance.currentRoom.GetComponent<Room>().trailActive = true;
                    }
                }
            }
            doorCheckTimer = 3f;
        }
        else
        {
            doorCheckTimer -= Time.deltaTime;
        }
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

    //shuffle
    private void Shuffle(List<GameObject> list)
    {
	    for (int i = list.Count - 1; i > 0; i--)
	    {
            int n = UnityEngine.Random.Range(0, i + 1);
		    GameObject temp = list[i];
		    list[i] = list[n];
		    list[n] = temp;
	    }
    }

    //update A* graph
    private IEnumerator UpdateGraph()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<AstarPath>().Scan();
    }
}