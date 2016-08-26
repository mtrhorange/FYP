using UnityEngine;
using System.Collections;
using Pathfinding;

public class Slime : Enemy {
    //target position
    public Vector3 target;
    //player reference
    private GameObject player;
    //seeker component
    private Seeker seeker;
    //character controller
    private CharacterController charCon;
    //calculated path
    public Path path;
    //AI speed
    public float speed;
    private float originalSpeed;
    //distance AI is to a waypoint for it to continue to the next
    public float nextWayPointDistance = 1f;
    //current waypoint
    private int currentWayPoint = 0;
    //timers
    private float idleTimer = 0, chaseTimer = 1;
    //movement boolean
    private bool move = true;

    private Vector3 lastPos;

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

	//Start
	void Start ()
    {
        //slime properties
        health = 25;
        damage = 5;
        originalSpeed = speed;
        //get our seeker component
        seeker = GetComponent<Seeker>();
        //get character controller
        charCon = GetComponent<CharacterController>();

        //player
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	//Update
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
	}

    //Idle state
    private void Idle()
    {
        //idle for 3 seconds
        if (idleTimer >= 3)
        {
            //check if idle again or Chase
            if (Random.Range(0f, 1f) >= 0.4f)
            {
                //chase target
                target = player.transform.position;
                //set a path to tgt position
                seeker.StartPath(transform.position, target, OnPathComplete);
                currentWayPoint = 0;
                myState = States.Chase;
            }
            else
            {
                Debug.Log("Idle again");
            }
            idleTimer = 0;
        }
        idleTimer += Time.deltaTime;
    }

    //Patrol
    private void Chase()
    {
        if (path == null)
        {
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
        //only move in "jumps" (every 1 sec)
        if (move)
        {
            //Direction to the next waypoint
            Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
            dir *= speed * Time.deltaTime;
            charCon.SimpleMove(dir);
            chaseTimer += Time.deltaTime;
        }
        else
        {
            chaseTimer -= Time.deltaTime;
        }
        if (chaseTimer >= 1)
        {
            move = false;
            speed = 0;

            //chase target
            target = player.transform.position;
            //set a path to tgt position
            seeker.StartPath(transform.position, target, OnPathComplete);
        }
        else if (chaseTimer <= 0)
        {
            move = true;
            speed = originalSpeed;
        }

        //For slime, check if we moved a set distance, then update the movement timer
        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < nextWayPointDistance)
        {
            currentWayPoint++;
            return;
        }
    }

    private void OnPathComplete(Path p)
    {
        Debug.Log("Path Set, Error: " + p.error);
        if (!p.error)
        {
            path = p;
            //Reset the waypoint counter
            currentWayPoint = 0;
        }
    }
}
