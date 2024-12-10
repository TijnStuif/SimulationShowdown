using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.V1
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Rigidbody player;
        [SerializeField] private float speed;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private Transform playerRotator;
        [NonSerialized] public Vector2 movementInput = Vector2.zero;
        [NonSerialized] public bool isGrounded;
        private Vector3 movement;
        private float groundDrag = 0.2f;
        private float speedControlMultiplier = 0.4f;
        public bool areControlsInverted = false;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void FixedUpdate()
        {
            movement = new Vector3(movementInput.x, 0, movementInput.y);
            SpeedControl();
            CheckForDrag();
            player.AddForce(speed * Time.fixedDeltaTime * movement.normalized, ForceMode.Impulse); 
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                movementInput = context.ReadValue<Vector2>();
                RotationBasedInputs();
            }
            else if (context.canceled)
            {
                movementInput = Vector2.zero;
            }
        }
    
        //this function rotates the player's inputs based on the rotation of the player's inherent rotation
        private void RotationBasedInputs()
        {
            if (movementInput == Vector2.zero)
            {
                return;
            }

            // Invert the input direction if controls are inverted
            Vector3 rotatedInput = Quaternion.Euler(0, playerRotator.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);
            movementInput = new Vector2(rotatedInput.x, rotatedInput.z) * (areControlsInverted ? -1 : 1);
        }

        //this function makes sure that the player's speed is capped at a certain value
        private void SpeedControl()
        {
            Vector3 horizontalVelocity = new(player.velocity.x, 0, player.velocity.z);

            if (horizontalVelocity.magnitude > (speed * speedControlMultiplier))
            {
                Vector3 cappedVelocity = horizontalVelocity.normalized * (speed * speedControlMultiplier);
                player.velocity = new Vector3(cappedVelocity.x, player.velocity.y, cappedVelocity.z);
            }
        }

        //this function checks for ground distance for the player and then applies drag if it does touch the ground
        private void CheckForDrag()
        {
            isGrounded = Physics.Raycast(player.transform.position, Vector3.down, 1.1f, groundMask);

            if (isGrounded)
            {
                player.drag = groundDrag;
            }
            else
            {
                player.drag = 0;
            }
        }
    }
}
