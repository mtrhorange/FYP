using UnityEngine;
using System.Collections;
using Pathfinding;

public class FlowerMonster : Enemy {
    //character controller
    private CharacterController charCon;
    //timers
    private float attackInterval = 4f;
    //movement variables
    private Vector3 dir = Vector3.zero;

    //acid spit projectile
    public GameObject projectile;


	// Use this for initialization
	void Start ()
    {
        //flower monster properties
        health = 30;
        damage = 3; //hit damage, apply continuous poison D.O.T at 2 ticks per second or smth
        //seeker component
        seeker = GetComponent<Seeker>();
        //character controller
        charCon = GetComponent<CharacterController>();
        //player reference
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(myState == States.Idle)
        {
            myState = States.Chase;
            //chase target
            target = player.transform.position;
            //set a path to tgt position
            seeker.StartPath(transform.position, target, OnPathComplete);
            currentWayPoint = 0;
        }
        else if(myState == States.Chase)
        {
            Chase();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            shoot();
        }
	}

    //Chase
    protected override void Chase()
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

        //if can't shoot yet, attackInterval not over yet
        if (attackInterval >= 0)
        {
            //chase target
            target = player.transform.position;
            //set a path to tgt position
            seeker.StartPath(transform.position, target, OnPathComplete);
            currentWayPoint = 0;
            //set the direction to move to
            dir = (path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1] - transform.position).normalized;
            //factor in the speed to move at
            dir *= speed;
            //move
            charCon.Move(dir * Time.deltaTime);

            //look
            Vector3 look = dir;
            look.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(look);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);

            attackInterval -= Time.deltaTime;
        }
        //can shoot
        else
        {
            //calculate where to shoot


            //reset attack interval
            attackInterval = 4f;
        }
        

        //update the waypoint on the path once the current one has been reached
        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < nextWayPointDistance)
        {
            currentWayPoint++;
            return;
        }
    }

    //shoot acid
    private void shoot()
    {
        GameObject boo = (GameObject)Instantiate(projectile, transform.GetChild(5).position, Quaternion.identity);
        Vector3 targ = (player.transform.position - transform.position).normalized;
        targ.y = 0;
        boo.GetComponent<Rigidbody>().velocity = targ * 5f;

    }

}
