using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    private GameObject[] platformPrefabs;
    [SerializeField] private GameObject player;

    private void Start()
    {
        platformPrefabs = GameObject.FindGameObjectsWithTag("Floor");
        Debug.Log(platformPrefabs.Length);
    }
}

