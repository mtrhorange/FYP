using UnityEngine;
using System.Collections;
using Pathfinding;

public class FlowerMonster : Enemy {
    //rigidbody
    private Rigidbody rB;
    //timers
    public float attackInterval = 1f,pathUpdateTimer = 3f;
    private float attackTimer;
    //movement variables
    private Vector3 dir = Vector3.zero;
    //acid spit variables
    public GameObject projectile;
    private float interceptionTime = 0f;
    private Vector3 interceptPoint = Vector3.zero;

    //Start
    void Start ()
    {
        //flower monster properties
        health = 30;
        damage = 3; //hit damage, apply continuous poison D.O.T at 2 ticks per second or smth
        //seeker component
        seeker = GetComponent<Seeker>();
        //rigidbody
        rB = GetComponent<Rigidbody>();
        nextWayPointDistance = 3f;
        
        attackTimer = attackInterval;

        //targetting style
        tgtStyle = targetStyle.ClosestPlayer;
        player = base.reacquireTgt(tgtStyle, this.gameObject);
	}
	
	//Update
	void Update ()
    {
        if(myState == States.Idle)
        {
            Idle();
        }
        else if(myState == States.Chase)
        {
            Chase();
        }
        else if(myState == States.Attack)
        {
            Attack();
        }
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
    protected override void Chase()
    {
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

        //if attackTimer is not over yet
        if (attackTimer >= 0)
        {
            if (path.GetTotalLength() > 15f)
            {

                nextPathPoint =
                 path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];

                //look & move
                Vector3 look = dir.normalized + AvoidObstacle();
                look.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(look);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);

                rB.velocity = transform.forward * speed;
            }
            else
            {
                //look at player if can see
                RaycastHit hit;
                Vector3 sight = (player.transform.position - transform.position);
                sight.y = transform.position.y;
                if (Physics.Raycast(transform.position, sight.normalized, out hit))
                {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        Vector3 look = (player.transform.position - transform.position).normalized;
                        look.y = 0;
                        Quaternion targetRotation = Quaternion.LookRotation(look);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
                    }
                }
            }
            attackTimer -= Time.deltaTime;
        }
        //can shoot provided there is a direct LOS to player
        else
        {
            RaycastHit hit;
            Vector3 sight = (player.transform.position - transform.position);
            sight.y = transform.position.y;
            if (Physics.Raycast(transform.position, sight.normalized, out hit))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    myState = States.Attack;
                }
                //else if cannot "see" player, delay the shot till next iteration and check again
                else
                {
                    nextPathPoint =
                path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];

                    //look & move
                    Vector3 look = dir.normalized + AvoidObstacle();
                    look.y = 0;
                    Quaternion targetRotation = Quaternion.LookRotation(look);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);

                    rB.velocity = transform.forward * speed;
                }
            }

        }

        //shot prediction debug ray
        Vector3 aimBot = (interceptPoint - transform.position);
        aimBot.y = 0;
        Debug.DrawRay(transform.position, aimBot.normalized * 15f, Color.magenta);

        pathUpdate();
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
            currentWayPoint = 2;
            pathUpdateTimer = 1f;
        }
    }

    //Attack
    protected override void Attack()
    {
        //calculate where to shoot
        Vector3 calc = CalculateInterception(player.transform.position, player.GetComponent<Rigidbody>().velocity, transform.GetChild(5).position, 10f);
        if (calc != Vector3.zero)
        {
            calc.Normalize();
            interceptionTime = GetApproachingPoint(player.transform.position, player.GetComponent<Rigidbody>().velocity, transform.GetChild(5).position, calc * 10f);
            interceptPoint = player.transform.position + player.GetComponent<Rigidbody>().velocity * interceptionTime;
        }
        shoot(interceptPoint);
        //reset attack interval and state to chase
        attackTimer = attackInterval;
        myState = States.Chase;
    }

    //shoot acid
    private void shoot(Vector3 here)
    {
        Vector3 shootHere = (here - transform.GetChild(5).position).normalized;
        //shootHere.y = 0;
        GameObject boo = (GameObject)Instantiate(projectile, transform.GetChild(5).position, Quaternion.identity);
        boo.GetComponent<Rigidbody>().velocity = shootHere * 10f;

    }

    //calculates an interception path along the path of another moving object
    public Vector3 CalculateInterception(Vector3 tgtPos, Vector3 tgtSpeed, Vector3 interceptorPos, float interceptorSpeed)
    {
        //target direction
        Vector3 tgtD = tgtPos - interceptorPos;
        //interceptor and target speed
        float iSpeed2 = interceptorSpeed * interceptorSpeed;
        float tSpeed2 = tgtSpeed.sqrMagnitude;
        float fDot1 = Vector3.Dot(tgtD, tgtSpeed);
        float targetDist2 = tgtD.sqrMagnitude;
        float d = (fDot1 * fDot1) - targetDist2 * (tSpeed2 - iSpeed2);
        //if there is no possible course to shoot
        if (d < 0.1f)
        {
            //abort
            return Vector3.zero;
        }
        float sqrt = Mathf.Sqrt(d);
        float S1 = (-fDot1 - sqrt) / targetDist2;
        float S2 = (-fDot1 + sqrt) / targetDist2;
        //determine final direction towards interception point
        if (S1 < 0.0001f)
        {
            if (S2 < 0.0001f)
                return Vector3.zero;
            else
                return (S2) * tgtD + tgtSpeed;
        }
        else if (S2 < 0.0001f)
            return (S1) * tgtD + tgtSpeed;
        else if (S1 < S2)
            return (S2) * tgtD + tgtSpeed;
        else
            return (S1) * tgtD + tgtSpeed;
    }


    //calculates the timing when 2 objects approach each other
    public float GetApproachingPoint(Vector3 pos1, Vector3 spd1, Vector3 pos2, Vector3 spd2)
    {
        Vector3 pVector = pos1 - pos2;
        Vector3 sVector = spd1 - spd2;
        float d = sVector.sqrMagnitude;
        //if d is 0 (approx to 5 d.p), there is no nearest approaching point, fire asap
        if (d >= -0.0001f && d <= 0.0002f)
            return 0.0f;
        //return nearest approaching point
        return (-Vector3.Dot(pVector, sVector) / d);
    }

    //Avoid obstacles
    protected Vector3 AvoidObstacle()
    {
        Vector3 destPos =
            path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];
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

            if (Hit.transform.gameObject.layer == 8)
            {
               
                return transform.forward - transform.right;
            }

        }

        if (Physics.Raycast((transform.position + transform.up),
            left45, out Hit, minDistance))
        {
            if (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType)
            {
                
                Physics.IgnoreCollision(GetComponent<Collider>(), Hit.transform.GetComponent<Collider>());
            }

            if (Hit.transform.gameObject.layer == 8)
            {
               
                return transform.forward + transform.right;
            }
        }

        if (Physics.Raycast((transform.position + transform.up),
            transform.forward, out Hit, minDistance))
        {
            if (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType)
            {
                
                Physics.IgnoreCollision(GetComponent<Collider>(), Hit.transform.GetComponent<Collider>());
            }

            if (Hit.transform.gameObject.layer == 8)
            {
               
                return transform.forward + Hit.normal;
            }
        }

        //right ray
        if (Physics.Raycast((transform.position), transform.right.normalized, out Hit, 1.5f, 1 << 8))
        {
            transform.position += (-transform.right).normalized * 0.05f;

        }

        //left ray
        else if (Physics.Raycast((transform.position), -transform.right.normalized, out Hit, 1.5f, 1 << 8))
        {
            transform.position += (transform.right).normalized * 0.05f;

        }
        return Vector3.zero;
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
        Debug.DrawLine(transform.position, transform.position + transform.right.normalized * (minDistance - 0.5f),
            Color.blue);
        Debug.DrawLine(transform.position, transform.position - transform.right.normalized * (minDistance - 0.5f),
            Color.blue);

        //Gizmos.color = new Color32(255,0,0,40);
        //Gizmos.DrawSphere(this.transform.position,5f);
    }

}
