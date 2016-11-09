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
    void Start()
    {
        damageText = (GameObject)Resources.Load("DamageText");
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

    public void ReceiveDamage(float dmg)
    {

        health -= dmg;
        Camera camera = FindObjectOfType<Camera>();
        Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
        GameObject txt = (GameObject)Instantiate(damageText, screenPos, Quaternion.identity);
        txt.transform.SetParent(GameObject.Find("Canvas").transform);
        txt.GetComponent<UnityEngine.UI.Text>().text = dmg.ToString("F0");
        //txt.GetComponent<TextMesh>().text = dmg.ToString("F0");
        //txt.transform.Rotate(55, 0, 0);
    }

    void OnTriggerEnter(Collider other)
    {


    }

    //reacquire target
    public GameObject reacquireTgt(targetStyle ts, GameObject sender)
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
            //target weaker player
            else if (ts == targetStyle.WeakestPlayer)
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
        }
        //both dead no need target, game shld be restarting or smth idk
        return null;
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
