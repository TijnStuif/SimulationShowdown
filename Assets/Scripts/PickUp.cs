using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickUp : MonoBehaviour
{
    public static event Action<float> PickUpCollected;
    private int xPosition;
    private int zPosition;
    private float offset = -12.5f;
    private float tileWidth = 5f;
    private float amountOfPickUpsCollected = 0;
    private AudioManager audioManager;
    public float AmountOfPickupsCollected 
    {
        get { return amountOfPickUpsCollected; }
        set { amountOfPickUpsCollected = value; PickUpCollected?.Invoke(amountOfPickUpsCollected); }

    } 
    
    void Awake()
    {
        SpawnPickUps();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void SpawnPickUps()
    {
        xPosition = Random.Range(0, 6);
        zPosition = Random.Range(0, 6);
        transform.position = new Vector3(offset + xPosition * tileWidth, 1.5f, offset + zPosition * tileWidth);
    }

    private void OnTriggerEnter(Collider player)
    {
        audioManager.PlaySFX(audioManager.pickupSFX);
        AmountOfPickupsCollected++;
        SpawnPickUps();
    }
}
