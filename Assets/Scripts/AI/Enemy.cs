using UnityEngine;
using System.Collections;
using Pathfinding;

public class Enemy : MonoBehaviour
{

    public float health;
    public GameObject damageText;

    public float damage;

    //target position
    protected Vector3 target;
    //flocking velocity
    public Vector3 velocity;
    //player reference
    public GameObject player;
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

    //enemy strength category
    public enum Strength
    {
        Weak,
        Medium,
        Strong
    }
    //myStrength (how strong this enemy is)
    public Strength myStrength = Strength.Weak;
    //Monster Level
    protected int monsterLevel = 1;

    //States
    public enum States
    {
        Idle,
        Chase,
        Attack,
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
    bool isSlowed = false;
    bool isStunned = false;
    bool isConfused = false;

    //Start
    protected virtual void Start()
    {
        damageText = (GameObject)Resources.Load("DamageText");
        CalculateDamage();

    }

    //Update
    void Update()
    {

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
        Debug.Log("ENEMY SCRIPT DEATH");
        myState = States.Dead;

        GetComponent<CapsuleCollider>().enabled = false;

        GetComponent<Rigidbody>().velocity = -transform.up * 8f;

        AIManager.instance.RemoveMe(this.gameObject);

        Destroy(this.gameObject, 5f);
    }

    //receive damage
    public void ReceiveDamage(float dmg)
    {

        health -= dmg;
        Camera camera = FindObjectOfType<Camera>();
        Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
        GameObject txt = (GameObject)Instantiate(damageText, screenPos, Quaternion.identity);
        txt.transform.SetParent(GameObject.Find("Canvas").transform);
        txt.GetComponent<UnityEngine.UI.Text>().text = dmg.ToString("F0");
        txt.GetComponent<DamageText>().target = transform;
        //txt.GetComponent<TextMesh>().text = dmg.ToString("F0");
        //txt.transform.Rotate(55, 0, 0);

        if (health <= 0)
        {
            Death();
        }
    }

    //Calculate damage to deal
    protected float CalculateDamage()
    {
        Debug.Log(myStrength);
        float baseDmg = 0, baseMul = 0, levelMul = 0;

        if (myStrength == Strength.Weak)
        {
            Debug.Log("wk");
            baseDmg = 3;
            baseMul = 0.3f;
            levelMul = 0.5f;
        }
        else if (myStrength == Strength.Medium)
        {
            Debug.Log("md");
            baseDmg = 5;
            baseMul = 0.5f;
            levelMul = 0.6f;
        }
        else if (myStrength == Strength.Strong)
        {
            Debug.Log("st");
            baseDmg = 8;
            baseMul = 0.75f;
            levelMul = 0.75f;
        }

        //base: 3, 5, 8
        //baseMul: 0.3, 0.5, 0.75
        //levelMul: 0.5, 0.6, 0.75
        //damage = base + (LVL - 1) * baseMul + (LVL / 5) * levelMul

        return baseDmg + ((monsterLevel - 1) * baseMul) + ((monsterLevel / 5) * levelMul);
    }

    //Trigger Enter
    protected void OnTriggerEnter(Collider other)
    {
        //if other is a player and my box collider is on (box colliders will be used for attacks)
        if (other.gameObject.tag == "Player" && GetComponent<BoxCollider>() && GetComponent<BoxCollider>().enabled)
        {
            //attack player
            other.GetComponent<Player>().ReceiveDamage(CalculateDamage());
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
