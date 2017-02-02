using UnityEngine;
using System.Collections;
using Pathfinding;

public class GuideTrail : MonoBehaviour {

    //positons
    public Vector3 start, end;
    //seeker component
    public Vector3 nextPathPoint;
    protected Seeker seeker;
    //calculated path
    protected Path path;
    //Fly Speed
    private float speed = 16f;
    //distance AI is to a waypoint for it to continue to the next
    public float nextWayPointDistance = 3f;
    //current waypoint
    public int currentWayPoint = 3;


	// Use this for initialization
	void Start ()
    {
        seeker = GetComponent<Seeker>();

        seeker.StartPath(start, end, OnPathComplete);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (path != null && currentWayPoint < path.vectorPath.Count)
            nextPathPoint = path.vectorPath[currentWayPoint + 1 >= path.vectorPath.Count ? currentWayPoint : currentWayPoint + 1];

        Vector3 look = nextPathPoint - transform.position;

        look.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 14f);

        transform.position += transform.forward * Time.deltaTime * speed;


        if (currentWayPoint >= path.vectorPath.Count)
        {
            Destroy(this.gameObject);
            return;
        }

        //update the waypoint on the path once the current one has been reached
        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < nextWayPointDistance)
        {
            currentWayPoint++;
            return;
        }

	}

    //path calculation complete callback
    protected void OnPathComplete(Path p)
    {

        if (!p.error)
        {
            path = p;
            //Reset the waypoint counter
            currentWayPoint = 3;
        }
        else
        {
            Debug.Log(" Error: " + p.error);
        }
    }
}
