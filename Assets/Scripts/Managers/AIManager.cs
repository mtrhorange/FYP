using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum enemyType
{
    Slime,
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
    Dragon
}
public class AIManager : MonoBehaviour {

    //public instance
    public static AIManager instance;

    //PUBLIC
    public List<GameObject> enemyPrefabs;

    //PRIVATE
    private List<GameObject> enemyList;

    //awake
    void Awake()
    {

        instance = this;

    }

    // Use this for initialization
    void Start () {

        enemyList = new List<GameObject>();
        enemyPrefabs = new List<GameObject>();

        enemyList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void spawnMob(enemyType e, Vector3 l)
    {
        switch (e)
        {
            case enemyType.Bug:
                break;
            case enemyType.CatBat:
                break;
            case enemyType.Dragon:
                break;
            case enemyType.Flower:
                break;
            case enemyType.Goblin:
                break;
            case enemyType.Slime:
                break;
            case enemyType.Magma:
                break;
            case enemyType.MaskedOrc:
                break;
            case enemyType.Mushroom:
                break;
            case enemyType.Plant:
                break;
            case enemyType.SkeletalWarrior:
                break;
            case enemyType.Zombie:
                break;
        }
    }
}
