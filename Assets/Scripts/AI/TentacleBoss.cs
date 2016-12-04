﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TentacleBoss : Enemy {

    //tentacle prefab
    public bool spawn;
    public float SpawnX = 4f, SpawnZ = 4f;
    public GameObject tentaclePref;
    public float attackInterval = 3f;
    public GameObject mouth;
    public GameObject projectile;
    //list to keep track of tentacles
    private List<Tentacle> tentacles;
    private float spawnTimer = 5f;
    public float attackTimer;
    private Animator anim;
    private bool attacking;
    private float interceptionTime = 0f;
    private Vector3 interceptPoint = Vector3.zero;

    //Start
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        //Tentacle Boss properties
        health = 500;
        damage = 12;

        //tentacles
        tentacles = new List<Tentacle>();
        tentacles.AddRange(GetComponentsInChildren<Tentacle>());

        //targetting style
        tgtStyle = targetStyle.ClosestPlayer;
        player = base.reacquireTgt(tgtStyle, this.gameObject);
	}
	
	//Update
	void Update ()
    {
        if (spawn)
        {
            spawnTentacle();
            spawn = false;
        }

        if (myState == States.Idle)
        {
            Idle();
        }
        else if (myState == States.Attack)
        {
            Attack();
        }

        spawnTimer -= Time.deltaTime;
    }

    protected override void Idle()
    {
        //look at player
        if (Vector3.Distance(this.transform.position, player.transform.position) < 15f)
        {
            Vector3 sight = (player.transform.position - transform.position);
            sight.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(sight);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
        }

        //spawn tentacles (maximum of 6) when spawn timer is up
        if (Vector3.Distance(transform.position, player.transform.position) < 20f)
        {
            if (spawnTimer <= 0)
            {
                if (tentacles.Count < 6)
                {
                    spawnTentacle();
                }
                spawnTimer = 8f;
            }
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer < 0)
        {
            myState = States.Attack;
            attackTimer = attackInterval;
        }
    }

    //Spawn tentacle
    void spawnTentacle()
    {
        //spawn a tentacle within a given area of of the player.
        float randomX = Random.Range(player.transform.position.x + SpawnX, player.transform.position.x - SpawnX);
        float randomZ = Random.Range(player.transform.position.z + SpawnZ, player.transform.position.z - SpawnZ);
        Vector3 spawnLocation = new Vector3(randomX, 1.4f, randomZ);

        GameObject temp = (GameObject)Instantiate(tentaclePref, spawnLocation, Quaternion.identity);
        tentacles.Add(temp.GetComponent<Tentacle>());

        //reacquire closer target
        player = base.reacquireTgt(tgtStyle, this.gameObject);
    }

    //Attack
    protected override void Attack()
    {

        int r = Random.Range(0, 2);

        if (Vector3.Distance(transform.position, player.transform.position) < 4f )
        {
            anim.SetTrigger("Bite Attack");
            Vector3 sight = (player.transform.position - transform.position);
            sight.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(sight);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*8);
        }
        else if (Vector3.Distance(transform.position, player.transform.position) > 4f &&
                 Vector3.Distance(transform.position, player.transform.position) < 12f)
        {
            projectileAttack();
        }
        else
        {
            myState = States.Idle;
            return;
        }


        myState = States.Idle;
        


    }

    private void projectileAttack()
    {
        //TODO: PROJECTILE ATTACK
        if (!attacking)
        {
            attacking = true;
            anim.SetTrigger("Projectile Attack");
        }

    }

    private void shoot(Vector3 here)
    {
        Vector3 shootHere = (here - mouth.transform.position).normalized;

        GameObject boo = (GameObject)Instantiate(projectile, mouth.transform.position, Quaternion.identity);
        boo.GetComponent<Rigidbody>().velocity = shootHere * 10f;
        boo.GetComponent<EnemyProjectiles>().target = here;
    }

    //attack event 1
    public void AttackEvent1()
    {
        float offset;
        //offset for the shot
        if (player.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            offset = Random.Range(2.8f, 2.8f);
        }
        else
        {
            offset = 0f;
        }

        //direction for offset
        Vector3 towards = new Vector3(Random.Range(0f, 1f) * 2 - 1, 0, Random.Range(0f, 1f) * 2 - 1);

        //calculate where to shoot
        Vector3 calc = CalculateInterception(player.transform.position + player.transform.up + towards.normalized * offset, player.GetComponent<Rigidbody>().velocity, mouth.transform.position, 10f);
        if (calc != Vector3.zero)
        {
            calc.Normalize();
            interceptionTime = GetApproachingPoint(player.transform.position + player.transform.up + towards.normalized * offset, player.GetComponent<Rigidbody>().velocity, mouth.transform.position, calc * 10f);
            interceptPoint = (player.transform.position + player.transform.up + towards.normalized * offset) + player.GetComponent<Rigidbody>().velocity * interceptionTime;
        }
        shoot(interceptPoint);
    }

    //attack event 2
    public void AttackEvent2()
    {
        //reset attack interval and state to chase
        attackTimer = attackInterval;
        myState = States.Idle;
        attacking = false;
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

    public void triggerOn()
    {

        GetComponent<BoxCollider>().enabled = true;
    }

    public void triggerOff()
    {
        attackTimer = attackInterval;
        myState = States.Idle;
        GetComponent<BoxCollider>().enabled = false;
    }

    //tentacle death
    public void TentacleDeath(Tentacle me)
    {
        tentacles.Remove(me);
        Destroy(me.gameObject);
    }



    public void OnDrawGizmos()
    {

    }


}
