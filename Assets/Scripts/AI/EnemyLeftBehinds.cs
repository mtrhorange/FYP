using UnityEngine;
using System.Collections;

public class EnemyLeftBehinds : MonoBehaviour {
    //damage
    public float dmg = 0;
    //if true, hits once only, if false, hits constantly as long as requirements are met
    public bool hitOnce = true;

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
        if (!hitOnce && other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().ReceiveDamage(dmg * Time.deltaTime);
        }
    }
}
