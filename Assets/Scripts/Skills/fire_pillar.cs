using UnityEngine;
using System.Collections;

public class fire_pillar : MonoBehaviour {

    // Use this for initialization
    public float lifeSpan = 5;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }
}
