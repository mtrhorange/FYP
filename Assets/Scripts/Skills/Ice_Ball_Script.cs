using UnityEngine;
using System.Collections;

public class Ice_Ball_Script : Spell
{

    // Use this for initialization
    public GameObject Ice_blast;
    public float Speed = 0.0f;
    public float Lifespan = 1.0f;
    private Rigidbody rigidBody;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Speed );

        Lifespan -= Time.deltaTime;
        if (Lifespan <= 0)
        {
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

		if (other.GetComponent<Enemy>()) {

			GetDamage ();
			other.GetComponent<Enemy> ().ReceiveDamage (damage, player);

			Explode ();
		}
    }

	void Explode() {

		Instantiate(Ice_blast, new Vector3(this.transform.position.x, 0, transform.position.z), transform.rotation);
		Destroy(gameObject);

	}
}