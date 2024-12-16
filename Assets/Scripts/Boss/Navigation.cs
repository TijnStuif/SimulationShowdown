using System;
using System.Collections;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    private Boss.Controller bossController;
    [SerializeField] private Transform[] wayPoints;
    private GameObject currentWayPoint;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private GameObject wayPointsPrefab;
    private Player.V2.Controller playerController;
    public event Action OnBossReachedWayPoint;

    private void Awake()
    {
        bossController = GetComponent<Boss.Controller>();
        playerController = FindObjectOfType<Player.V2.Controller>();
        Instantiate(wayPointsPrefab, transform.position, Quaternion.identity);
        wayPoints = wayPointsPrefab.GetComponentsInChildren<Transform>();
        OnBossReachedWayPoint += BossReachedWayPoint;
        StartCoroutine(LoopWayPointMovement());
    }

    private void Update()
    {
        if (currentWayPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint.transform.position, movementSpeed * Time.deltaTime);
            if (bossController.transform.position == currentWayPoint.transform.position)
            {
                OnBossReachedWayPoint?.Invoke();
            }
        }
        if (currentWayPoint == null)
        {
            LetBossFace(playerController.gameObject);
        }
    }

    private IEnumerator LoopWayPointMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            GameObject targetedWayPoint = currentWayPoint;
            currentWayPoint = wayPoints[UnityEngine.Random.Range(0, wayPoints.Length - 1)].gameObject;
            LetBossFace(currentWayPoint);
            
            while (currentWayPoint == targetedWayPoint)
            {
                currentWayPoint = wayPoints[UnityEngine.Random.Range(0, wayPoints.Length)].gameObject;
            }
        }
    }

    private void LetBossFace(GameObject gameObject)
    {
        transform.LookAt(gameObject.transform.position);
        transform.Rotate(0, 180, 0);
    }

    private void BossReachedWayPoint()
    {
        currentWayPoint = null;
    }
}