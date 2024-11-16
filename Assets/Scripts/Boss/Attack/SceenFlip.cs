using UnityEngine;

namespace Boss.Attack
{
    public class SceneFlip : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;

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
}