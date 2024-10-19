using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    public Transform inventoryParent; 
    private List<TrapScript> inventory = new List<TrapScript>();
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlaceTrap();
        }
    }

    public void AddTrapToInventory(TrapScript trap)
    {
        inventory.Add(trap);
        Debug.Log("Picked up trap. Inventory count: " + inventory.Count);
    }

    void PlaceTrap()
    {
        if (inventory.Count > 0)
        {
            TrapScript trap = inventory[0];
            inventory.RemoveAt(0);
            Vector3 placePosition = transform.position + transform.forward * 2f;
            trap.Place(placePosition);
            Debug.Log("Placed trap. Inventory count: " + inventory.Count);
        }
    }
}