using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBehaviour : MonoBehaviour
{
    public float bossHealth = 5;
    public float bossHealthMax = 5;
    public Image bossHealthBarImage;
    [Header("Platforms")]
    private List<GameObject> platforms;
    private List<GameObject> platformsDisabled = new List<GameObject>();
    private GameObject targetedPlatform;
    public Material warningMaterial;
    [SerializeField] private PlayerBehaviour player;
    private readonly float timeBetweenAttacks = 3f;
    private float timeSinceLastAttack = 0f;
    

    private void Start()
    {
        platforms = new List<GameObject>(GameObject.FindGameObjectsWithTag("Floor"));
        platformsDisabled = new List<GameObject>();
        SelectPlatform();
    }

    private void Update()
    {
        CheckDeath();
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

    public Vector3 GetBossPosition()
    {
        return transform.position;
    }

    private void CheckDeath()
    {
        if (bossHealth <= 0)
        {
            Debug.Log("Boss defeated!");
        }
    }
}

