using UnityEngine;
using System.Collections;

public class Ice_Blast_Script : MonoBehaviour {

    // Use this for initialization
    double TimeSpan = 2;
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        TimeSpan -= Time.deltaTime;
        if (TimeSpan <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
