using System;
using System.Collections;
using Player.V2;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    private Boss.Controller bossController;
    private bool frozen;
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

    private void OnEnable()
    {
        Player.V2.Teleport.MashSequenceStateChange += OnMashSequenceStateChange;
    }

    private void OnDisable()
    {
        Player.V2.Teleport.MashSequenceStateChange -= OnMashSequenceStateChange;
    }

    private void Update()
    {
        if (currentWayPoint != null && frozen == false)
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

    private void OnMashSequenceStateChange(Teleport.MashState state)
    {
        switch (state)
        {
            case Teleport.MashState.Start:
                frozen = true;
                break;
            case Teleport.MashState.End:
                frozen = false;
                break;
        }
    }
    private void BossReachedWayPoint()
    {
        currentWayPoint = null;
    }
}