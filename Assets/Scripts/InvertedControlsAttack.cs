using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedControlsAttack : MonoBehaviour, IAttack
{
    public AttackType Type => AttackType.Environment;

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void Execute()
    {
        if (playerMovement != null)
        {
            playerMovement.areControlsInverted = !playerMovement.areControlsInverted;
        }
        else
        {
            Debug.LogWarning("PlayerMovement component not found.");
        }
    }
}