using UnityEngine;
using System.Collections;

public class EnemyLeftBehinds : MonoBehaviour {
    //damage
    public float dmg = 0;
    public EnemyProjectiles.type typ;


	//Start
	void Start()
    {
	}
	
	//Update
	void Update()
    {
	    if (typ == EnemyProjectiles.type.WebShot)
        {
            if (GetComponent<ParticleSystem>().time / GetComponent<ParticleSystem>().duration >= 0.7f)
            {
                GetComponent<BoxCollider>().enabled = false;
            }
        }
	}

    //Trigger Enter
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (typ)
            {
                case EnemyProjectiles.type.SkullMissile:
                    other.GetComponent<Player>().ReceiveDamage(dmg);
                    break;
            }
        }
    }


    //Trigger Stay
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (typ)
            {
                //acid spit > acid pool > poison
                case EnemyProjectiles.type.AcidSpit:
                    //call player's poison function
                    other.GetComponent<Player>().ApplyPoison(5f);
                    break;
                //web shot > sticky area > slow
                case EnemyProjectiles.type.WebShot:
                    //call player's slow function
                    other.GetComponent<Player>().ApplySlow(5f);
                    break;
            }
        }
    }
}
