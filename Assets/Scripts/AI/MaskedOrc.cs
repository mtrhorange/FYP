using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class MaskedOrc : Enemy {

    //Rigidbody
    private Rigidbody rB;
    //timers
    private float pathUpdateTimer = 3f;
    public float attackInterval = 3f;
    public float attackTimer;
    //movement variables
    private Vector3 dir;
    private Animator anim;
    private bool attacking = false;

   

	// Use this for initialization
    protected override void Start()
    {
        myStrength = Strength.Medium;

        anim = GetComponent<Animator>();
        base.Start();
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
            if ((player.transform.position - transform.position).magnitude <= 3f)
            {
                anim.SetBool("RunWalk", false);

                anim.SetTrigger("Attack 01");
                rB.velocity = Vector3.zero;
                attacking = true;
                myState = States.Attack;
            }
            else
            {
                anim.SetBool("RunWalk", true);
                if (currentWayPoint < path.vectorPath.Count)
                    nextPathPoint =
                        path.vectorPath[
                            currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];

                //look & move
                dir = velocity;

                Vector3 look = dir.normalized  + AvoidObstacle();
    
                look.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(look);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);

                rB.velocity = transform.forward * speed;
                
            }
           
        }
        else 
        {
            if ((player.transform.position - transform.position).magnitude <= 2f)
            {
                anim.SetBool("RunWalk",false);
            }
            else
            {
                anim.SetBool("RunWalk", true);
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
            if ((player.transform.position - transform.position).magnitude >= 3f)
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

    //Flinch override
    protected override void Flinch()
    {
        base.Flinch();
        //stop moving
        rB.velocity = Vector3.zero;
        GetComponent<BoxCollider>().enabled = false;
        attacking = false;
        //play flinch animaton
        anim.SetBool("RunWalk", false);
        anim.SetTrigger("Take Damage");
        SFXManager.instance.playSFX(sounds.ogre);
    }

    //Flinch End Animation Event callback override
    public override void FlinchEnd()
    {
        pathUpdateTimer = 0;
        pathUpdate();
        myState = States.Chase;
    }

    //Attack
    protected override void Attack()
    {
        attacking = false;
            
        pathUpdateTimer = 0f;
        
        rB.velocity = Vector3.zero;

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
        pathUpdateTimer -= Time.deltaTime;

        if (pathUpdateTimer <= 0)
        {
            //get target
            player = base.reacquireTgt(tgtStyle, this.gameObject);
            //chase target
            target = player.transform.position;
            //set a path to tgt position
            seeker.StartPath(transform.position, target, OnPathComplete);
            currentWayPoint = 2;
            pathUpdateTimer = 1f;
        }
    }
    
    //Avoid Obstacles
    protected Vector3 AvoidObstacle()
    {
        RaycastHit Hit;
        //Check if there is obstacle
        Vector3 right45 = (transform.forward + transform.right).normalized;
        Vector3 left45 = (transform.forward - transform.right).normalized;

        //Shoot the rays!
        if (Physics.Raycast((transform.position + transform.up),
            right45, out Hit, minDistance))
        {
            if (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType)
            {
                
                Physics.IgnoreCollision(GetComponent<Collider>(), Hit.transform.GetComponent<Collider>());
            }

            //if is obstacle
            if (Hit.transform.gameObject.layer == 8)
                return transform.forward - transform.right;
        }
                
        

        if (Physics.Raycast((transform.position + transform.up),
            left45, out Hit, minDistance))
        {
            Debug.Log(Hit.transform.name);
            if (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType)
            {
                
                Physics.IgnoreCollision(GetComponent<Collider>(), Hit.transform.GetComponent<Collider>());
            }

            //if is obstacle
            if (Hit.transform.gameObject.layer == 8)
                return transform.forward + transform.right;
        }

        if (Physics.Raycast((transform.position + transform.up),
            transform.forward, out Hit, minDistance))
        {
            Debug.Log(Hit.transform.name);
            if (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType)
            {
                
                Physics.IgnoreCollision(GetComponent<Collider>(), Hit.transform.GetComponent<Collider>());
            }

            //if is obstacle
            if (Hit.transform.gameObject.layer == 8)
                return transform.forward + Hit.normal;
        }

        //right ray
        if (Physics.Raycast((transform.position), transform.right.normalized, out Hit, 2f, 1 << 8))
        {
            transform.position += (-transform.right).normalized * 0.05f;
        }

        //left ray
        else if (Physics.Raycast((transform.position), -transform.right.normalized, out Hit, 2f, 1 << 8))
        {
            transform.position += (transform.right).normalized * 0.05f;

        }
        return Vector3.zero;
    }


    void OnDrawGizmos()
    {
        Vector3 frontRay = transform.position + transform.forward* minDistance + transform.up;
        Vector3 right45 = transform.position +
            (transform.forward + transform.right).normalized * minDistance + transform.up;
        Vector3 left45 = transform.position +
            (transform.forward - transform.right).normalized  * minDistance + transform.up;

        Debug.DrawLine(transform.position + transform.up, frontRay, Color.blue);
        Debug.DrawLine(transform.position + transform.up, left45, Color.blue);
        Debug.DrawLine(transform.position + transform.up, right45, Color.blue);
        Debug.DrawLine(transform.position + transform.up, transform.position + transform.right.normalized * (minDistance - 0.5f) + transform.up,
            Color.blue);
        Debug.DrawLine(transform.position + transform.up , transform.position - transform.right.normalized * (minDistance - 0.5f) + transform.up,
            Color.blue);

        //Gizmos.color = new Color32(255,0,0,40);
        //Gizmos.DrawSphere(this.transform.position,5f);
    }
}
