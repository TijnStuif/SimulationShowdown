using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    private List<GameObject> platforms;
    private List<GameObject> platformsDisabled = new List<GameObject>();
    [SerializeField] private GameObject player;
    private readonly float timeBetweenAttacks = 3f;
    private float timeSinceLastAttack = 0f;
    private GameObject targetedPlatform;
    public Material warningMaterial;

    private void Start()
    {
        platforms = new List<GameObject>(GameObject.FindGameObjectsWithTag("Floor"));
        platformsDisabled = new List<GameObject>();
        SelectPlatform();
    }

    private void Update()
    {
        if (Time.time >= timeSinceLastAttack + timeBetweenAttacks)
        {
            timeSinceLastAttack = Time.time;
            PlatformAttack();
        }
        else if (Time.time >= timeSinceLastAttack + timeBetweenAttacks - 1)
        {
            PlatformWarning();
        }
    }

    private void PlatformAttack()
    {
        if (platforms.Count <= 10)
        {
            return;
        }
        MovePlatformToDisabled();
        targetedPlatform.SetActive(false);
        SelectPlatform();
    }

    private void PlatformWarning()
    {
        if (platforms.Count <= 10)
        {
            return;
        }
        targetedPlatform.GetComponent<MeshRenderer>().material = warningMaterial;
    }

    private void SelectPlatform()
    {
        if (platforms.Count <= 10)
        {
            return;
        }
        targetedPlatform = platforms[Random.Range(0, platforms.Count)];
    }

    private void MovePlatformToDisabled()
    {
        platforms.Remove(targetedPlatform);
        platformsDisabled.Add(targetedPlatform);
    }
}

