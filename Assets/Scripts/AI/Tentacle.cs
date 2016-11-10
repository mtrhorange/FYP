using UnityEngine;
using System.Collections;

public class Tentacle : Enemy
{
    //Variables
    public bool attacking;
    public float lifeTime = 45f;
    public TentacleBoss Boss;

	//Start
    protected override void Start()
    {
        base.Start();
        //Tentacle properties
        health = 30;
        damage = 4;

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

    protected override void Attack()
    {
        //TODO: SMACK DOWN
        if (transform.localScale.y <= 12f && attacking)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + Time.deltaTime * 8f, transform.localScale.z);

            if (transform.localScale.y >= 12f)
            {
                attacking = false;
            }
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - Time.deltaTime * 8, transform.localScale.z);
            if (transform.localScale.y <= 3f)
            {
                transform.localScale = new Vector3(transform.localScale.x, 3, transform.localScale.z);
                myState = States.Idle;
            }
        }

        if (Vector3.Distance(this.transform.position, player.transform.position) > 3f)
        {
            transform.localScale = new Vector3(transform.localScale.x, 3, transform.localScale.z);
            myState = States.Idle;
        }
    }

    protected override void Idle()
    {
        //look at player
        if (Vector3.Distance(this.transform.position, player.transform.position) < 6f)
        {
            Vector3 sight = (player.transform.position - transform.position);
            sight.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(sight);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
        }


        if (Vector3.Distance(this.transform.position, player.transform.position) < 3f)
        {
            myState = States.Attack;
            attacking = true;
        }

        //reduce lifetime only in idle
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            //die
            Boss.TentacleDeath(this);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color32(255, 0, 0, 90);
        Gizmos.DrawSphere(this.transform.position,3f);
    }
}
