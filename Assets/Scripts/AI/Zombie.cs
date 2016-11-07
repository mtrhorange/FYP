using UnityEngine;
using System.Collections;
using Pathfinding;

public class Zombie : Enemy {

    //Rigidbody
    private Rigidbody rB;
    //timers
    private float pathUpdateTimer = 0f, attackTimer;
    public float attackInterval = 3f;
    //movement variables
    private Vector3 dir;

    private bool attacking = false;

    public float minDistance = 3.0f;

	// Use this for initialization
	void Start () 
    {
        //Zombie properties
        health = 20;
        damage = 1;
        //seeker component
        seeker = GetComponent<Seeker>();
        //rigidbody
        rB = GetComponent<Rigidbody>();
        nextWayPointDistance = 2f;

        attackTimer = attackInterval;

        //targetting style
        tgtStyle = targetStyle.AssignedPlayer;
        //player = base.reacquireTgt(tgtStyle, this.gameObject);
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
        base.Idle();
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

        //update the waypoint on the path once the current one has been reached
        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < nextWayPointDistance)
        {
            currentWayPoint++;
            return;
        }

        //check first if catched up to the player
        //if yes proceed to attack
        //attack trigger distance debug ray
        //Debug.DrawRay(transform.position + transform.up, nextPathPoint, Color.cyan);
        Debug.DrawRay(transform.position + transform.up, velocity, Color.magenta);
        if (attackTimer <= 0 && (player.transform.position - transform.position).magnitude <= 3f)
        {
            attacking = true;
            myState = States.Attack;
        }
        else
        {
            nextPathPoint =
                path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];

            //look & move
            dir = velocity;


            Vector3 look = dir.normalized + AvoidObstacle();
            look.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(look);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);


            //factor in the speed to move at
            //dir *= speed;
            //move
            //charCon.Move(dir * Time.deltaTime);

            rB.velocity = transform.forward * speed;

            attackTimer -= Time.deltaTime;

            //look at where you walking
            //Vector3 look = (path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1] - transform.position).normalized;
            //look.y = 0;
            //Quaternion targetRotation = Quaternion.LookRotation(look);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
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
                pathUpdateTimer = 0f;
                myState = States.Chase;
            }
        }
    }

    //update calculated path every set time
    public void pathUpdate()
    {


        if (pathUpdateTimer <= 0)
        {
            //chase target
            target = player.transform.position;
            //set a path to tgt position
            seeker.StartPath(transform.position, target, OnPathComplete);
            currentWayPoint = 1;
            pathUpdateTimer = 1f;
        }

        nextPathPoint.y = 0;
        pathUpdateTimer -= Time.deltaTime;
    }

    //Avoid obstacles
    protected Vector3 AvoidObstacle()
    {
        Vector3 destPos = path.vectorPath[currentWayPoint];
        RaycastHit Hit;
        //Check if there is obstacle
        Vector3 right45 = (transform.forward + transform.right).normalized;
        Vector3 left45 = (transform.forward - transform.right).normalized;
        //Shoot the rays!
        //right ray
        if (Physics.Raycast((transform.position), transform.right.normalized, out Hit, 1.5f))
        {
            //if (Hit.transform.gameObject.tag == "Enemy")
            //{
            //    return (-transform.right).normalized;
            //}
        }
        //left ray
        else if (Physics.Raycast((transform.position), -transform.right.normalized, out Hit, 1.5f))
        {
            //if (Hit.transform.gameObject.tag == "Enemy")
            //{
            //    return (transform.right).normalized;
            //}
        }
        //front ray
        else if (Physics.Raycast((transform.position),
            transform.forward, out Hit, minDistance))
        {
            //if hit an enemy and is not my type
            if (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType)
            {
                Debug.Log("hit " + Hit);
                Physics.IgnoreCollision(GetComponent<Collider>(), Hit.transform.GetComponent<Collider>());
            }
            else
            {
                //    //if left 45 deg and right 45 deg have thing also
                //    if (Physics.Raycast((transform.position),
                //        right45, out Hit, minDistance * 2.5f) && Physics.Raycast((transform.position),
                //left45, out Hit, minDistance * 2.5f))
                //    {
                //        return (transform.forward - transform.right + Hit.normal).normalized * 2f;
                //    }
                //    //only right 45 deg
                //    else if (Physics.Raycast((transform.position),
                //        right45, out Hit, minDistance * 2.5f))
                //    {
                //        return (transform.forward + transform.right + Hit.normal).normalized * 2f;
                //    }
                //    //only left 45 deg
                //    else if (Physics.Raycast((transform.position),
                //        left45, out Hit, minDistance * 2.5f))
                //    {
                //        return (transform.forward - transform.right + Hit.normal).normalized * 2f;
                //    }
            }
        }
        //right 45 deg ray
        else if (Physics.Raycast((transform.position),
            right45, out Hit, minDistance))
        {
            //if hit an enemy and is not my type
            if (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType)
            {
                Debug.Log("hit " + Hit);
                Physics.IgnoreCollision(GetComponent<Collider>(), Hit.transform.GetComponent<Collider>());
            }

            if (Hit.transform.tag != "Enemy")
                return (transform.forward - transform.right).normalized;
        }
        //left 45 deg ray
        else if (Physics.Raycast((transform.position),
            left45, out Hit, minDistance))
        {
            //if hit an enemy and is not my type
            if (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType)
            {
                Debug.Log("hit " + Hit);
                Physics.IgnoreCollision(GetComponent<Collider>(), Hit.transform.GetComponent<Collider>());
            }

            if (Hit.transform.tag != "Enemy")
                return (transform.forward + transform.right).normalized;
        }

        return (destPos - transform.position).normalized;
    }

    void OnDrawGizmos()
    {
        Vector3 frontRay = transform.position + transform.forward * minDistance;
        Vector3 right45 = transform.position +
            (transform.forward + transform.right).normalized * minDistance;
        Vector3 left45 = transform.position +
            (transform.forward - transform.right).normalized * minDistance;

        Debug.DrawLine(transform.position, frontRay , Color.blue);
        Debug.DrawLine(transform.position, left45 , Color.blue);
        Debug.DrawLine(transform.position, right45 , Color.blue);
        Debug.DrawLine(transform.position, transform.position + transform.right.normalized * 1.5f , Color.blue);
        Debug.DrawLine(transform.position, transform.position - transform.right.normalized * 1.5f , Color.blue);

        //Gizmos.color = new Color32(255,0,0,40);
        //Gizmos.DrawSphere(this.transform.position,5f);
    }
}
