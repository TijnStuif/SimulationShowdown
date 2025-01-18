using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PickUp : MonoBehaviour
{
    private int xPosition;
    private int zPosition;
    private float offset = -12.5f;
    private float tileWidth = 5f;
    public float pickUpsCollected = 0;
    
    void Awake()
    {
        SpawnPickUps();
    }

    private void SpawnPickUps()
    {
        xPosition = Random.Range(0, 6);
        zPosition = Random.Range(0, 6);
        transform.position = new Vector3(offset + xPosition * tileWidth, 1.5f, offset + zPosition * tileWidth);
    }

    private void OnTriggerEnter(Collider player)
    {
        pickUpsCollected++;
        SpawnPickUps();
        Debug.Log(pickUpsCollected);
    }
}
