using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossWanderScript : MonoBehaviour
{
    public NavMeshAgent agent; 
    public Transform player; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 newPos = FindHidingPosition();
        agent.SetDestination(newPos);
    }

    Vector3 FindHidingPosition()
    {
        Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
        Vector3 potentialPosition = transform.position + directionAwayFromPlayer * 10f; 

        NavMeshHit navHit;
        NavMesh.SamplePosition(potentialPosition, out navHit, 10f, -1);

        if (IsPositionBehindWall(navHit.position))
        {
            return AdjustPositionBeforeWall(navHit.position, directionAwayFromPlayer);
        }
        else
        {
            return RandomNavSphere(transform.position, 10f, -1);
        }
    }

    bool IsPositionBehindWall(Vector3 position)
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.position - position).normalized;

        if (Physics.Raycast(position, directionToPlayer, out hit))
        {
            if (hit.collider.gameObject != player.gameObject)
            {
                return true; 
            }
        }
        return false;
    }

    Vector3 AdjustPositionBeforeWall(Vector3 position, Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit, 10f))
        {
            return hit.point - direction * 1f; // Stop 1 unit before the wall
        }
        return position;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}