using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private Rigidbody player;
    [SerializeField] private Transform playerRotator;
    [SerializeField] private PlayerMovement playerMovement;
    private readonly float teleportDistanceMultiplier = 2.5f;
    private readonly float teleportCooldown = 2;
    private float timeSinceLastTeleport = 0;

    public void OnTeleport(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time - timeSinceLastTeleport < teleportCooldown) return;
            timeSinceLastTeleport = Time.time;
            player.velocity = new Vector3(0, 0, 0);
            
            //if no input is pressed, teleport in the direction the player is facing
            if (playerMovement.movementInput == Vector2.zero)
            {
                player.position += new Vector3(Mathf.Sin(playerRotator.eulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(playerRotator.eulerAngles.y * Mathf.Deg2Rad)) * teleportDistanceMultiplier;
            }
            //if input is pressed, teleport to the direction of the inputs
            else
            {
                player.position += new Vector3(playerMovement.movementInput.x, 0, playerMovement.movementInput.y) * teleportDistanceMultiplier;
            }    
        }
    }
}
