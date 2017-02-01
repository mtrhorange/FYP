using UnityEngine;
using System.Collections;

public class Tentacle : Enemy
{
    //Variables
    public bool attacking;
    public float lifeTime = 45f;
    public TentacleBoss Boss;
    public float attackInterval;
    private float attackTimer, reTargetTimer = 3f;
    private Animator anim;

    //Start
    protected override void Start()
    {
        myStrength = Strength.Weak;
        
        base.Start();

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

    //Attack override
    protected override void Attack()
    {
        anim.SetTrigger("Slap Attack");
        Vector3 sight = (player.transform.position - transform.position);
        sight.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(sight);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8);
        myState = States.Idle;

    }

    //Death override
    protected override void Death()
    {
        anim.SetTrigger("Die");
        GetComponent<BoxCollider>().enabled = false;
        Boss.TentacleDeath(this);
        base.Death();
    }

    //Flinch override
    protected override void Flinch()
    {
        base.Flinch();
        GetComponent<BoxCollider>().enabled = false;
        //play flinch animaton
        anim.SetTrigger("Take Damage");
    }

    //Flinch End Animation Event callback override
    public override void FlinchEnd()
    {
        myState = States.Idle;
    }

    //receive damage override
    public override void ReceiveDamage(float dmg, Player attacker)
    {
        if (myState != States.Dead)
        {
            //receive damageee
            base.ReceiveDamage(dmg, attacker);
            //give damage to the main body
            Boss.ReceiveDamage(Mathf.Ceil((Boss.health / Boss.maxHealth) * 0.35f * dmg), attacker);
        }
    }

    //attacking trigger on
    public void triggerOn()
    {
        GetComponent<BoxCollider>().enabled = true;
    }
    //attacking trigger off
    public void triggerOff()
    {
        attackTimer = attackInterval;
        myState = States.Idle;
        GetComponent<BoxCollider>().enabled = false;
        attackTimer = attackInterval;
    }

    //re-target
    private void ReTarget()
    {
        reTargetTimer -= Time.deltaTime;

        if (reTargetTimer <= 0)
        {
            player = base.reacquireTgt(tgtStyle, this.gameObject);
            reTargetTimer = 1f;
        }
    }

    //Idle override
    protected override void Idle()
    {
        ReTarget();

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
