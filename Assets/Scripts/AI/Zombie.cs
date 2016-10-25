using UnityEngine;
using System.Collections;
using Pathfinding;

public class Zombie : Enemy {

    //Character Controller
    private CharacterController charCon;
    //timers
    private float pathUpdateTimer = 0.5f, attackTimer;
    public float attackInterval = 3f;
    //movement variables
    private Vector3 dir;

    private bool attacking = false;

	// Use this for initialization
	void Start () 
    {
        //Zombie properties
        health = 20;
        damage = 1;
        //seeker component
        seeker = GetComponent<Seeker>();
        //character controller
        charCon = GetComponent<CharacterController>();
        nextWayPointDistance = 2f;
        //player reference
        player = GameObject.FindGameObjectWithTag("Player");

        attackTimer = attackInterval;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (myState == States.Idle)
        {
            Idle();
        }
        else if (myState == States.Chase)
        {
            Chase();
        }
        else if (myState == States.Attack)
        {
            Attack();
        }
	}

    //Idle
    protected override void Idle()
    {
        if (player)
        {
            myState = States.Chase;
        }
        //chase target
        target = player.transform.position;
        //set a path to tgt position
        seeker.StartPath(transform.position, target, OnPathComplete);
        currentWayPoint = 0;
    }

    //Chase
    protected override void Chase()
    {
        pathUpdate();

        //if no path yet
        if (path == null)
        {
            Debug.Log("NO PATH");
            //No path to move to yet
            return;
        }
        if (currentWayPoint >= path.vectorPath.Count)
        {
            Debug.Log("End Point Reached");
            //go back to idle
            myState = States.Idle;
            return;
        }

        //check first if catched up to the player
        //if yes proceed to attack
        //attack trigger distance debug ray
        Debug.DrawRay(transform.position + transform.up, (player.transform.position - transform.position).normalized * 3f, Color.magenta);
        if (attackTimer <= 0 && (player.transform.position - transform.position).magnitude <= 3f)
        {
            attacking = true;
            myState = States.Attack;
        }
        else
        {
            //move
            dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
            //factor in the speed to move at
            dir *= speed;
            //move
            charCon.Move(dir * Time.deltaTime);

            attackTimer -= Time.deltaTime;

            //look at where you walking
            Vector3 look = (path.vectorPath[currentWayPoint + 2 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 2] - transform.position).normalized;
            look.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(look);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
        }

        //update the waypoint on the path once the current one has been reached
        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < nextWayPointDistance)
        {
            currentWayPoint++;
            return;
        }
    }

    //Attack
    protected override void Attack()
    {
        if (transform.GetChild(3).localScale.y  <= 5f && attacking)
        {
            transform.GetChild(3).localScale = new Vector3(transform.GetChild(3).localScale.x, transform.GetChild(3).localScale.y + Time.deltaTime * 4f, transform.GetChild(3).localScale.z);
            transform.GetChild(4).localScale = new Vector3(transform.GetChild(4).localScale.x, transform.GetChild(4).localScale.y + Time.deltaTime * 4f, transform.GetChild(4).localScale.z);

            if (transform.GetChild(3).localScale.y  >= 5f)
            {
                attacking = false;
            }
        }
        else
        {
            transform.GetChild(3).localScale = new Vector3(transform.GetChild(3).localScale.x, transform.GetChild(3).localScale.y - Time.deltaTime * 4, transform.GetChild(3).localScale.z);
            transform.GetChild(4).localScale = new Vector3(transform.GetChild(4).localScale.x, transform.GetChild(4).localScale.y - Time.deltaTime * 4, transform.GetChild(4).localScale.z);
            if (transform.GetChild(3).localScale.y <= 2f)
            {
                transform.GetChild(3).localScale = new Vector3(transform.GetChild(3).localScale.x, 2, transform.GetChild(3).localScale.z);
                transform.GetChild(4).localScale = new Vector3(transform.GetChild(4).localScale.x, 2, transform.GetChild(4).localScale.z);

                attackTimer = attackInterval;
                myState = States.Chase;
            }
        }
    }

    //update calculated path every set time
    public void pathUpdate(){

        pathUpdateTimer -= Time.deltaTime;

        if (pathUpdateTimer <= 0)
        {
            //chase target
            target = player.transform.position;
            //set a path to tgt position
            seeker.StartPath(transform.position, target, OnPathComplete);
            currentWayPoint = 1;
            pathUpdateTimer = 1f;
        }
    }
}
