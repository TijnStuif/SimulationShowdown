using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    private bool isPickedUp = false;
    private bool isPlayerNearby = false;
    private bool isPlaced = false;
    private GameObject player;

    void Start()
    {
        
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.name);
        if (other.CompareTag("Player") && !isPickedUp && !isPlaced)
        {
            isPlayerNearby = true;
            player = other.gameObject;
            Debug.Log("Player can pick up the trap");
        }
        if ((other.CompareTag("Player") || other.CompareTag("Boss")) && isPlaced)
        {
            Debug.Log("Trap triggered by " + other.name);
            TriggerCustomEvent(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isPickedUp)
        {
            isPlayerNearby = false;
            player = null;
        }
    }

    public void PickUp()
    {
        if (isPlaced)
        {
            Debug.Log("Cannot pick up a placed trap.");
            return;
        }

        isPickedUp = true;
        gameObject.SetActive(false);
        if (player != null)
        {
            PlayerInventoryScript inventory = player.GetComponent<PlayerInventoryScript>();
            if (inventory != null)
            {
                inventory.AddTrapToInventory(this);
            }
        }
    }

    public void Place(Vector3 position)
    {
        isPickedUp = false;
        transform.position = position;
        gameObject.SetActive(true);
        isPlaced = true;
    }

    void TriggerCustomEvent(Collider other)
    {
        Debug.Log("Trap triggered");
        if (other.CompareTag("Player"))
        {
            PlayerFocusScript playerFocus = other.GetComponent<PlayerFocusScript>();
            if (playerFocus != null)
            {
                playerFocus.focus -= 20;
            }
        }
        else if (other.CompareTag("Boss"))
        {
            BossFocusScript bossFocus = other.GetComponent<BossFocusScript>();
            if (bossFocus != null)
            {
                bossFocus.FocusBar -= 10;
            }
        }

        gameObject.SetActive(false);   
    }
}