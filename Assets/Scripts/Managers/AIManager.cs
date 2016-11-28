using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    //PUBLIC
    public List<GameObject> mobPrefabs;
    private List<GameObject> bossPrefabs;
    public GameObject enemySpawnPoint;

    //PRIVATE
    private List<GameObject> enemyList;
    private bool player1 = true;

    //awake
    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        mobPrefabs = new List<GameObject>();
        bossPrefabs = new List<GameObject>();

        mobPrefabs.Add((GameObject)Resources.Load("Enemies/BugMonster"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Cat Bat"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Dragon"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Dragon Undead"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/FlowerMonster"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Hornet"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Magma Demon"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Masked Orc"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Mushroom Monster"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Plant Monster"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Slime(Animated)"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Slime(AnimatedBigger)"));
        mobPrefabs.Add((GameObject)Resources.Load("Enemies/Zombie"));


        bossPrefabs.Add((GameObject)Resources.Load("Enemies/Dragon Boss"));
        bossPrefabs.Add((GameObject)Resources.Load("Enemies/Tentacle Monster-Yellow"));


        enemyList = new List<GameObject>();

        enemyList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        mobType temp = mobType.Bug;
        //spawnBoss(temp, enemySpawnPoint.transform.position);
        spawnMob(temp, enemySpawnPoint.transform.position);

        //mobType temp = mobType.Flower;
        //spawnMob(temp, enemySpawnPoint.transform.position);
        //spawnMob(temp, enemySpawnPoint.transform.position);
        //spawnMob(temp, enemySpawnPoint.transform.position);

        //temp = mobType.Zombie;

        //spawnMob(temp, enemySpawnPoint.transform.position);
        //spawnMob(temp, enemySpawnPoint.transform.position);
        //spawnMob(temp, enemySpawnPoint.transform.position);

    }

    // Update is called once per frame
    void Update()
    {

    }

    //spawn mob, given type of mob, and vector position
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

        //set target
        //enemyList[enemyList.Count - 1].GetComponent<Enemy>().player = player1 ? GameManager.instance.player1.gameObject : GameManager.instance.player2.gameObject;
        //player1 = !player1;
    }

    //spawn boss, given type and vector position
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

    //Remove enemy
    public void RemoveMe(GameObject me)
    {
        enemyList.Remove(me);
        FlockingManager.instance.UpdateAgentArray();
    }
}