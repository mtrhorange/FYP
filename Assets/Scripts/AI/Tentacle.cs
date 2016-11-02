using UnityEngine;
using System.Collections;

public class Tentacle : Enemy{

    
    //Tentacle type
    protected bool offensive = true;

	//Start
	void Start () {
	    player = GameObject.FindGameObjectWithTag("Player");
	}
	
	//Update
	void Update () {

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
        Debug.Log("SMACK DOWN");
    }

    protected override void Idle()
    {
        //TODO: LEPAK
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
        }

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color32(255, 0, 0, 90);
        Gizmos.DrawSphere(this.transform.position,3f);
    }
}
