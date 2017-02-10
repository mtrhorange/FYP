using UnityEngine;
using System.Collections;
using Pathfinding;

//enemy strength category
public enum Strength
{
    Weak,
    Medium,
    Strong,
    Boss
}

public class Enemy : MonoBehaviour
{
    public float health, maxHealth;
    public GameObject damageText;
    protected int numOfOrbs = 1;
    public float damage;
    public float expValue;
    //target position
    protected Vector3 target;
    //flocking velocity
    public Vector3 velocity;
    //player references
    public GameObject player;
    public Player murderer;
    //seeker component
    public Vector3 nextPathPoint;
    protected Seeker seeker;
    //calculated path
    protected Path path;
    //AI speed
    public float speed;
    //distance AI is to a waypoint for it to continue to the next
    protected float nextWayPointDistance = 1f;
    //current waypoint
    protected int currentWayPoint = 0;
    public float minDistance = 2.0f;
    //wheter they triggered and should chase
    protected bool triggered = false;

    //myStrength (how strong this enemy is)
    public Strength myStrength = Strength.Weak;
    //Monster Level
    public int monsterLevel = 1;

    //States
    public enum States
    {
        Idle,
        Chase,
        Attack,
        Flinch,
        Dead
    }
    //myState (current state this entity is in)
    public States myState = States.Idle;

    //targetting style (method of target acquisition)
    public enum targetStyle
    {
        AssignedPlayer, //Chases its first set target to the ends of the earth
        ClosestPlayer, //Lazy bum that prefers to chase the closer target
        WeakestPlayer, //Bully that prefers to finish off weaker foes first
    }
    //target acquisition method of this entity
    public targetStyle tgtStyle = targetStyle.AssignedPlayer;

    //enemy type
    public mobType myType;

    //Status Effects
    bool isBurning = false;
    bool isFrozen = false;

    //Start
    protected virtual void Start()
    {
        damageText = (GameObject)Resources.Load("DamageText");
        monsterLevel = CalculateLevel();
        health = CalculateHP();
        maxHealth = health;
        damage = CalculateDamage();
        expValue = CalculateExpReward();

        triggered = false;
    }

