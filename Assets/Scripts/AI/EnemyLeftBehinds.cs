using UnityEngine;
using System.Collections;

public class EnemyLeftBehinds : MonoBehaviour {
    //damage
    public float dmg = 0;
    //if true, hits once only, if false, hits constantly as long as requirements are met
    public bool hitOnce = true;
    public float hitTimer = 0;

	//Start
	void Start()
    {
	}
	
	//Update
	void Update()
    {	
	}

    //Trigger Stay
    void OnTriggerStay(Collider other)
    {
        //if not hitOnce and its a player
        if (other.gameObject.tag == "Player")
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0)
            {
                other.gameObject.GetComponent<Player>().ReceiveDamage(dmg * Time.deltaTime);
                hitTimer = 2.0f;
            }
            
            
        }
    }
}
