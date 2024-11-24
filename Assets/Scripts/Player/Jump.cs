using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Jump : MonoBehaviour
    {
        [SerializeField] private Movement playerMovement;
        [SerializeField] private Rigidbody player;

        public void OnJump(InputAction.CallbackContext context)
        {
            Debug.Log(playerMovement.isGrounded);
            if (context.performed && playerMovement.isGrounded)
            {
                player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
                player.AddForce(Vector3.up * 5, ForceMode.Impulse);
            }
        }
    }
}
