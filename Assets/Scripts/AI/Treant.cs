using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class Treant : Enemy {

    //Rigidbody
    private Rigidbody rB;
    //timers
    private float pathUpdateTimer = 3f;
    public float attackInterval = 3f;
    private float summonInterval = 45f, summonTimer;

    public float attackTimer;
    public GameObject shockwave, summonEffect, HPBarPrefab;
    private GameObject HpBar;
    private Vector3 heightOffset;
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

        heightOffset = transform.up;
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

        //targetting style
        tgtStyle = targetStyle.ClosestPlayer;
        player = base.reacquireTgt(tgtStyle, this.gameObject);

        //setup canvas HP
        HpBar = (GameObject)Instantiate(HPBarPrefab, GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
        HpBar.GetComponent<BossHpBar>().boss = this.gameObject;
        HpBar.GetComponent<BossHpBar>().UpdateHPBar();
	}
	
	// Update is called once per frame
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
        Debug.DrawRay(transform.position + transform.up, (player.transform.position - transform.position).normalized * 3f, Color.cyan);
        Debug.DrawRay(transform.position + transform.up, velocity, Color.magenta);
        attackTimer -= Time.deltaTime;

        //if cooldown for summon (summonTimer) is over, summon 2 minions to aid in battle
        if (summonTimer <= 0)
        {
            if (!attacking)
            {
                anim.SetBool("Walk", false);
                rB.velocity = Vector3.zero;
                anim.SetTrigger("Spell Cast");
                attacking = true;
            }
        }
        //if attack timer is up
        else if (attackTimer <= 0)
        {
            //close enough to club smash
            if ((player.transform.position - transform.position).magnitude <= 2.5f)
            {
                myState = States.Attack;
                anim.SetBool("Walk", false);
                
                rB.velocity = Vector3.zero;
                attacking = true;
                
            }
            //else if close enough to jump attack
            else if ((player.transform.position - transform.position).magnitude > 2.5f &&
                     (player.transform.position - transform.position).magnitude < 10f)
            {
                anim.SetBool("Walk", false);
                rB.velocity = Vector3.zero;
                attacking = true;
                if (attacking)
                {
                    jumpAttack();
                    attacking = false;
                }
            }
            //else is walk
            else
            {
                anim.SetBool("Walk", true);
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
        //else is walk
        else 
        {
            if ((player.transform.position - transform.position).magnitude <= 2.5f)
            {
                anim.SetBool("Walk", false);
            }
            else
            {
                anim.SetBool("Walk", true);
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

        anim.SetTrigger("Attack");
        attacking = false;
        attackTimer = attackInterval;
        myState = States.Chase;
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
            anim.SetBool("Walk", false);
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
        myState = States.Chase;
    }

    //Death override
    protected override void Death()
    {
        anim.SetTrigger("Die");
        GetComponent<BoxCollider>().enabled = false;
        Destroy(HpBar, 1f);
        base.Death();
    }

    //summon minion (anim event callback)
    void summonShit()
    {
        //summon 2 nature themed monsters to aid in battle
        GameObject temp = (GameObject)Instantiate(summonEffect, new Vector3(transform.position.x + 4f, transform.position.y, transform.position.z), Quaternion.Euler(-90, 0, 0));
        temp.GetComponent<EnemySummon>().typeToSpawn = mobType.Flower;
        GameObject temp2 = (GameObject)Instantiate(summonEffect, new Vector3(transform.position.x - 4f, transform.position.y, transform.position.z), Quaternion.Euler(-90, 0, 0));
        temp2.GetComponent<EnemySummon>().typeToSpawn = mobType.Plant;

        attackTimer = attackInterval;
        summonTimer = summonInterval;
        attacking = false;
        myState = States.Chase;
    }

    void jumpAttack()
    {
        Debug.Log("jumpman");
        anim.SetTrigger("Jump Attack");
    }

    void stopJump()
    {
        attackTimer = attackInterval;
        myState = States.Chase;
    }

    public void spawnShockwave()
    {
        attackTimer = attackInterval;
        GameObject sw = (GameObject)Instantiate(shockwave, new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z), Quaternion.identity);
        sw.GetComponent<Shockwave>().shockwaveDmg = Mathf.Floor(damage * 0.25f);
    }

    public void triggerOn()
    {
        rB.velocity = Vector3.zero;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void triggerOff()
    {
        attackTimer = attackInterval;
        myState = States.Chase;
        GetComponent<BoxCollider>().enabled = false;
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
        heightOffset = transform.up * 0.5f;
        RaycastHit Hit;
        //Check if there is obstacle
        Vector3 right45 = (transform.forward + transform.right).normalized;
        Vector3 left45 = (transform.forward - transform.right).normalized;

        //Shoot the rays!
        if (Physics.Raycast((transform.position + transform.up),
            right45 + heightOffset, out Hit, minDistance))
        {
            //if is obstacle
            if (Hit.transform.gameObject.layer == 8 ||
                (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType))
                return transform.forward - transform.right;
        }
                
        

        if (Physics.Raycast((transform.position + transform.up),
            left45 + heightOffset, out Hit, minDistance))
        {
            //if is obstacle
            if (Hit.transform.gameObject.layer == 8 ||
                (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType))
                return transform.forward - transform.right;
        }

        if (Physics.Raycast((transform.position + transform.up),
            transform.forward + heightOffset, out Hit, minDistance))
        {
            //if is obstacle
            if (Hit.transform.gameObject.layer == 8 ||
                (Hit.transform.GetComponent<Enemy>() && Hit.transform.GetComponent<Enemy>().myType != myType))
                return transform.forward - transform.right;
        }

        //right ray
        if (Physics.Raycast((transform.position) + heightOffset, transform.right.normalized + heightOffset, out Hit, 1.5f, 1 << 8))
        {
            transform.position += (-transform.right).normalized * 0.05f;
        }

        //left ray
        else if (Physics.Raycast((transform.position) + heightOffset, -transform.right.normalized + heightOffset, out Hit, 1.5f, 1 << 8))
        {
            transform.position += (transform.right).normalized * 0.05f;

        }
        return Vector3.zero;
    }


    void OnDrawGizmos()
    {
        heightOffset = transform.up*0.5f;
        Vector3 frontRay = transform.position + transform.forward * minDistance;
        Vector3 right45 = transform.position +
            (transform.forward + transform.right).normalized * minDistance;
        Vector3 left45 = transform.position +
            (transform.forward - transform.right).normalized * minDistance;

        Debug.DrawLine(transform.position + heightOffset, frontRay + heightOffset, Color.blue);
        Debug.DrawLine(transform.position + heightOffset, left45 + heightOffset, Color.blue);
        Debug.DrawLine(transform.position + heightOffset, right45 + heightOffset, Color.blue);
        Debug.DrawLine(transform.position + heightOffset, transform.position + transform.right.normalized * (minDistance - 0.5f) + heightOffset,
            Color.blue);
        Debug.DrawLine(transform.position + heightOffset, transform.position - transform.right.normalized * (minDistance - 0.5f) + heightOffset,
            Color.blue);

        //Gizmos.color = new Color32(255,0,0,40);
        //Gizmos.DrawSphere(this.transform.position,5f);
    }
}
