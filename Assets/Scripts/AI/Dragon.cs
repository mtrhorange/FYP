using UnityEngine;
using System.Collections;
using Pathfinding;

public class Dragon : Enemy {
    
    //Rigidbody
    private Rigidbody rB;
    //timers
    private float pathUpdateTimer = 0.5f, attackTimer;
    public float attackInterval = 3f;
    //attacking variables
    private bool attacking = false;
    //movement variables
    public float walkSpeed; //speed on ground
    private Vector3 dir;
    private float minDistance = 2.0f;
    private bool flying = false, waitAnim = false;
    //animation component because this scrub uses old skool legacy anims
    private Animation anim;

	//Start
	void Start () 
    {
        //Dragon properties
        health = 50;
        damage = 5;
        //seeker component
        seeker = GetComponent<Seeker>();
        //rigidbody
        rB = GetComponent<Rigidbody>();
        //animation
        anim = GetComponent<Animation>();

        attackTimer = attackInterval;

        //targetting style
        tgtStyle = targetStyle.ClosestPlayer;
        player = base.reacquireTgt(tgtStyle, this.gameObject);
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

        //debug ray for attacking distance
        Debug.DrawRay(transform.position + transform.up, (player.transform.position - transform.position).normalized * 10f, Color.magenta);

        //check distance,
        //fly, walk depending on distance
        //Start flying
        if (path.GetTotalLength() > 15f && !flying)
        {
            flying = true;
            anim.Play("flyBegin");
            anim.PlayQueued("fly");
        }
        //close enough, check whether can attack, if not land and walk
        else if (path.GetTotalLength() <= 15f)
        {
            //attack interval is up && can see player
            if (attackTimer <= 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, 10f))
                {
                    if (hit.transform.tag == "Player")
                    {
                        myState = States.Attack;
                    }
                }
            }
            else if (!waitAnim)
            {
                //if not flying, cotinue pursuit on foot
                if (!flying)
                {
                    anim.Play("walk");
                    anim["walk"].speed = 2f;
                }
                //if flying, land first
                else
                {
                    waitAnim = true;
                    playAnim("land", 2f, true);
                }
            }
        }


        //look & move
        dir = AvoidObstacle();
        Vector3 look = dir.normalized;
        look.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
        rB.velocity = transform.forward * (flying ? speed : walkSpeed);

        attackTimer -= Time.deltaTime;
    }

    //Attack
    protected override void Attack()
    {
        //decide which attack to use depending on how close to the player
        //within close range (3f), use bite or ground stomp
        //within medium range (10f), use fire breath
        if (!attacking && (transform.position - player.transform.position).magnitude <= 10f)
        {
            attacking = true;
            //if flying, breathe fire from the air
            if (flying)
            {
                playAnim("fly_breath", 1, true);
            }
            //if on the ground, breathe fire while standing
            else
            {
                playAnim("stand_breath", 1, true);
            }
        }
    }

    //play animation (legacy)
    private void playAnim(string name, float playSpeed, bool callBack)
    {
        anim.Play(name);
        anim[name].speed = playSpeed;
        if (callBack)
        {
            StartCoroutine(animCallBack(playSpeed, name));
        }
    }

    //animation callback
    private IEnumerator animCallBack(float seconds, string name)
    {
        yield return new WaitForSeconds(anim[name].length / seconds);
        switch(name)
        {
            case "land":
                flying = false;
                anim.Play("walk");
                anim["walk"].speed = 2f;
                break;
            case "fly_breath":
            case "stand_breath":
                attackTimer = attackInterval;
                attacking = false;
                myState = States.Chase;
                break;
        }
        waitAnim = false;
    }

    //update calculated path every set time
    public void pathUpdate()
    {
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

    //Avoid obstacles
    protected Vector3 AvoidObstacle()
    {
        Vector3 destPos = path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];
        RaycastHit Hit;
        //Check if there is obstacle
        Vector3 right45 = (transform.forward + transform.right).normalized;
        Vector3 left45 = (transform.forward - transform.right).normalized;
        //Shoot the rays!
        //right ray
        if (Physics.Raycast((transform.position), transform.right.normalized, out Hit, 1.5f))
        {
            if (Hit.transform.gameObject.tag == "Enemy")
            {
                return (-transform.right).normalized;
            }
        }
        //left ray
        else if (Physics.Raycast((transform.position), -transform.right.normalized, out Hit, 1.5f))
        {
            if (Hit.transform.gameObject.tag == "Enemy")
            {
                return (transform.right).normalized;
            }
        }
        //front ray
        else if (Physics.Raycast((transform.position),
        transform.forward, out Hit, minDistance))
        {
            //if hit an enemy and is not my type
            if (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), Hit.transform.GetComponent<Collider>());
            }
            else
            {
                //if left 45 deg and right 45 deg have thing also
                if (Physics.Raycast((transform.position),
                    right45, out Hit, minDistance * 2.5f) && Physics.Raycast((transform.position),
            left45, out Hit, minDistance * 2.5f))
                {
                    return (transform.forward - transform.right + Hit.normal).normalized * 0;
                }
                //only right 45 deg
                else if (Physics.Raycast((transform.position),
                    right45, out Hit, minDistance * 2.5f))
                {
                    return (transform.forward + transform.right + Hit.normal).normalized * 0f;
                }
                //only left 45 deg
                else if (Physics.Raycast((transform.position),
                    left45, out Hit, minDistance * 2.5f))
                {
                    return (transform.forward - transform.right + Hit.normal).normalized * 0f;
                }
            }
        }
        //right 45 deg ray
        else if (Physics.Raycast((transform.position),
        right45, out Hit, minDistance))
        {
            return (transform.forward - transform.right).normalized;
        }
        //left 45 deg ray
        else if (Physics.Raycast((transform.position),
        left45, out Hit, minDistance))
        {
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

        Debug.DrawLine(transform.position, frontRay, Color.blue);
        Debug.DrawLine(transform.position, left45, Color.blue);
        Debug.DrawLine(transform.position, right45, Color.blue);
        Debug.DrawLine(transform.position, transform.position + transform.right.normalized * 1.5f, Color.blue);
        Debug.DrawLine(transform.position, transform.position - transform.right.normalized * 1.5f, Color.blue);

        //Gizmos.color = new Color32(255,0,0,40);
        //Gizmos.DrawSphere(this.transform.position,5f);
    }
}
