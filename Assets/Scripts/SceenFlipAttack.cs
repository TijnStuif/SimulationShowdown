using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SceenFlipAttack : MonoBehaviour, IAttack
{
    public AttackType Type => AttackType.Environment;

    public Transform PlayerCamera;

    private bool isFlipped = false;

    public void Execute()
    {
        if (PlayerCamera != null)
        {
            // flip the camera
            if (isFlipped)
            {
                PlayerCamera.localRotation = Quaternion.Euler(0, 0, 0);
                PlayerCamera.position = new Vector3(PlayerCamera.position.x, PlayerCamera.position.y - 2, PlayerCamera.position.z);
            }
            else
            {
                PlayerCamera.localRotation =  Quaternion.Euler(0, 0, 180);
                PlayerCamera.position = new Vector3(PlayerCamera.position.x, PlayerCamera.position.y + 2, PlayerCamera.position.z);
            }

            isFlipped = !isFlipped;

            Debug.Log($"Screen flipped. Camera rotation: {PlayerCamera.localRotation.eulerAngles}");
        }
        else
        {
            Debug.LogWarning("PlayerCamera not assigned. Make sure to assign it in the Inspector.");
        }
    }
}