using UnityEngine;
using System.Collections;

public class Tentacle : TentacleBoss{

    //Tentacle type
    protected bool offensive = true;

	//Start
	void Start () {
	
	}
	
	//Update
	void Update () {

        if (Vector3.Distance(this.transform.position, player.transform.position) < 3f)
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
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color32(255, 0, 0, 90);
        Gizmos.DrawSphere(this.transform.position,3f);
    }
}
