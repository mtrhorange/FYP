using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlockingManager : MonoBehaviour
{
    private ArrayList agentArray = new ArrayList();
    [Range(0f, 1f)] public float alignmentWeight = 0.2f;
    [Range(0f, 1f)]public float cohesionWeight = 0.2f;
    [Range(0f, 1f)]public float separationWeight = 0.2f;
    [Range(0f, 10f)]public float range = 3f;
    public float boundary = 5000f;
    private float velocityResetTime =0.5f;
    // Use this for initialization
    void Start()
    {

        agentArray.AddRange(FindObjectsOfType<Enemy>());

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Enemy agent in agentArray)
        {
            Vector3 alignment = computeAlignment(agent);
            Vector3 cohesion = computeCohesion(agent);
            Vector3 separation = computeSeparation(agent);
            Vector3 wallAvoidance = computeWallAvoidance(agent);

            velocityResetTime -= Time.deltaTime;

            if(velocityResetTime <= 0)
            {
                agent.velocity = Vector3.zero;
                velocityResetTime = 0.1f;
            }
            
            
            agent.velocity.x += alignment.x * alignmentWeight + cohesion.x * cohesionWeight + separation.x * separationWeight;
            agent.velocity.y += alignment.y * alignmentWeight + cohesion.y * cohesionWeight + separation.y * separationWeight;
            agent.velocity.z += alignment.z * alignmentWeight + cohesion.z * cohesionWeight + separation.z * separationWeight;
            //agent.velocity.x += separation.x * separationWeight;
            //agent.velocity.y += separation.y * separationWeight;
            //agent.velocity.z += separation.z * separationWeight;
            agent.velocity.x += wallAvoidance.x * 0.1f;
            agent.velocity.z += wallAvoidance.z * 0.1f;

            Vector3 direction = agent.nextPathPoint -  agent.transform.position ;
            direction.y = 0;
            agent.velocity += 0.1f * direction.normalized;

        }
    }

    public Vector3 computeAlignment(Enemy myAgent)
    {
        Vector3 v = new Vector3();
        int neighborCount = 0;
        foreach (Enemy agent in agentArray)
        {
            if (agent != myAgent && agent.myType == myAgent.myType)
            {
                if (Vector3.Distance(
                    myAgent.transform.position,
                    agent.transform.position) < range)
                {
                    v.x += agent.velocity.x;
                    v.y += agent.velocity.y;
                    v.z += agent.velocity.z;
                    neighborCount++;
                }

            }

        }

        if (neighborCount == 0)
            return v;
        v.x /= neighborCount;
        v.y /= neighborCount;
        v.z /= neighborCount;
        v.Normalize();
        return v;
    }

    public Vector3 computeCohesion(Enemy myAgent)
    {
        Vector3 v = new Vector3();
        int neighborCount = 0;
        foreach (Enemy agent in agentArray)
        {
            if (agent != myAgent && agent.myType == myAgent.myType)
            {
                if (Vector3.Distance(
                myAgent.transform.position,
                agent.transform.position) > range)
                {
                    v.x += agent.transform.position.x;
                    v.y += agent.transform.position.y;
                    v.z += agent.transform.position.z;
                    neighborCount++;
                }
            }

        }
        if (neighborCount == 0)
            return v;
        v.x /= neighborCount;
        v.y /= neighborCount;
        v.z /= neighborCount;
        v = new Vector3(
            v.x - myAgent.transform.position.x,
            v.y - myAgent.transform.position.y,
            v.z - myAgent.transform.position.z);
        v.Normalize();
        return v;
    }

    public Vector3 computeSeparation(Enemy myAgent)
    {
        Vector3 v = new Vector3();
        int neighborCount = 0;
        foreach (Enemy agent in agentArray)
        {
            if (agent != myAgent && agent.myType == myAgent.myType)
            {
                if (Vector3.Distance(
                myAgent.transform.position,
                agent.transform.position) < range)
                {
                    v.x += agent.transform.position.x - myAgent.transform.position.x;
                    v.y += agent.transform.position.y - myAgent.transform.position.y;
                    v.z += agent.transform.position.z - myAgent.transform.position.z;
                    neighborCount++;
                }
            }

        }
        if (neighborCount == 0)
            return v;
        v.x /= neighborCount;
        v.y /= neighborCount;
        v.z /= neighborCount;
        v.x *= -1;
        v.y *= -1;
        v.z *= -1;
        v.Normalize();
        return v;
    }

    public Vector3 computeWallAvoidance(Enemy agent)
    {
        Vector3 v = new Vector3();
        if (agent.transform.position.x < -boundary)
        {
            v.x = 1;
        }
        if (agent.transform.position.x > boundary)
        {
            v.x = -1;
        }
        if (agent.transform.position.z < -boundary)
        {
            v.z = 1;
        }
        if (agent.transform.position.z > boundary)
        {
            v.z = -1;
        }
        return v;
    }
    public void printMe() { 
        
    }
}