    //Update
    protected virtual void Update()
    {
        if (myState == States.Dead)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().freezeRotation = true;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    //Idle state
    protected virtual void Idle()
    {
        if (!player)
        {
            return;
        }
        else
        {
            //chase target
            target = player.transform.position;
            //set a path to tgt position
            seeker.StartPath(transform.position, target, OnPathComplete);
            currentWayPoint = 0;
            myState = States.Chase;
        }
    }
    //chase
    protected virtual void Chase()
    {
        Debug.Log("ENEMY SCRIPT Chase");
    }

    protected virtual void Attack()
    {
        Debug.Log("ENEMY SCRIPT Attack");
    }

    //death
    protected virtual void Death()
    {
        myState = States.Dead;
        GetComponent<CapsuleCollider>().enabled = false;
        //reward exp
        if (murderer != null)
            RewardEXP();
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        AIManager.instance.RemoveMe(this.gameObject);
        //Drop health orbs
        DropHealthOrbs();
        Destroy(this.gameObject, 5f);
    }

    //Reward EXP points
    protected void RewardEXP()
    {
        //if 2 player, split 60% to player who killed and 40% to the other
        if (GameManager.instance.twoPlayers)
        {
            murderer.ReceiveExp(expValue * 0.6f);
            if (GameManager.instance.player1 == murderer)
            {
                GameManager.instance.player2.ReceiveExp(expValue * 0.4f);
            }
            else if (GameManager.instance.player2 == murderer)
            {
                GameManager.instance.player1.ReceiveExp(expValue * 0.4f);
            }
        }
        //else, award to the single player
        else
        {
            murderer.ReceiveExp(expValue);
        }
    }

    //receive damage
    public virtual void ReceiveDamage(float dmg, Player attacker)
    {
        if (myState != States.Dead)
        {
            health -= dmg;
            if (health < 0) { health = 0; }
            Camera camera = FindObjectOfType<Camera>();
            Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
            //offset
            Vector3 offset = new Vector3(Random.Range(-30f, 30f), Random.Range(-20f, 20f), 0f);
            GameObject txt = (GameObject)Instantiate(damageText, screenPos + offset, Quaternion.identity);
            txt.transform.SetParent(GameObject.Find("Canvas").transform);
            txt.GetComponent<UnityEngine.UI.Text>().text = dmg.ToString("F0");
            txt.GetComponent<DamageText>().initialOffset = offset;
            txt.GetComponent<DamageText>().target = transform;

            if (health <= 0)
            {
                murderer = attacker;
                Death();
            }
            else
            {
                //Flinch
                if (myState != States.Flinch)
                    Flinch();
            }
        }
    }

    //Flinch
    protected virtual void Flinch()
    {
        myState = States.Flinch;
    }

    //Flinch End Animation Event callback
    public virtual void FlinchEnd()
    {
        myState = States.Chase;
        Debug.Log("enemy.cs flinch end");
    }

    //Calculate Level the monster should be
    protected int CalculateLevel()
    {
        //if two players
        if (GameManager.instance.twoPlayers)
        {
            //Return average level of 2 players
            return (Mathf.CeilToInt((float)(GameManager.instance.player1.Level + GameManager.instance.player2.Level) / 2));
        }
        else
        {
            //else return level of the single player
            return GameManager.instance.player1.Level;
        }
    }

    //Calculate damage to deal
    protected float CalculateDamage()
    {
        float baseDmg = 0, baseMul = 0, levelMul = 0;

        if (myStrength == Strength.Weak)
        {
            baseDmg = 3;
            baseMul = 0.3f;
            levelMul = 0.5f;
        }
        else if (myStrength == Strength.Medium)
        {
            baseDmg = 5;
            baseMul = 0.5f;
            levelMul = 0.6f;
        }
        else if (myStrength == Strength.Strong)
        {
            baseDmg = 8;
            baseMul = 0.75f;
            levelMul = 0.75f;
        }
        else if (myStrength == Strength.Boss)
        {
            baseDmg = 12;
            baseMul = 0.8f;
            levelMul = 0.8f;
        }

        //FORMURA
        //base: 3, 5, 8
        //baseMul: 0.3, 0.5, 0.75
        //levelMul: 0.5, 0.6, 0.75
        //damage = base + (LVL - 1) * baseMul + (LVL / 5) * levelMul

        return baseDmg + ((monsterLevel - 1) * baseMul) + ((monsterLevel / 5) * levelMul);
    }

    //Calculate HP the monster shld have
    protected float CalculateHP()
    {
        float baseHP = 0, baseMul = 0, levelMul = 0;

        if (myStrength == Strength.Weak)
        {
            baseHP = GameManager.instance.twoPlayers ? 8 : 6;
            baseMul = 3.5f;
            levelMul = 1f;
        }
        else if (myStrength == Strength.Medium)
        {
            baseHP = GameManager.instance.twoPlayers ? 20 : 15;
            baseMul = 6.5f;
            levelMul = 3f;
        }
        else if (myStrength == Strength.Strong)
        {
            baseHP = GameManager.instance.twoPlayers ? 30 : 22.5f;
            baseMul = 10.5f;
            levelMul = 5f;
        }
        else if (myStrength == Strength.Boss)
        {
            baseHP = GameManager.instance.twoPlayers ? 250 : 220;
            baseMul = 20f;
            levelMul = 8f;
        }

        //FORMURA
        //base: 8, 20, 30
        //baseMul: 3.5, 6.5, 10.5
        //levelMul: 1, 3, 5
        //HP = base + (LVL - 1) * baseMul + (LVL / 5) * levelMul

        return baseHP + ((monsterLevel - 1) * baseMul) + ((monsterLevel / 5) * levelMul);
    }

    //Calculate EXP to reward
    protected float CalculateExpReward()
    {
        float baseExp = 0;
        if (myStrength == Strength.Weak)
        {
            baseExp = 3;
            numOfOrbs = Random.Range(0, 2);
        }
        else if (myStrength == Strength.Medium)
        {
            baseExp = 5f;
            numOfOrbs = Random.Range(1, 4);
        }
        else if (myStrength == Strength.Strong)
        {
            baseExp = 7;
            numOfOrbs = Random.Range(3, 6);
        }
        else if (myStrength == Strength.Boss)
        {
            baseExp = 30;
            numOfOrbs = Random.Range(6, 10);
        }

        //scale for 1 or 2 players
        if (GameManager.instance.twoPlayers)
        {
            return (baseExp + (baseExp * monsterLevel));
        }
        else
        {
            return ((baseExp * 0.5f) + ((baseExp * 0.5f) * monsterLevel));
        }
    }

    //Trigger Enter
    protected void OnTriggerEnter(Collider other)
    {
        //if other is a player and my box collider is on (box colliders will be used for attacks)
        if (other.gameObject.tag == "Player" && GetComponent<BoxCollider>() && GetComponent<BoxCollider>().enabled)
        {
            //attack player
            other.GetComponent<Player>().ReceiveDamage(damage);
        }
    }

    //Drop Health Orbs
    private void DropHealthOrbs()
    {
        if (numOfOrbs == 1)
        {
            Instantiate(Resources.Load("HealthOrb"), transform.position + transform.up, Quaternion.identity);
        }
        else if (numOfOrbs > 1)
        {
            for (int i = 0; i < numOfOrbs; i++)
            {
                float angle = i * Mathf.PI * 2 / numOfOrbs;
                Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 1.8f;
                Instantiate(Resources.Load("HealthOrb"), transform.position + transform.up + pos, Quaternion.identity);
            }
        }
    }

    //reacquire target
    public GameObject reacquireTgt(targetStyle ts, GameObject sender)
    {
        //if two players
        if (GameManager.instance.twoPlayers)
        {
            Player p1 = GameManager.instance.player1, p2 = GameManager.instance.player2;
            //if only 1 is alive, target that player
            if (p1.Health <= 0f && p2.Health > 0f)
            {
                return p2.gameObject;
            }
            else if (p2.Health <= 0f && p1.Health > 0f)
            {
                return p1.gameObject;
            }
            //both alive
            else if (p2.Health > 0f && p1.Health > 0f)
            {
                //target closer player || assigned player (only on initial target acquisition)
                if (ts == targetStyle.ClosestPlayer || ts == targetStyle.AssignedPlayer)
                {
                    //check if sender(enemy) closer to player 1 or 2
                    //return closer player
                    if ((p1.gameObject.transform.position - sender.transform.position).magnitude < (p2.gameObject.transform.position - sender.transform.position).magnitude)
                    {
                        return p1.gameObject;
                    }
                    else
                    {
                        return p2.gameObject;
                    }
                }
                //target weaker player if they are almost same distance from player (~4f)
                else if (ts == targetStyle.WeakestPlayer)
                {
                    //check distance difference
                    float p1Dist = (p1.gameObject.transform.position - transform.position).magnitude;
                    float p2Dist = (p2.gameObject.transform.position - transform.position).magnitude;

                    //if they close tgt, then get weaker one
                    if (Mathf.Abs(p1Dist - p2Dist) <= 4f)
                    {
                        //compare player HP,
                        //return weaker player
                        if (p1.Health < p2.Health)
                        {
                            return p1.gameObject;
                        }
                        else if (p2.Health < p1.Health)
                        {
                            return p2.gameObject;
                        }
                        //if both same (Wow)
                        else
                        {
                            //return closer player
                            if ((p1.gameObject.transform.position - sender.transform.position).magnitude < (p2.gameObject.transform.position - sender.transform.position).magnitude)
                            {
                                return p1.gameObject;
                            }
                            else
                            {
                                return p2.gameObject;
                            }
                        }
                    }
                    //else just get closer one
                    else
                    {
                        //return closer player
                        if ((p1.gameObject.transform.position - sender.transform.position).magnitude < (p2.gameObject.transform.position - sender.transform.position).magnitude)
                        {
                            return p1.gameObject;
                        }
                        else
                        {
                            return p2.gameObject;
                        }
                    }
                }
            }
            //both dead no need target, game shld be restarting or smth idk
            return null;
        }
        //else if only 1 player
        else
        {
            Player p1 = GameManager.instance.player1;
            //check if he alive
            if (p1.Health <= 0f)
            {
                return null;
            }
            //else just target player 1
            else
            {
                return p1.gameObject;
            }
        }
    }

    //path calculation complete callback
    protected void OnPathComplete(Path p)
    {

        if (!p.error)
        {
            path = p;
            //Reset the waypoint counter
            currentWayPoint = 0;
        }
        else
        {
            Debug.Log(" Error: " + p.error);
        }
    }
}
