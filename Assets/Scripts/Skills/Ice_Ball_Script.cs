using UnityEngine;
using System.Collections;

public class Ice_Ball_Script : Spell
{

    // Use this for initialization
    public GameObject Ice_blast;
	public GameObject iceSpike;
    public float Speed = 0.0f;
    public float Lifespan = 1.0f;
    private Rigidbody rigidBody;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        SFXManager.instance.playSFX(sounds.iceBall);
    }

    // Update is called once per frame
    void Update()
    {
		transform.Translate(Vector3.forward * Speed * Time.deltaTime * 50f);

        Lifespan -= Time.deltaTime;
        if (Lifespan <= 0)
        {
            SFXManager.instance.playSFX(sounds.iceBlast);
            Instantiate(Ice_blast, new Vector3(this.transform.position.x,0,transform.position.z), transform.rotation);
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
			Explode ();
        }

        if (other.GetComponent<Enemy>() && other.GetType() == typeof(CapsuleCollider))
        {

			GetDamage ();
			other.GetComponent<Enemy> ().ReceiveDamage (damage, player);

			int chance = 5 * player.skills.iceBoltSpikeLevel;
			int noOfSpikes = 0;
			while (chance > 100) {
				chance -= 100;
				noOfSpikes++;
			}
			int rand = Random.Range (0, 100);
			rand++;
			if (rand > (100 - chance))
				noOfSpikes++;

			for (int i = 0; i < noOfSpikes; i++) {
				StartCoroutine (_IceSpike (0f + 0.3f * i, other.GetComponent<Enemy>().transform.position));
			}

			Explode ();
		}
    }

	void Explode() {
        SFXManager.instance.playSFX(sounds.iceBlast);
		Instantiate(Ice_blast, new Vector3(this.transform.position.x, 0, transform.position.z), transform.rotation);
		Destroy(gameObject);
	}

	IEnumerator _IceSpike(float delayTime, Vector3 pos) {

		yield return new WaitForSeconds (delayTime);

		GameObject spike = (GameObject)Resources.Load ("Skills/IceSpike");

		spike = (GameObject)Instantiate (spike, pos, transform.rotation);
		spike.GetComponent<Spell> ().player = player;

	}
}