using Player;
using UnityEngine;

namespace Boss.Attack
{
    public class InvertedControls : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;

        private Movement playerMovement;
        private GameObject player;
        private GameObject inputReverserAttackIndicator;
        private Vector3 startPosition = new Vector3(200, 0, -20);

        void Start()
        {
            inputReverserAttackIndicator = GameObject.Find("InputReverserIndicator");
            player = FindObjectOfType<Player.Controller>().gameObject;
            playerMovement = FindObjectOfType<Movement>();
            inputReverserAttackIndicator.transform.position = startPosition;
        }

        public void Execute()
        {
            if (playerMovement != null || player != null)
            {
                inputReverserAttackIndicator.transform.position = player.transform.position;
                Invoke(nameof(RemoveIndicator), 0.2f);
                playerMovement.areControlsInverted = !playerMovement.areControlsInverted;
            }
            else
            {
            }
        }

        private void RemoveIndicator()
        {
            inputReverserAttackIndicator.transform.position = startPosition;
        }
    }
}