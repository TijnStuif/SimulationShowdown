using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] private Rigidbody player;
        [SerializeField] private Movement playerMovement;
        [SerializeField] private Transform boss;
        [SerializeField] private Camera playerCamera; 
        private readonly float teleportCooldown = 2f;
        private float timeSinceLastTeleport = 0;
        private readonly float aimThreshold = 30f; 
        private readonly float maxTeleportDistance = 10f;

        // hotfix solution
        private void Awake()
        {
            boss = GameObject.Find("Boss").transform;
            playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        }

        public void OnTeleport(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (Time.time - timeSinceLastTeleport < teleportCooldown) return;
                timeSinceLastTeleport = Time.time;
                player.velocity = Vector3.zero;

                Vector3 movementInput = new Vector3(playerMovement.movementInput.x, 0, playerMovement.movementInput.y).normalized;

                if (movementInput.z > 0)
                {
                    Vector3 directionToBoss = (boss.position - player.position).normalized;

                    float angleToBoss = Vector3.Angle(playerCamera.transform.forward, directionToBoss);

                    if (angleToBoss <= aimThreshold)
                    {
                        float distanceToBoss = Vector3.Distance(player.position, boss.position);

                        player.position += directionToBoss * distanceToBoss;
                        return;
                    }
                }

                RaycastHit hit;
                if (Physics.Raycast(player.position, movementInput, out hit, maxTeleportDistance))
                {
                    player.position = hit.point - movementInput * 0.5f; 
                }
                else
                {
                    player.position += movementInput * maxTeleportDistance;
                }
            }
        }
    }
}