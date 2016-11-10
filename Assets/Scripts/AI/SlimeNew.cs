using UnityEngine;
using System.Collections;
using Pathfinding;

public class SlimeNew : Enemy {

    //'bigger' boolean controls whether the slime will be a bigger variant that splits into 2 normal slimes upon death
    public bool bigger;
    //rigidbody
    private Rigidbody rB;
    //movement variables
    private float pathUpdateTimer = 0.5f;
    private Vector3 dir = Vector3.zero;
    //attack variables
    private bool attacking = false;

	//Start
    protected override void Start()
    {
        base.Start();
        //slime properties
        health = bigger ? 50 : 25;
        damage = bigger ? 7 : 5;
        transform.localScale = bigger ? new Vector3(1.8f, 1.8f, 1.8f) : transform.localScale;
        transform.position = new Vector3(transform.position.x, transform.localScale.y * 0.5f, transform.position.z);
        //get our seeker component
        seeker = GetComponent<Seeker>();
        //get rigidbody
        rB = GetComponent<Rigidbody>();

        //targetting style
        tgtStyle = targetStyle.WeakestPlayer;
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
            if (!attacking)
            {
                attacking = true;
                Debug.Log("update");
                StartCoroutine(Attack());
            }
        }
        else if (myState == States.Dead)
        {
        }

        //testing
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ReceiveDamage(5);
        }
	}

    //Idle state
    protected override void Idle()
    {
        base.Idle();
    }

    //Chase
    protected override void Chase()
    {
        pathUpdate();

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

        //update the waypoint on the path once the current one has been reached
        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < nextWayPointDistance)
        {
            currentWayPoint++;
            return;
        }

        //attack trigger distance debug ray
        Debug.DrawRay(transform.position + transform.up, velocity, Color.magenta);

        //check first if close enough to the player
        //if yes proceed to attack
        if ((player.transform.position - transform.position).magnitude <= 2f)
        {
            myState = States.Attack;
        }
        //continue chasing
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
            rB.velocity = transform.forward * speed;
        }
    }

    //Death override
    protected override void Death()
    {
        myState = States.Dead;
        //check if is a biger variant
        if (bigger)
        {
            //split into 2 normal sized slimes
            AIManager.instance.spawnMob(mobType.Slime, new Vector3(transform.position.x + 2.5f, 1, transform.position.z));
            AIManager.instance.spawnMob(mobType.Slime, new Vector3(transform.position.x - 2.5f, 1, transform.position.z));
            AIManager.instance.RemoveMe(this.gameObject);
            Destroy(this.gameObject);
        }
        else
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().velocity = -transform.up * 8f;
            AIManager.instance.RemoveMe(this.gameObject);
            Destroy(this.gameObject, 5f);
        }
    }

    //attack, override next time when got model + animation
    private IEnumerator Attack()
    {
        //temporary attack action to be changed
        Vector3 scale = transform.localScale;
        scale.x += 0.25f;
        scale.y += 0.25f;
        scale.z += 0.25f;
        transform.localScale = scale;
        yield return new WaitForSeconds(0.25f);
        scale.x += 0.25f;
        scale.y += 0.25f;
        scale.z += 0.25f;
        transform.localScale = scale;
        yield return new WaitForSeconds(0.25f);
        scale.x += 0.25f;
        scale.y += 0.25f;
        scale.z += 0.25f;
        transform.localScale = scale;
        yield return new WaitForSeconds(0.25f);
        scale.x += 0.25f;
        scale.y += 0.25f;
        scale.z += 0.25f;
        transform.localScale = scale;
        GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(0.25f);
        scale.x -= 0.25f;
        scale.y -= 0.25f;
        scale.z -= 0.25f;
        transform.localScale = scale;
        yield return new WaitForSeconds(0.25f);
        scale.x -= 0.25f;
        scale.y -= 0.25f;
        scale.z -= 0.25f;
        transform.localScale = scale;
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.25f);
        scale.x -= 0.25f;
        scale.y -= 0.25f;
        scale.z -= 0.25f;
        transform.localScale = scale;
        yield return new WaitForSeconds(0.25f);
        scale.x -= 0.25f;
        scale.y -= 0.25f;
        scale.z -= 0.25f;
        transform.localScale = scale;
        myState = States.Chase;
        attacking = false;
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

    //Avoid Obstacles
    protected Vector3 AvoidObstacle()
    {
        Vector3 destPos =
            path.vectorPath[currentWayPoint];
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
            if (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType)
            {

                Physics.IgnoreCollision(GetComponent<Collider>(), Hit.transform.GetComponent<Collider>());
            }

            //if is obstacle
            if (Hit.transform.gameObject.layer == 8)
                return transform.forward + Hit.normal;
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
        Debug.DrawLine(transform.position, transform.position + transform.right.normalized * 1.5f, Color.blue);
        Debug.DrawLine(transform.position, transform.position - transform.right.normalized * 1.5f, Color.blue);

        //Gizmos.color = new Color32(255,0,0,40);
        //Gizmos.DrawSphere(this.transform.position,5f);
    }
}
