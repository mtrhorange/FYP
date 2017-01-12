using UnityEngine;
using System.Collections;
using Pathfinding;

public class DragonBoss : Enemy
{
    //Rigidbody
    private Rigidbody rB;
    //timers
    private float pathUpdateTimer = 0.5f, breathTimer, stompTimer, summonTimer;
    private float breathInterval = 13f, stompInterval = 6f, summonInterval = 40f;
    //attacking variables
    private bool attacking = false;
    public GameObject breath, fireBlast;
    private GameObject fireBlastRef;
    //flinch variables
    private float damagedAmount, flinchThreshold;
    private float flinchTimer = 5f;

    private enum attackType
    {
        GroundStomp,
        FireBreath,
        Summon
    }
    private attackType atkType;
    //movement variables
    private Vector3 dir;
    //animation component because this scrub uses old skool legacy anims
    private Animation anim;

    //Start
    protected override void Start()
    {
        myStrength = Strength.Boss;

        base.Start();

        //set flinch threshold to 15% of max Hp?
        flinchThreshold = 0.15f * health;

        //seeker component
        seeker = GetComponent<Seeker>();
        //rigidbody
        rB = GetComponent<Rigidbody>();
        //animation
        anim = GetComponent<Animation>();
        //attack timers
        breathTimer = breathInterval;
        stompTimer = stompInterval;
        summonTimer = summonInterval;

        minDistance = 4.0f;

        breath.GetComponent<EnemyProjectiles>().damage = damage;
        fireBlast.GetComponent<EnemyProjectiles>().damage = damage;

        //targetting style
        tgtStyle = targetStyle.ClosestPlayer;
        player = base.reacquireTgt(tgtStyle, this.gameObject);
    }

    //Update
    void Update()
    {
        //states
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

        //flinch time window
        if (flinchTimer <= 0)
        {
            flinchTimer = 5f;
            damagedAmount = 0f;
        }

        flinchTimer -= Time.deltaTime;
        breathTimer -= Time.deltaTime;
        stompTimer -= Time.deltaTime;
        summonTimer -= Time.deltaTime;
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
        Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * 15f, Color.magenta);
        
        //if close enough to ground stomp && cooldown (stompTimer) is over
        if ((transform.position - player.transform.position).magnitude <= 7f && stompTimer <= 0)
        {
            atkType = attackType.GroundStomp;
            myState = States.Attack;
        }
        //else if close enough to do firebreath (20f) && cooldown (breathTimer) is over
        else if (path.GetTotalLength() <= 15f && breathTimer <= 0)
        {
            //do breath if there is direct LOS to player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, 15f))
            {
                if (hit.transform.tag == "Player")
                {
                    atkType = attackType.FireBreath;
                    myState = States.Attack;
                }
            }
        }
        //else if cooldown for summon (summonTimer) is over, summon 2 minions to aid in battle
        else if (summonTimer <= 0)
        {
            atkType = attackType.Summon;
            myState = States.Attack;
        }

        playAnim("walk", 1.3f, false);

        nextPathPoint =
                path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];

        //look & move towards player slowly, the slow strut is highly intimidating
        dir = velocity.normalized + AvoidObstacle();
        Vector3 look = dir;
        look.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
        rB.velocity = transform.forward * speed;
    }

    //receive damage override
    public override void ReceiveDamage(float dmg, Player attacker)
    {
        damagedAmount += dmg;
        base.ReceiveDamage(dmg, attacker);
    }

    //Flinch override
    protected override void Flinch()
    {
        //check if should flinch
        if (damagedAmount >= flinchThreshold)
        {
            base.Flinch();
            //stop moving
            rB.velocity = Vector3.zero;
            breath.SetActive(false);
            StopAllCoroutines();
            attacking = false;
            //play flinch animaton
            playAnim("idle_stretch", 3f, true);
        }
    }

    //Attack
    protected override void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            //which attack to do
            //Ground Stomp
            if (atkType == attackType.GroundStomp)
            {
                playAnim("groundStomp", 1, true);
                //create fire blast
                fireBlastRef = (GameObject)Instantiate(fireBlast, transform.position + transform.forward * 7, transform.rotation);
                stompTimer = stompInterval;
            }
            //Fire Breath
            else if (atkType == attackType.FireBreath)
            {
                playAnim("stand_breath", 0.5f, true);
                breath.SetActive(true);
                breathTimer = breathInterval;
            }
            //Summon
            else if (atkType == attackType.Summon)
            {
                playAnim("summon 1", 1f, true);
                summonTimer = summonInterval;
            }

        }

        //look at target while attacking
        Vector3 look = (player.transform.position - transform.position).normalized;
        look.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2);
    }

    //Death override
    protected override void Death()
    {
        playAnim("die", 1f, false);
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
        switch (name)
        {
            case "groundStomp":
                attacking = false;
                myState = States.Chase;
                break;
            case "stand_breath":
                attacking = false;
                myState = States.Chase;
                break;
            case "summon 1":
                AIManager.instance.spawnMob(mobType.Dragon, new Vector3(transform.position.x + 4f, 1, transform.position.z));
                AIManager.instance.spawnMob(mobType.DragonUndead, new Vector3(transform.position.x - 4f, 1, transform.position.z));
                myState = States.Chase;
                break;
            case "idle_stretch":
                pathUpdateTimer = 0;
                pathUpdate();
                flinchTimer = 5f;
                damagedAmount = 0f;
                myState = States.Chase;
                break;
        }
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

    //Ground Stomp animation event
    public void GroundStompEvent(int on)
    {
        fireBlastRef.GetComponent<BoxCollider>().enabled = on == 1 ? true : false;
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
