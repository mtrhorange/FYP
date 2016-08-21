using UnityEngine;
using System.Collections;

public class Testing : MonoBehaviour {

    // Use this for initialization
    public GameObject Ice_ball;
    public GameObject Transmutation_ice;
    public GameObject Transmutation_fire;
    public GameObject fire_pillar;
    public GameObject PowerSpawn;
    public GameObject enemy;
    public GameObject DivineSpin;
    private int currentSkill = 1;
    public float Speed;
    private Rigidbody rigidBody;
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(Horizontal, 0, Vertical) * 0.1f);

        if (Input.GetKeyDown("1"))
        {
            currentSkill = 1;
        }
        if (Input.GetKeyDown("2"))
        {
            currentSkill = 2;
        }
        if (Input.GetKeyDown("3"))
        {
            currentSkill = 3;
        }

        if (Input.GetKey("o"))
        {
            transform.Rotate(new Vector3(0, -1, 0));

        }
        if (Input.GetKey("p"))
        {
            transform.Rotate(new Vector3(0, 1, 0));

        }

        if (Input.GetKeyDown("space")){
            if (currentSkill == 1)
            {
                Instantiate(Transmutation_ice, PowerSpawn.transform.position, transform.localRotation);
                Instantiate(Ice_ball, this.transform.position, transform.rotation);
                //Ice_blast.transform.position = this.transform.position;
            }
            else if (currentSkill == 2)
                {
                    Instantiate(Transmutation_fire, enemy.transform.position, transform.localRotation);
                    Instantiate(fire_pillar, enemy.transform.position, transform.rotation);
                    //Ice_blast.transform.position = this.transform.position;
                }

        }
        if (Input.GetKey("space"))
        {
            if (currentSkill == 3)
            {
                transform.Rotate(new Vector3(0, 10, 0)); transform.Rotate(new Vector3(0, 20, 0));
                DivineSpin.GetComponent<TrailRenderer>().enabled = true;
            }

        }
        else
        {
            DivineSpin.GetComponent<TrailRenderer>().enabled = false;
        }
    }
}
