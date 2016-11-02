using UnityEngine;
using System.Collections;
using Pathfinding;

public class Slime : Enemy {

    //'bigger' boolean controls whether the slime will be a bigger variant that splits into 2 normal slimes upon death
    public bool bigger;
    public GameObject slimePrefab;
    //character controller
    private CharacterController charCon;
    //timers
    private float chaseTimer = 0;
    //movement variables
    private bool gravityOn;
    private Vector3 lastPos, dir = Vector3.zero;
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
        //get character controller
        charCon = GetComponent<CharacterController>();

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

    //fixed update for debug rays and lines
    private void FixedUpdate()
    {
        //attack trigger distance debug ray
        Debug.DrawRay(transform.position + transform.up, (player.transform.position - transform.position).normalized * 2f, Color.magenta);
    }

    //Idle state
    protected override void Idle()
    {
        base.Idle();
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

        //if this entity on the ground
        if (charCon.isGrounded)
        {
            //check first if catched up to the player
            //if yes proceed to attack
            if ((player.transform.position - transform.position).magnitude <= 2f)
            {
                myState = States.Attack;
            }
            //if not, continue chasing
            else
            {
                //if chaseTimer is bigger than zero, continue counting down
                if (chaseTimer >= 0)
                {
                    chaseTimer -= Time.deltaTime;
                }
                //else if it is smaller than zero
                else
                {
                    //chase target
                    target = player.transform.position;
                    //set a path to tgt position
                    seeker.StartPath(transform.position, target, OnPathComplete);
                    currentWayPoint = 0;
                    //set the direction to move to
                    dir = (path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1] - transform.position).normalized;
                    dir.y = 1.5f;
                    //simulate gravity
                    dir.y -= 9.8f * Time.deltaTime;
                    //factor in the speed to move at
                    dir *= speed;
                    //move
                    charCon.Move(dir * Time.deltaTime);
                    

                    gravityOn = false;
                }
            }
        }
        //if this entity is off the ground
        else
        {
            //update the direction to move to
            dir = (path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1] - transform.position).normalized;
            //"jump"
            if (transform.position.y < 2f && !gravityOn)
            {
                dir.y = 1.5f;
            }
            //when reached the height of jump, switch on gravity
            else
            {
                gravityOn = true;
                dir.y -= 9.8f * Time.deltaTime;
            }
            dir *= speed;
            charCon.Move(dir * Time.deltaTime);
            chaseTimer = 1f;
        }

        //look
        Vector3 look = dir;
        look.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);

        //update the waypoint on the path once the current one has been reached
        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < nextWayPointDistance)
        {
            currentWayPoint++;
            return;
        }
    }
}
