using Player;
using UnityEngine;

namespace Boss.Attack
{
    public class InvertedControls : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;

        private Movement playerMovement;

        void Start()
        {
            playerMovement = FindObjectOfType<Movement>();
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
}