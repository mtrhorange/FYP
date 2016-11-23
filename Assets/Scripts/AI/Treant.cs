using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Treant : Enemy {

    //Rigidbody
    private Rigidbody rB;
    //timers
    private float pathUpdateTimer = 3f;
    public float attackInterval = 3f;
    public float attackTimer;
    public GameObject shockwave;
    private Vector3 heightOffset;
    //movement variables
    private Vector3 dir;
    private Animator anim;
    private bool attacking = false;


   

	// Use this for initialization
    protected override void Start()
    {
        heightOffset = transform.up ;
        anim = GetComponent<Animator>();
        base.Start();
        //Zombie properties
        health = 20;
        damage = 2;
        //seeker component
        seeker = GetComponent<Seeker>();
        //rigidbody
        rB = GetComponent<Rigidbody>();
        nextWayPointDistance = 3f;

        attackTimer = attackInterval;

        //targetting style
        tgtStyle = targetStyle.AssignedPlayer;
        player = base.reacquireTgt(tgtStyle, this.gameObject);
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
        else if (myState == States.Dead)
        {
            Death();
        }

        //testing
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    ReceiveDamage(5);
        //}
	}

    //Idle
    protected override void Idle()
    {
        
        //chase target
        target = player.transform.position;
        //set a path to tgt position
        seeker.StartPath(transform.position, target, OnPathComplete);
        currentWayPoint = 1;
        myState = States.Chase;
        
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
        //check first if catched up to the player
        //if yes proceed to attack
        //attack trigger distance debug ray
        Debug.DrawRay(transform.position + transform.up, (player.transform.position - transform.position).normalized * 3f, Color.cyan);
        Debug.DrawRay(transform.position + transform.up, velocity, Color.magenta);
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            if ((player.transform.position - transform.position).magnitude <= 3.5f)
            {
                myState = States.Attack;
                anim.SetBool("Walk", false);
                
                rB.velocity = Vector3.zero;
                attacking = true;
                
            }
            else
            {
                anim.SetBool("Walk", true);
                if (currentWayPoint < path.vectorPath.Count)
                    nextPathPoint =
                        path.vectorPath[
                            currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];

                //look & move
                dir = velocity;

                Vector3 look = dir.normalized + AvoidObstacle();
    
                look.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(look);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);

                rB.velocity = transform.forward * speed;
                
            }
           
        }
        else 
        {
            if ((player.transform.position - transform.position).magnitude <= 3.5f)
            {
                anim.SetBool("Walk", false);
            }
            else
            {
                anim.SetBool("Walk", true);
                if (currentWayPoint < path.vectorPath.Count)
                    nextPathPoint =
                        path.vectorPath[
                            currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];

                //look & move
                dir = velocity;

                Vector3 look = dir.normalized + AvoidObstacle();
                look.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(look);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);

                rB.velocity = transform.forward * speed;
                
            }
        }


        if (currentWayPoint >= path.vectorPath.Count)
        {
            Debug.Log("End Point Reached");
            //go back to idle
            if ((player.transform.position - transform.position).magnitude >= 3.5f)
                myState = States.Idle;

            return;
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
        Vector3 look = player.transform.position - transform.position;

        look.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(look);
        transform.rotation = targetRotation;

        pathUpdateTimer = 0f;
        
        rB.velocity = Vector3.zero;

        int whichAttack = Random.Range(0, 2);

        if (whichAttack == 0)
        {
            if (attacking)
            {
                anim.SetTrigger("Attack");
                attacking = false;
            }

        }
        else 
        {
            if (attacking)
            {
                jumpAttack();
                attacking = false;
            }
        }
        attackTimer = attackInterval;
        myState = States.Chase;
    }

    void summonShit()
    {
        //TODO: SPAWN SHIT
        Debug.Log("Summon shit");
        attackTimer = attackInterval;
        myState = States.Chase;
    }

    void jumpAttack()
    {
        Debug.Log("jumpman");
        anim.SetTrigger("Jump Attack");
        attackTimer = attackInterval;
        myState = States.Chase;
    }

    public void spawnShockwave()
    {
        attackTimer = attackInterval;
        Instantiate(shockwave, new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z), Quaternion.identity);
    }
    public void triggerOn()
    {

        rB.velocity = Vector3.zero;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void triggerOff()
    {
        attackTimer = attackInterval;
        myState = States.Chase;
        GetComponent<BoxCollider>().enabled = false;
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
            currentWayPoint = 2;
            pathUpdateTimer = 1f;
        }

        nextPathPoint.y = 0;
        pathUpdateTimer -= Time.deltaTime;
    }
    
    //Avoid Obstacles
    protected Vector3 AvoidObstacle()
    {
        heightOffset = transform.up * 0.5f;
        RaycastHit Hit;
        //Check if there is obstacle
        Vector3 right45 = (transform.forward + transform.right).normalized;
        Vector3 left45 = (transform.forward - transform.right).normalized;

        //Shoot the rays!
        if (Physics.Raycast((transform.position + transform.up),
            right45 + heightOffset, out Hit, minDistance))
        {
            //if is obstacle
            if (Hit.transform.gameObject.layer == 8 ||
                (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType))
                return transform.forward - transform.right;
        }
                
        

        if (Physics.Raycast((transform.position + transform.up),
            left45 + heightOffset, out Hit, minDistance))
        {
            //if is obstacle
            if (Hit.transform.gameObject.layer == 8 ||
                (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType))
                return transform.forward - transform.right;
        }

        if (Physics.Raycast((transform.position + transform.up),
            transform.forward + heightOffset, out Hit, minDistance))
        {
            //if is obstacle
            if (Hit.transform.gameObject.layer == 8 ||
                (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType))
                return transform.forward - transform.right;
        }

        //right ray
        if (Physics.Raycast((transform.position) + heightOffset, transform.right.normalized + heightOffset, out Hit, 1.5f, 1 << 8))
        {
            transform.position += (-transform.right).normalized * 0.05f;
        }

        //left ray
        else if (Physics.Raycast((transform.position) + heightOffset, -transform.right.normalized + heightOffset, out Hit, 1.5f, 1 << 8))
        {
            transform.position += (transform.right).normalized * 0.05f;

        }
        return Vector3.zero;
    }


    void OnDrawGizmos()
    {
        heightOffset = transform.up*0.5f;
        Vector3 frontRay = transform.position + transform.forward * minDistance;
        Vector3 right45 = transform.position +
            (transform.forward + transform.right).normalized * minDistance;
        Vector3 left45 = transform.position +
            (transform.forward - transform.right).normalized * minDistance;

        Debug.DrawLine(transform.position + heightOffset, frontRay + heightOffset, Color.blue);
        Debug.DrawLine(transform.position + heightOffset, left45 + heightOffset, Color.blue);
        Debug.DrawLine(transform.position + heightOffset, right45 + heightOffset, Color.blue);
        Debug.DrawLine(transform.position + heightOffset, transform.position + transform.right.normalized * (minDistance - 0.5f) + heightOffset,
            Color.blue);
        Debug.DrawLine(transform.position + heightOffset, transform.position - transform.right.normalized * (minDistance - 0.5f) + heightOffset,
            Color.blue);

        //Gizmos.color = new Color32(255,0,0,40);
        //Gizmos.DrawSphere(this.transform.position,5f);
    }
}
