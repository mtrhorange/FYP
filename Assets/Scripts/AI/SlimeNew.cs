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
    private Animator anim;
    //attack variables
    public float attackInterval = 0.5f;
    private float attackTimer;
    private bool attacking = false;
    private float jumpTimer = 0.3f;

	//Start
    protected override void Start()
    {
        myStrength = bigger ? Strength.Medium : Strength.Weak;

        anim = GetComponent<Animator>();

        base.Start();
        //slime properties
        transform.localScale = bigger ? new Vector3(1.8f, 1.8f, 1.8f) : transform.localScale;
        transform.position = new Vector3(transform.position.x, transform.localScale.y * 0.5f, transform.position.z);
        //get our seeker component
        seeker = GetComponent<Seeker>();
        //get rigidbody
        rB = GetComponent<Rigidbody>();

        attackTimer = attackInterval;

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
            Attack();
        }

        jumpTimer -= Time.deltaTime;
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

        //if can attack aledy
        if (/*attackTimer <= 0 && */(player.transform.position - transform.position).magnitude <= 1.8f)
        {
            //if close enough to the player
            //proceed to attack
            anim.SetBool("Move", false);
            anim.SetTrigger("Attack");
            myState = States.Attack;
        }
        //continue chasing
        else
        {
            nextPathPoint =
               path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];

            anim.SetBool("Move", true);

            //look & move
            dir = velocity;

            Vector3 look = dir.normalized + AvoidObstacle();
            look.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(look);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
            rB.velocity = transform.forward * speed;
        }
       // attackTimer -= Time.deltaTime;
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
        anim.SetBool("Move", false);
        anim.SetTrigger("Take Damage");
    }

    //Flinch End Animation Event callback override
    public override void FlinchEnd()
    {
        pathUpdateTimer = 0;
        pathUpdate();
        myState = States.Chase;
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
            anim.SetTrigger("Die");
            GetComponent<BoxCollider>().enabled = false;
            base.Death();
        }
        else
        {
            anim.SetTrigger("Die");
            GetComponent<BoxCollider>().enabled = false;
            base.Death();
        }
    }

    //Attack
    protected override void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            anim.SetBool("Move", false);
            anim.SetTrigger("Attack");
            rB.velocity = Vector3.zero;
        }
    }

    //attack event 1
    public void AttackEvent1()
    {
        GetComponent<BoxCollider>().enabled = true;
        SFXManager.instance.playSFX(sounds.slime);
    }

    //attack event 2
    public void AttackEvent2()
    {
        //reset attack interval and state to chase
        GetComponent<BoxCollider>().enabled = false;
        attackTimer = attackInterval;
        myState = States.Chase;
        attacking = false;
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
