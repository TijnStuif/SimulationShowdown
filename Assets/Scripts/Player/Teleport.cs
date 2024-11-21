using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Teleport : MonoBehaviour
    {
        public enum BossRange
        {
           Inside,
           Outside
        }
        
        public static event Action<BossRange> RangeChange;
        
        [SerializeField] private Rigidbody player;
        [SerializeField] private Movement playerMovement;
        
        private Transform boss;
        private Camera playerCamera; 
        
        private readonly float teleportCooldown = 2f;
        private float timeSinceLastTeleport = 0;
        private readonly float aimThreshold = 30f; 
        private readonly float maxTeleportDistance = 10f;
        
        private Vector3 InputMovement3D { get; set; }


        private bool m_inAttackRange;
        
        private Vector3 DirectionToBoss => (boss.position - player.position).normalized;

        private bool InAttackRange
        {
            get => m_inAttackRange;
            set
            {
                if (InAttackRange == value)
                    throw new InvalidOperationException("ERROR: Please do not adjust this value carelessly, " +
                                                        "it invokes an event\n"+
                                                        "please only call this setter if the new value is different");
                if (Debug.isDebugBuild)
                    Debug.Log($"Attack range: {value}");
                
                switch (value)
                {
                    case true:
                        RangeChange?.Invoke(BossRange.Inside);
                        break;
                    case false:
                        RangeChange?.Invoke(BossRange.Outside);
                        break;
                }

                m_inAttackRange = value;
            }
        }

        private bool CanTeleport => Time.time - timeSinceLastTeleport >= teleportCooldown;

        private void Awake()
        {
            // My solution to avoid GameObject.Find
            // is finding a unique component of the object I need
            // example: Boss.Controller class is only used for the Boss GameObject
            boss = FindObjectOfType<Boss.Controller>().transform;
            playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        }

        private void UpdateAttackRange()
        {
            float angleToBoss = Vector3.Angle(playerCamera.transform.forward, DirectionToBoss);

            var newRange = (angleToBoss <= aimThreshold);
            if (InAttackRange == newRange)
                return;
            InAttackRange = newRange;
        }

        private void StandardTeleport()
        {
                if (Physics.Raycast(origin: player.position, direction: InputMovement3D, hitInfo: out RaycastHit hit, 
                        maxDistance: maxTeleportDistance))
                { 
                    player.position = hit.point - InputMovement3D * 0.5f; 
                }
                else
                { 
                    player.position += InputMovement3D * maxTeleportDistance;
                }
            
        }

        private bool AttackingBoss()
        {
            if (!(InputMovement3D.z > 0)) return false;
            UpdateAttackRange();
            if (!InAttackRange) return false;
            float distanceToBoss = Vector3.Distance(player.position, boss.position);

            player.position += DirectionToBoss * distanceToBoss;
            return true;
        }

        public void OnTeleport(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!CanTeleport) return;
                
                timeSinceLastTeleport = Time.time;
                player.velocity = Vector3.zero;

                InputMovement3D = new Vector3(playerMovement.movementInput.x, 0, playerMovement.movementInput.y).normalized;

                if (AttackingBoss()) return;

                StandardTeleport();
            }
        }
    }
}