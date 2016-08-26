using UnityEngine;
using System.Collections;
using Pathfinding;

public class SimpleAI : MonoBehaviour {
    //target position
    public GameObject target;
    //seeker component
    private Seeker seeker;
    //character controller
    private CharacterController charCon;
    //calculated path
    public Path path;
    //AI speed
    public float speed = 100;
    //distance AI is to a waypoint for it to continue to the next
    public float nextWayPointDistance = 3;
    //current waypoint
    private int currentWayPoint = 0;

    //Start
	void Start ()
    {
        //get our seeker component
        seeker = GetComponent<Seeker>();
        //get character controller
        charCon = GetComponent<CharacterController>();

        //set a path to tgt position
        seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
	}

    private void OnPathComplete(Path p)
    {
        Debug.Log("Path Set, Error: " + p.error);
        if (!p.error)
        {
            path = p;
            //Reset the waypoint counter
            currentWayPoint = 0;
        }
    }

	//Update
	void Update ()
    {
        if (path == null)
        {
            //No path to move to yet
            return;
        }
        if (currentWayPoint >= path.vectorPath.Count)
        {
            Debug.Log("End Point Reached");
            return;
        }
        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
        dir *= speed * Time.deltaTime;
        charCon.SimpleMove(dir);

        //Check if we are close enough to the next waypoint
        //If yes, move to the next waypoint
        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < nextWayPointDistance)
        {
            currentWayPoint++;
            return;
        }

	}

}
