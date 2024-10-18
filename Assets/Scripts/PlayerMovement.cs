using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 movementInput = Vector2.zero;
    public bool isGrounded;
    private Vector3 movement;
    private float groundDrag = 0.2f;
    [SerializeField] private Rigidbody player;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundMask;
    private float speedControlMultiplier = 0.4f;

    void FixedUpdate()
    {
        movement = new Vector3(movementInput.x, 0, movementInput.y);
        player.AddForce(speed * Time.fixedDeltaTime * movement.normalized, ForceMode.Impulse); 
        SpeedControl();
        CheckForDrag();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            movementInput = Vector2.zero;
        }
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
