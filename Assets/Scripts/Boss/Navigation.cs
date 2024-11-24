using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    private Boss.Controller bossController;
    [SerializeField] private Transform[] wayPoints;
    private GameObject currentWayPoint;
    [SerializeField] private float movementSpeed = 5f;

    private void Awake()
    {
        bossController = GetComponent<Boss.Controller>();
        StartCoroutine(LoopWayPointMovement());
    }

    private void Update()
    {
        if (currentWayPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint.transform.position, movementSpeed * Time.deltaTime);
        }
    }

    private IEnumerator LoopWayPointMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            GameObject targetedWayPoint = currentWayPoint;
            currentWayPoint = wayPoints[Random.Range(0, wayPoints.Length)].gameObject;
            while (currentWayPoint == targetedWayPoint)
            {
                currentWayPoint = wayPoints[Random.Range(0, wayPoints.Length)].gameObject;
            }
        }
    }
}