using UnityEngine;
using System.Collections;

public class Tentacle : Enemy
{
    //Variables
    public bool attacking;
    public float lifeTime = 45f;
    public TentacleBoss Boss;
    public float attackInterval;
    private float attackTimer;
    private Animator anim;

    //Start
    protected override void Start()
    {
        base.Start();
        //Tentacle properties
        health = 30;
        damage = 4;
        anim = GetComponent<Animator>();
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
        else if (myState == States.Attack)
        {
            Attack();
        }

    }

    protected override void Attack()
    {
        anim.SetTrigger("Slap Attack");
        Vector3 sight = (player.transform.position - transform.position);
        sight.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(sight);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
        myState = States.Idle;

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
        attackTimer = attackInterval;
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

        attackTimer -= Time.deltaTime;

        if (Vector3.Distance(transform.position, player.transform.position) < 4f && attackTimer < 0)
        {
            myState = States.Attack;
            attackTimer = attackInterval;
        }
    }
}
