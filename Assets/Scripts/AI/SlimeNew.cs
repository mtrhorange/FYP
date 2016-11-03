using UnityEngine;
using System.Collections;
using Pathfinding;

public class SlimeNew : Enemy {

    //'bigger' boolean controls whether the slime will be a bigger variant that splits into 2 normal slimes upon death
    public bool bigger;
    public GameObject slimePrefab;
    //rigidbody
    private Rigidbody rB;
    //movement variables
    private float pathUpdateTimer = 0.5f, minDistance = 2.0f;
    private Vector3 dir = Vector3.zero;
    //attack variables
    private bool attacking = false;

	//Start
	void Start ()
    {
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
                StartCoroutine(Attack());
            }
        }
        else if (myState == States.Dead)
        {
        }

        //testing
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    ReceiveDamage(5);
        //    if (health <= 0)
        //    {
        //        myState = States.Dead;

        //        //check if is a biger variant
        //        if (bigger)
        //        {
        //            //split into 2 normal sized slimes
        //            Instantiate(slimePrefab, new Vector3(transform.position.x + 2.5f, 1, transform.position.z), transform.rotation);
        //            Instantiate(slimePrefab, new Vector3(transform.position.x - 2.5f, 1, transform.position.z), transform.rotation);
        //            Destroy(this.gameObject);
        //        }
        //        else
        //        {
        //            //do death
        //            Destroy(this.gameObject);
        //        }
        //    }
        //}
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
        Debug.DrawRay(transform.position + transform.up, (player.transform.position - transform.position).normalized * 2f, Color.magenta);

        //check first if close enough to the player
        //if yes proceed to attack
        if ((player.transform.position - transform.position).magnitude <= 2f)
        {
            attacking = true;
            myState = States.Attack;
        }
        //continue chasing
        else
        {
            //look & move
            dir = AvoidObstacle();
            Vector3 look = dir.normalized;
            look.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(look);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
            rB.velocity = transform.forward * speed;
        }
    }

    //attack, override next time when got model + animation
    private IEnumerator Attack()
    {
        Debug.Log("i attack u");
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
