using System;
using UnityEngine;

namespace Boss.Attack
{
    public class SceneFlip : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;

        private Transform playerCamera;

        private bool isFlipped = false;

        private void Awake()
        {
            // find PlayerFollower (this object is under Player)
            playerCamera = FindObjectOfType<Player.Rotation>().gameObject.transform;
        }

        public void Execute()
        {
            if (playerCamera != null)
            {
                // flip the camera
                if (isFlipped)
                {
                    playerCamera.localRotation = Quaternion.Euler(0, 0, 0);
                    playerCamera.position = new Vector3(playerCamera.position.x, playerCamera.position.y - 2, playerCamera.position.z);
                }
                else
                {
                    playerCamera.localRotation =  Quaternion.Euler(0, 0, 180);
                    playerCamera.position = new Vector3(playerCamera.position.x, playerCamera.position.y + 2, playerCamera.position.z);
                }

                isFlipped = !isFlipped;

                Debug.Log($"Screen flipped. Camera rotation: {playerCamera.localRotation.eulerAngles}");
            }
            else
            {
                Debug.LogWarning("PlayerCamera not assigned. Make sure to assign it in the Inspector.");
            }
        }
    }
}