using UnityEngine;
using System.Collections;

public class Testing : MonoBehaviour {

    // Use this for initialization
    public GameObject Ice_spike;
    public GameObject Ice_ball;
    public GameObject Ice_blast;
    public GameObject Transmutation_ice;
    public GameObject Transmutation_fire;
    public GameObject fire_pillar;
    public GameObject PowerSpawn;
    public GameObject enemy;
    public GameObject DivineSpin;
    private int currentSkill = 1;
    public float Speed;
    private Rigidbody rigidBody;
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
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
        if (Input.GetKeyDown("4"))
        {
            currentSkill = 4;
        }

        if (Input.GetKey("o"))
        {
            transform.Rotate(new Vector3(0, -1, 0));

        }
        if (Input.GetKey("p"))
        {
            transform.Rotate(new Vector3(0, 1, 0));

        }
        if (Input.GetKeyDown("space")) {
            if (currentSkill == 1)
            {
                StartCoroutine(IceSpike());
                StopCoroutine(IceSpike());
                //Instantiate(Transmutation_ice, PowerSpawn.transform.position, transform.localRotation);
                /*Instantiate(Ice_ball, this.transform.position, transform.rotation);
                StartCoroutine(Delay());
                StopCoroutine(Delay());
                Instantiate(Ice_ball, this.transform.position + new Vector3(0,0,5), transform.rotation);
                StartCoroutine(Delay());
                StopCoroutine(Delay());
                Instantiate(Ice_ball, this.transform.position + new Vector3(0,0,10), transform.rotation);
                StartCoroutine(Delay());
                StopCoroutine(Delay());
                Instantiate(Ice_ball, this.transform.position + new Vector3(0,0,15), transform.rotation);*/
                //Ice_blast.transform.position = this.transform.position;
            }
            else if (currentSkill == 2)
            {
                Instantiate(Transmutation_fire, enemy.transform.position, transform.localRotation);
                Instantiate(fire_pillar, enemy.transform.position, transform.rotation);
                //Ice_blast.transform.position = this.transform.position;
            }
            else if (Input.GetKeyDown("space"))
            {
                if (currentSkill == 3)
                {
                    Instantiate(Transmutation_ice, PowerSpawn.transform.position, transform.localRotation);
                    Instantiate(Ice_ball, this.transform.position, transform.rotation);
                    Ice_blast.transform.position = this.transform.position;
                }

            }
            if (Input.GetKey("space"))
            {
                if (currentSkill == 4)
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
    IEnumerator IceSpike()
    {
        Instantiate(Ice_spike, this.transform.position + transform.forward * 2 + new Vector3(0,-2,0), transform.rotation * Quaternion.Euler(0, 90, 0));
        yield return new WaitForSeconds(0.1f);
        Instantiate(Ice_spike, this.transform.position + transform.forward * 4 + new Vector3(0, -2, 0), transform.rotation * Quaternion.Euler(0, 300, 0));
        yield return new WaitForSeconds(0.1f);
        Instantiate(Ice_spike, this.transform.position + transform.forward * 6 + new Vector3(0, -2, 0), transform.rotation * Quaternion.Euler(0, 126, 0));
        yield return new WaitForSeconds(0.1f);
        Instantiate(Ice_spike, this.transform.position + transform.forward * 8 + new Vector3(0, -2, 0), transform.rotation * Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(0.1f);
        Instantiate(Ice_spike, this.transform.position + transform.forward * 10 + new Vector3(0, -2, 0), transform.rotation * Quaternion.Euler(0, 40, 0));
        yield return new WaitForSeconds(0.1f);
        Instantiate(Ice_spike, this.transform.position + transform.forward * 12 + new Vector3(0, -2, 0), transform.rotation * Quaternion.Euler(0, 185, 0));

    }
}