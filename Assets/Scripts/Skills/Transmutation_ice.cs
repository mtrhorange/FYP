using UnityEngine;
using System.Collections;

public class Transmutation_ice : MonoBehaviour {

    // Use this for initialization
    public float Lifespan = 1.5f;
	void Start () {
	    
	}

    // Update is called once per frame
    void Update() {
        Lifespan -= Time.deltaTime;
        if (Lifespan <= 0) {
            Destroy(this.gameObject);
        }
        
	}
}
