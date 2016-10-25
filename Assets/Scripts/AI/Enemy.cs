using UnityEngine;
using System.Collections;
using Pathfinding;

public class Enemy : MonoBehaviour {

	public float health;
	public GameObject damageText;

	public float damage;

    //target position
    protected Vector3 target;
    //player reference
    protected GameObject player;
    //seeker component
    protected Seeker seeker;
    //calculated path
    protected Path path;
    //AI speed
    public float speed;
    //distance AI is to a waypoint for it to continue to the next
    protected float nextWayPointDistance = 1f;
    //current waypoint
    protected int currentWayPoint = 0;

    //States
    public enum States
    {
        Idle,
        Chase,
        Attack,
        Dead
    }
    //myState (current state this entity is in)
    public States myState = States.Idle;

	//Status Effects
	bool isBurning = false;
	bool isFrozen = false;
	bool isSlowed = false;
	bool isStunned = false;
	bool isConfused = false;

	// Use this for initialization
	void Start () {
		damageText = (GameObject)Resources.Load ("DamageText");
	}
	
	// Update is called once per frame
	void Update () {
        
	}


    //Idle state
    protected virtual void Idle()
    {
        Debug.Log("ENEMY SCRIPT Idle");
    }
    //chase
    protected virtual void Chase()
    {
        Debug.Log("ENEMY SCRIPT Chase");
    }

    protected virtual void Attack()
    {
        Debug.Log("ENEMY SCRIPT Attack");
    }

	public void ReceiveDamage(float dmg) {

		health -= dmg;
		GameObject txt = (GameObject)Instantiate(damageText, transform.position, Quaternion.identity);
		txt.GetComponent<TextMesh>().text = dmg.ToString("F0");
		txt.transform.Rotate(55, 0, 0);
		Debug.Log (GetComponent<Player> ().Health);
	}

	void OnTriggerEnter(Collider other) {


	}

    protected void OnPathComplete(Path p)
    {
    
        if (!p.error)
        {
            path = p;
            //Reset the waypoint counter
            currentWayPoint = 0;

        }
        else
        {
            Debug.Log(" Error: " + p.error);
        }
    }
}
