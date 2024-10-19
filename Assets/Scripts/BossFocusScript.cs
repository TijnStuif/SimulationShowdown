using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossFocusScript : MonoBehaviour
{
    public int FocusBar = 100;
    public NavMeshAgent agent;
    public Transform player;
    public float walkSpeed = 30f;
    public float sneakSpeed = 2f;
    public float sneakDistanceThreshold = 10f;
    public float focusLossRate = 3f; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        DecideWalkOrSneak();
    }

    void DecideWalkOrSneak()
    {
        float distance = CalculatePathDistance(player.position);

        if (distance > sneakDistanceThreshold)
        {
            Sneak();
        }
        else
        {
            Walk();
        }
    }

    float CalculatePathDistance(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);

        float distance = 0f;
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            for (int i = 1; i < path.corners.Length; i++)
            {
                distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }

        return distance;
    }

    void Walk()
    {
        agent.speed = walkSpeed;
        ReduceFocus();
    }

    void Sneak()
    {
        agent.speed = sneakSpeed;
    }

    void ReduceFocus()
    {
        float focusLoss = focusLossRate * Time.deltaTime;
        FocusBar -= Mathf.RoundToInt(focusLoss);
        FocusBar = Mathf.Clamp(FocusBar, 0, 100);
    }
}