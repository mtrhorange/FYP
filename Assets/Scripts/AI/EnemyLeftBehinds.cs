using UnityEngine;
using System.Collections;

public class EnemyLeftBehinds : MonoBehaviour {
    //damage
    public float dmg = 0;
    public float hitTimer = 0;
    public EnemyProjectiles.type typ;


	//Start
	void Start()
    {
	}
	
	//Update
	void Update()
    {	
	}

    //Trigger Enter
    void OnTriggerEnter(Collider other)
    {
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
                    break;
                //web shot > sticky area > slow
                case EnemyProjectiles.type.WebShot:
                    //call player's slow function
                    break;
            }
        }
    }
}
