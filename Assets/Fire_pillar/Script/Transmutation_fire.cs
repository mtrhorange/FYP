using UnityEngine;
using System.Collections;

public class Transmutation_fire : MonoBehaviour {

    // Use this for initialization
    public float lifeSpan = 10;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
	}
}
