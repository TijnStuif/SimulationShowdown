using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    private GameObject[] platformPrefabs;
    [SerializeField] private PlayerBehaviour player;
    private float timeBetweenAttacks = 3f;
    private float timeSinceLastAttack = 0f;
    private GameObject targetedPlatform;
    public Material warningMaterial;

    private void Start()
    {
        platformPrefabs = GameObject.FindGameObjectsWithTag("Floor");
        SelectPlatform();
    }

    private void Update()
    {
        if (Time.time >= timeSinceLastAttack + timeBetweenAttacks)
        {
            timeSinceLastAttack = Time.time;
            PlatformAttack();
            SelectPlatform();
        }
        else if (Time.time >= timeSinceLastAttack + timeBetweenAttacks - 1)
        {
            PlatformWarning();
        }
    }

    private void PlatformAttack()
    {
        if (platformPrefabs.Length <= 10)
        {
            return;
        }
        targetedPlatform.SetActive(false);
    }

    private void PlatformWarning()
    {
        if (platformPrefabs.Length <= 10)
        {
            return;
        }
        targetedPlatform.GetComponent<MeshRenderer>().material = warningMaterial;
    }

    private void SelectPlatform()
    {
        if (platformPrefabs.Length <= 10)
        {
            return;
        }
        targetedPlatform = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
    }

    private void ReverseInputsAttack()
    {
        player.ReverseInputs();
    }
}

