using UnityEngine;
using System.Collections;

public class LevelUp : MonoBehaviour {

    //player gameobject
    public GameObject p;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (!GetComponentInChildren<ParticleSystem>().IsAlive(true))
        {
            Destroy(this.gameObject);
        }
        else
        {
            Vector3 moveHere = p.transform.position;
            moveHere.y = this.transform.position.y;
            transform.position = moveHere;
        }
	}
}
