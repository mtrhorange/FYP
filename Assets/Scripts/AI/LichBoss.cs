using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class LichBoss : Enemy
{
    //Rigidbody
    private Rigidbody rB;
    //timers
    private float pathUpdateTimer = 3f;
    public float attackInterval = 3f, attackTimer;
    public float shootInterval = 8f, shootTimer;
    private float summonInterval = 45f, summonTimer;

    public GameObject summonEffect, SkullMissile, HPBarPrefab;
    private GameObject HpBar;

    //movement variables
    private Vector3 dir;
    private Animator anim;
    private bool attacking = false;
    //flinch variables
    public float damagedAmount, flinchThreshold;
    public float flinchTimer = 5f;

    // Use this for initialization
    protected override void Start()
    {
        myStrength = Strength.Boss;

        anim = GetComponent<Animator>();

        base.Start();

        //set flinch threshold to 15% of max Hp?
        flinchThreshold = 0.15f * health;

        //seeker component
        seeker = GetComponent<Seeker>();
        //rigidbody
        rB = GetComponent<Rigidbody>();
        nextWayPointDistance = 3f;

        //attack timers
        attackTimer = attackInterval;
        summonTimer = summonInterval;
        shootTimer = shootInterval;

        minDistance = 4.0f;

        //targetting style
        tgtStyle = targetStyle.ClosestPlayer;
        player = base.reacquireTgt(tgtStyle, this.gameObject);

        //setup canvas HP
        HpBar = (GameObject)Instantiate(HPBarPrefab, GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
        HpBar.GetComponent<BossHpBar>().boss = this.gameObject;
        HpBar.GetComponent<BossHpBar>().UpdateHPBar();
    }

    // Update is called once per frame
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

        //flinch time window
        if (flinchTimer <= 0)
        {
            flinchTimer = 5f;
            damagedAmount = 0f;
        }
        flinchTimer -= Time.deltaTime;
        summonTimer -= Time.deltaTime;
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

        //attack trigger distance debug ray
        Debug.DrawRay(transform.position + transform.up, (player.transform.position - transform.position).normalized * 6f, Color.cyan);
        Debug.DrawRay(transform.position + transform.up, velocity, Color.magenta);
        attackTimer -= Time.deltaTime;
        shootTimer -= Time.deltaTime;

        //if cooldown for summon (summonTimer) is over, summon 2 minions to aid in battle
        if (summonTimer <= 0)
        {
            if (!attacking)
            {
                anim.SetBool("Fly Forward", false);
                rB.velocity = Vector3.zero;
                anim.SetTrigger("Cast Spell");
                attacking = true;
            }
        }
        else if (shootTimer <= 0)
        {
            //shoot at player(s)
            if (!attacking)
            {
                myState = States.Attack;
                anim.SetBool("Fly Forward", false);
                attacking = true;
                rB.velocity = Vector3.zero;
                anim.SetTrigger("Spin Attack");
            }
        }
        //if attack timer is up
        else if (attackTimer <= 0)
        {
            //close enough to attack
            if ((player.transform.position - transform.position).magnitude <= 6f)
            {
                myState = States.Attack;
                anim.SetBool("Fly Forward", false);
                rB.velocity = Vector3.zero;
                attacking = true;
                anim.SetTrigger("Melee Attack");
            }
            else
            {
                anim.SetBool("Fly Forward", true);
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
        //else move to player
        else
        {
            if ((player.transform.position - transform.position).magnitude <= 2.5f)
            {
                anim.SetBool("Fly Forward", false);
            }
            else
            {
                anim.SetBool("Fly Forward", true);
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
            if ((player.transform.position - transform.position).magnitude >= 2.5f)
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

    //receive damage override
    public override void ReceiveDamage(float dmg, Player attacker)
    {
        damagedAmount += dmg;
        base.ReceiveDamage(dmg, attacker);
        HpBar.GetComponent<BossHpBar>().UpdateHPBar();
    }

    //Attack
   protected override void Attack()
    {
        Vector3 look = player.transform.position - transform.position;

        look.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(look);
        transform.rotation = targetRotation;

        pathUpdateTimer = 0f;

        rB.velocity = Vector3.zero;
    }

    //Death override
    protected override void Death()
    {
        anim.SetTrigger("Die");
        GetComponent<BoxCollider>().enabled = false;
        Destroy(HpBar, 1f);
        base.Death();
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
            attacking = false;
            //play flinch animaton
            anim.SetBool("Fly Forward", false);
            anim.SetTrigger("Take Damage");
        }
    }

    //Flinch End Animation Event callback override
    public override void FlinchEnd()
    {
        pathUpdateTimer = 0;
        pathUpdate();
        flinchTimer = 5f;
        damagedAmount = 0f;
        GetComponent<BoxCollider>().enabled = false;
        myState = States.Chase;
    }

    //attack event 1
    public void AttackEvent1(int f)
    {
        rB.velocity = Vector3.zero;
        if (f == 1)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
        else if (f == 2)
        {
            //Shoot Skull Missile
            ShootSkullMissiles();
        }
    }

    //attack event 2
    public void AttackEvent2(int f)
    {
        //reset interval and state to chase
        if (f == 1)
        {
            GetComponent<BoxCollider>().enabled = false;
            attackTimer = attackInterval;
        }
        else if (f == 2)
        {
            shootTimer = shootInterval;
        }
        myState = States.Chase;
        attacking = false;
    }

    //Skull Missiles
    private void ShootSkullMissiles()
    {
        GameObject shootAtMe = player;
        bool fireAtTwoPlayers = false;
        //target
        if (GameManager.instance.twoPlayers)
        {
            //both alive
            if (GameManager.instance.player2.Health > 0f && GameManager.instance.player1.Health > 0f)
            {
                fireAtTwoPlayers = true;
            }
            //only player 2 alive
            else if (GameManager.instance.player1.Health <= 0f && GameManager.instance.player2.Health > 0f)
            {
                shootAtMe = GameManager.instance.player2.gameObject;
            }
            //only player 1 alive
            else if (GameManager.instance.player2.Health <= 0f && GameManager.instance.player1.Health > 0f)
            {
                shootAtMe = GameManager.instance.player1.gameObject;
            }
        }

        //shoot 3 skulls
        GameObject g = (GameObject)Instantiate(SkullMissile, transform.position + transform.up + transform.up + transform.forward, transform.rotation);
        g.GetComponent<LichSkullMissiles>().chaseThis = shootAtMe;
        g.GetComponent<LichSkullMissiles>().damage = damage;
        g = (GameObject)Instantiate(SkullMissile, transform.position + transform.up + transform.up + transform.forward + transform.right, transform.rotation * Quaternion.Euler(0, 40, 0));
        if (fireAtTwoPlayers)
        { 
            if (shootAtMe == GameManager.instance.player1.gameObject)
            {
                shootAtMe = GameManager.instance.player2.gameObject;
            }
            else if (shootAtMe == GameManager.instance.player2.gameObject)
            {
                shootAtMe = GameManager.instance.player1.gameObject;
            }
        }
        g.GetComponent<LichSkullMissiles>().chaseThis = shootAtMe;
        g.GetComponent<LichSkullMissiles>().damage = damage;
        g = (GameObject)Instantiate(SkullMissile, transform.position + transform.up + transform.up + transform.forward - transform.right, transform.rotation * Quaternion.Euler(0, -40, 0));
        g.GetComponent<LichSkullMissiles>().chaseThis = shootAtMe;
        g.GetComponent<LichSkullMissiles>().damage = damage;
    }

    //FIX ENEMIES TO SPAWN WHEN ZM FINISH SKELETONS
    //summon minions (anim event callback)
    private void Summon()
    {
        //choose 2 out of the 3 undead themed monsters
        mobType Alpha = (mobType)Random.Range(3, 6);
        mobType Beta = (mobType)Random.Range(3, 6);

        //summon them to aid in battle
        GameObject temp = (GameObject)Instantiate(summonEffect, new Vector3(transform.position.x + 4f, transform.position.y, transform.position.z), Quaternion.Euler(90,0,0));
        temp.GetComponent<EnemySummon>().typeToSpawn = Alpha;
        GameObject temp2 = (GameObject)Instantiate(summonEffect, new Vector3(transform.position.x - 4f, transform.position.y, transform.position.z), Quaternion.Euler(90, 0, 0));
        temp2.GetComponent<EnemySummon>().typeToSpawn = Beta;

        attackTimer = attackInterval;
        summonTimer = summonInterval;
        attacking = false;
        myState = States.Chase;
    }

    //update calculated path every set time
    public void pathUpdate()
    {
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

        nextPathPoint.y = 0;
        pathUpdateTimer -= Time.deltaTime;
    }

    //Avoid Obstacles
    protected Vector3 AvoidObstacle()
    {
        RaycastHit Hit;
        //Check if there is obstacle
        Vector3 right45 = (transform.forward + transform.up + transform.right).normalized;
        Vector3 left45 = (transform.forward + transform.up - transform.right).normalized;

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
            transform.forward + transform.up, out Hit, minDistance))
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
        if (Physics.Raycast((transform.position + transform.up), transform.right.normalized, out Hit, 1.5f, 1 << 8))
        {
            transform.position += (-transform.right).normalized * 0.05f;
        }

        //left ray
        else if (Physics.Raycast((transform.position + transform.up), -transform.right.normalized, out Hit, 1.5f, 1 << 8))
        {
            transform.position += (transform.right).normalized * 0.05f;

        }
        return Vector3.zero;
    }


    void OnDrawGizmos()
    {
        Vector3 frontRay = transform.position + transform.up + transform.forward * minDistance;
        Vector3 right45 = transform.position + transform.up +
            (transform.forward + transform.right).normalized * minDistance;
        Vector3 left45 = transform.position + transform.up +
            (transform.forward - transform.right).normalized * minDistance;

        Debug.DrawLine(transform.position + transform.up, frontRay, Color.blue);
        Debug.DrawLine(transform.position + transform.up, left45, Color.blue);
        Debug.DrawLine(transform.position + transform.up, right45, Color.blue);
        Debug.DrawLine(transform.position + transform.up, transform.position + transform.up + transform.right.normalized * 1.5f, Color.blue);
        Debug.DrawLine(transform.position + transform.up, transform.position + transform.up - transform.right.normalized * 1.5f, Color.blue);

        //Gizmos.color = new Color32(255,0,0,40);
        //Gizmos.DrawSphere(this.transform.position,5f);
    }
}
