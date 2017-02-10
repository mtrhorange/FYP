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
    public GameObject breath;
    //movement variables
    public float walkSpeed; //speed on ground
    private Vector3 dir;
    private bool flying = false, waitAnim = false;
    //animation component because this scrub uses old skool legacy anims
    private Animation anim;

	//Start
    protected override void Start()
    {
        myStrength = Strength.Strong;

        base.Start();
        //seeker component
        seeker = GetComponent<Seeker>();
        //rigidbody
        rB = GetComponent<Rigidbody>();
        //animation
        anim = GetComponent<Animation>();

        attackTimer = attackInterval;

        breath.GetComponent<EnemyProjectiles>().damage = damage;

        //targetting style
        tgtStyle = targetStyle.ClosestPlayer;
        player = base.reacquireTgt(tgtStyle, this.gameObject);        
	}
	
	//Update
	protected override void Update () 
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

        base.Update();
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
            //No path to move to yet
            return;
        }
        if (currentWayPoint >= path.vectorPath.Count)
        {
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

        //debug ray for attacking (breath) distance
        Debug.DrawRay(transform.position + transform.up, (player.transform.position - transform.position).normalized * 3f, Color.magenta);

        //check distance,
        //fly, walk depending on distance
        //Start flying
        if (triggered || path.GetTotalLength() <= 45f)
        {
            if (!triggered)
                triggered = true;

            if (path.GetTotalLength() > 15f && !flying)
            {
                flying = true;
                playAnim("flyBegin", 1, true);
            }
            //if close enough to bite
            else if ((transform.position - player.transform.position).magnitude <= 3f && !flying)
            {
                myState = States.Attack;
            }
            //close enough, check whether can use firebreath, if not land and walk
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
                        playAnim("walk", 2f, false);
                    }
                    //if flying, land first
                    else
                    {
                        waitAnim = true;
                        playAnim("land", 2f, true);
                    }
                }
            }
            nextPathPoint =
                    path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];
            //look & move
            dir = velocity.normalized + AvoidObstacle();
            Vector3 look = dir;
            look.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(look);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
            rB.velocity = transform.forward * (flying ? speed : walkSpeed);

            Debug.DrawRay(transform.position, look, Color.yellow);

            attackTimer -= Time.deltaTime;
        }
    }

    //Flinch override
    protected override void Flinch()
    {
        base.Flinch();
        //stop moving
        rB.velocity = Vector3.zero;
        GetComponent<BoxCollider>().enabled = false;
        breath.SetActive(false);
        StopAllCoroutines();
        attacking = false;
        //play flinch animaton
        if (flying)
        {
            playAnim("fly_attack", 2f, true);
        }
        else
        {
            playAnim("idle_stretch", 8f, true);
        }
    }

    //Attack
    protected override void Attack()
    {
        //decide which attack to use depending on how close to the player
        if (!attacking)
        {
            //within close range (3f), use bite if not flying
            if ((transform.position - player.transform.position).magnitude <= 3f)
            {
                attacking = true;
                if (!flying)
                {
                    playAnim("bite", 1, true);
                }
            }
            //within medium range (10f), use fire breath
            else if ((transform.position - player.transform.position).magnitude <= 10f)
            {
                attacking = true;
                waitAnim = true;

                SFXManager.instance.playSFX(sounds.dragonFire);
                //if flying, breathe fire from the air
                if (flying)
                {
                    playAnim("fly_breath", 1, true);
                    breath.SetActive(true);
                }
                //if on the ground, breathe fire while standing
                else
                {
                    playAnim("stand_breath", 1, true);
                    breath.SetActive(true);
                }
            }
        }

        //look at target while attacking
        Vector3 look = (player.transform.position - transform.position).normalized;
        look.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 4);
    }

    //Death override
    protected override void Death()
    {
        if (flying)
        {
            playAnim("beginToFall", 1f, false);
            anim.PlayQueued("fallToGround");
        }
        else
        {
            playAnim("die", 1f, false);
        }

        GetComponent<BoxCollider>().enabled = false;
        breath.SetActive(false);
        StopAllCoroutines();
        base.Death();
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
                breath.SetActive(false);
                attackTimer = attackInterval;
                attacking = false;
                myState = States.Chase;
                break;
            case "bite":
                GetComponent<BoxCollider>().enabled = false;
                attacking = false;
                myState = States.Chase;
                break;
            case "flyBegin":
                playAnim("fly", 1, false);
                break;
            case "fly_attack":
            case "idle_stretch":
                pathUpdateTimer = 0;
                pathUpdate();
                attackTimer = attackInterval;
                myState = States.Chase;
                break;
        }
        waitAnim = false;
    }

    //Bite Animation Event callback
    public void BiteEvent()
    {
        GetComponent<BoxCollider>().enabled = true;
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
            if (player != null)
            {
                target = player.transform.position;
                //set a path to tgt position
                seeker.StartPath(transform.position, target, OnPathComplete);
            }
            else
            {
                path = null;
            }
            currentWayPoint = 1;
            pathUpdateTimer = 1f;
        }
    }

    //Avoid Obstacles
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
