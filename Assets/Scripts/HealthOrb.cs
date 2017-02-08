using UnityEngine;
using System.Collections;


public class HealthOrb : MonoBehaviour {

    private float lifeTime = 10f;

	//Start
	void Start()
    {

	}
	
	//Update
	void Update()
    {
        if (lifeTime < 0f)
        {
            //once lifetime is over, destroy
            Destroy(this.gameObject);
        }
        else
        {
            lifeTime -= Time.deltaTime;
        }
	}

    //trigger enter
    void OnTriggerEnter(Collider other)
    {
        //if other is player, give him health
        if (other.GetComponent<Player>())
        {
            //Do not pick up if already at full health
            if (other.GetComponent<Player>().Health < other.GetComponent<Player>().MaxHealth)
            {
                //heal between 3% ~ 7% of player's max health
                float healAmount = Mathf.Round(Random.Range(other.GetComponent<Player>().MaxHealth * 0.03f, other.GetComponent<Player>().MaxHealth * 0.07f));
                other.GetComponent<Player>().ReceiveHeal(healAmount);
                Destroy(this.gameObject);
            }
        }
    }
}
