using System;
using Player.V2;
using UnityEngine;

namespace Boss.Attack
{
    public class InvertedControls : MonoBehaviour, IAttack
    {
        public Type Type => Type.Environment;

        private Movement playerMovement;
        private Player.V1.Movement m_compatMovementV1;
        private GameObject player;
        [SerializeField] private GameObject inputReverserAttackIndicatorPrefab;
        private GameObject inputReverserAttackIndicator;
        private Vector3 startPosition = new Vector3(200, 0, -20);

        void Start()
        {
            #if DEBUG
            if (Compatibility.IsV1)
                m_compatMovementV1 = FindObjectOfType<Player.V1.Movement>();
            #endif
            
            inputReverserAttackIndicator = Instantiate(inputReverserAttackIndicatorPrefab);
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                throw new NullReferenceException($"ERROR: {nameof(player)} not found!");
            playerMovement = player.GetComponent<Movement>();
            if (playerMovement == null && m_compatMovementV1 == null)
                throw new StateController.ScriptNotFoundException(nameof(playerMovement));
            inputReverserAttackIndicator.transform.position = startPosition;
        }

        public void Execute()
        {
            inputReverserAttackIndicator.transform.position = player.transform.position;
            Invoke(nameof(RemoveIndicator), 0.2f);
            #if DEBUG
            if (Compatibility.IsV1)
            {
                m_compatMovementV1.areControlsInverted = !m_compatMovementV1.areControlsInverted;
                return;
            }
            #endif

            playerMovement.AreControlsInverted = !playerMovement.AreControlsInverted;
        }

        private void RemoveIndicator()
        {
            inputReverserAttackIndicator.transform.position = startPosition;
        }
    }
}