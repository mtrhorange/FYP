using UnityEngine;
using System.Collections;
using Pathfinding;

public class BugMonster : Enemy
{
    //Rigidbody
    private Rigidbody rB;
    //timers
    public float attackInterval = 1f, pathUpdateTimer = 0.5f;
    private float attackTimer;
    //movement variables
    private Vector3 dir = Vector3.zero;
    //acid spit variables
    public GameObject projectile;
    private float interceptionTime = 0f;
    private Vector3 interceptPoint = Vector3.zero;

    //Start
    void Start()
    {
        //flower monster properties
        health = 30;
        damage = 3; //hit damage, apply continuous poison D.O.T at 2 ticks per second or smth
        //seeker component
        seeker = GetComponent<Seeker>();
        //rigidbody
        rB = GetComponent<Rigidbody>();
        nextWayPointDistance = 2f;

        attackTimer = attackInterval;

        //targetting style
        tgtStyle = targetStyle.ClosestPlayer;
        player = base.reacquireTgt(tgtStyle, this.gameObject);
    }

    //Update
    void Update()
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


        //if attackTimer is not over yet
        if (attackTimer >= 0)
        {
            if (path.GetTotalLength() > 5f)
            {


                //look & move
                dir = (path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1] - transform.position).normalized;

                Vector3 look = dir.normalized;
                look.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(look);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);

                rB.velocity = transform.forward * speed;




                //dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
                ////factor in the speed to move at
                //dir *= speed;
                ////move
                //rB.Move(dir * Time.deltaTime);

                ////look
                //Vector3 look = target;
                //look.y = transform.position.y;
                //transform.LookAt(look);
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

                    //look & move
                    dir = (path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1] - transform.position).normalized;

                    Vector3 look = dir.normalized;
                    look.y = 0;
                    Quaternion targetRotation = Quaternion.LookRotation(look);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);

                    rB.velocity = transform.forward * speed;


                    ////set the direction to move to
                    ////dir = (path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1] - transform.position).normalized;
                    //dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
                    ////factor in the speed to move at
                    //dir *= speed;
                    ////move
                    //rB.Move(dir * Time.deltaTime);
                }
            }

        }

        //shot prediction debug ray
        Vector3 aimBot = (interceptPoint - transform.position);
        aimBot.y = 0;
        Debug.DrawRay(transform.position, aimBot.normalized * 15f, Color.magenta);

        //update the waypoint on the path once the current one has been reached
        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < nextWayPointDistance)
        {
            currentWayPoint++;
            return;
        }
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

    //Attack
    protected override void Attack()
    {
        //calculate where to shoot
        Vector3 calc = CalculateInterception(player.transform.position, player.GetComponent<Rigidbody>().velocity, transform.GetChild(5).position, 10f);
        if (calc != Vector3.zero)
        {
            calc.Normalize();
            interceptionTime = GetApproachingPoint(player.transform.position, player.GetComponent<Rigidbody>().velocity, transform.GetChild(5).position, calc * 10f);
            interceptPoint = player.transform.position + player.transform.up + player.GetComponent<Rigidbody>().velocity * interceptionTime;
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

        //look
        Vector3 look = here;
        look.y = transform.position.y;
        transform.LookAt(look);
        
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

}
