using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedControlsAttack : MonoBehaviour, IAttack
{
    public AttackType Type => AttackType.Environment;

    private PlayerMovement playerMovement;

    void Start()
    {
        // Find the PlayerMovement component on the player GameObject
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void Execute()
    {
        if (playerMovement != null)
        {
            // Toggle the inverted controls flag
            playerMovement.areControlsInverted = !playerMovement.areControlsInverted;
            Debug.Log($"Inverted controls: {playerMovement.areControlsInverted}");
        }
        else
        {
            Debug.LogWarning("PlayerMovement component not found.");
        }
    }
}