using UnityEngine;
using System.Collections;

public class EnemySummon : MonoBehaviour {

    public mobType typeToSpawn;
    private bool spawned = false;

	//Start
	void Start ()
    {
	
	}
	
	//Update
	void Update ()
    {
        //spawn at about 75% through the effect, destroy this once particles done
        if (GetComponent<ParticleSystem>())
        {
            if (!spawned && GetComponent<ParticleSystem>().time / GetComponent<ParticleSystem>().duration >= 0.75f)
            {
                spawned = true;
                AIManager.instance.spawnMob(typeToSpawn, transform.position);
            }
            if (!GetComponent<ParticleSystem>().IsAlive(true))
            {
                Destroy(this.gameObject);
            }
        }
        else if (GetComponentInChildren<ParticleSystem>())
        {
            if (!spawned && GetComponentInChildren<ParticleSystem>().time / GetComponentInChildren<ParticleSystem>().duration >= 0.75f)
            {
                spawned = true;
                AIManager.instance.spawnMob(typeToSpawn, transform.position);
            }
            if (!GetComponentInChildren<ParticleSystem>().IsAlive(false))
            {
                Destroy(this.gameObject);
            }
        }
	}
}
