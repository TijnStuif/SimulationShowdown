using Player.V2;
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
        private static AudioManager AudioManager => AudioManager.Instance;

        void Start()
        {
            // inputReverserAttackIndicator = Instantiate(inputReverserAttackIndicatorPrefab);
            inputReverserAttackIndicator = GameObject.Find("InputReverserIndicator");
            playerMovement = FindObjectOfType<Movement>();
            player = playerMovement.gameObject;
            inputReverserAttackIndicator.transform.position = startPosition;
        }

        public void Execute()
        {
            if (playerMovement != null || player != null)
            {
                inputReverserAttackIndicator.transform.position = player.transform.position;
                Invoke(nameof(RemoveIndicator), 0.2f);
                playerMovement.AreControlsInverted = !playerMovement.AreControlsInverted;
                AudioManager.PlaySFX(AudioManager.bossInputReverserSFX);
            }
        }

        private void RemoveIndicator()
        {
            inputReverserAttackIndicator.transform.position = startPosition;
        }
    }
}