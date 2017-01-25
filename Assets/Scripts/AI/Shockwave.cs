using UnityEngine;
using System.Collections;

public class Shockwave : MonoBehaviour {

    public float shockwaveDmg = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (GetComponent<ParticleSystem>().isStopped)
	    {
	        Destroy(gameObject);
	    }
	}

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().ReceiveDamage(shockwaveDmg);
        }
    }
}
