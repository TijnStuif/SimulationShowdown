using Player.V1;
using UnityEngine;

namespace Boss.Attack
{
    public class InvertedControls : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;

        private Movement playerMovement;
        private GameObject player;
        [SerializeField] private GameObject inputReverserAttackIndicatorPrefab;
        private GameObject inputReverserAttackIndicator;
        private Vector3 startPosition = new Vector3(200, 0, -20);

        void Start()
        {
            inputReverserAttackIndicator = Instantiate(inputReverserAttackIndicatorPrefab);
            player = FindObjectOfType<Player.V1.Controller>().gameObject;
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