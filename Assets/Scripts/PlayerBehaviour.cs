using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody player;

    [Header("Player Movement")]
    private Vector2 playerInput = Vector2.zero;
    private Vector3 movement;
    [SerializeField] private float speed;
    [SerializeField] private Transform playerRotator;
    private int inputDirection = 1;
    private int playerCameraReverser = -2;

    [Header("Player Teleport")]
    private readonly float teleportDistanceMultiplier = 3;
    private readonly float teleportCooldown = 2;
    private float timeSinceLastTeleport = 0;
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask;
    public float groundDrag;
    bool isGrounded;
    public float playerHeight;

    [Header("Other Collision")]
    public GameObject inputReverserWall;
    public GameObject cameraReverserWall;
    
    [Header("Boss")]
    public BossBehaviour boss;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        CheckForDrag();
        movement = new Vector3(playerInput.x, 0, playerInput.y);
        player.AddForce(speed * Time.fixedDeltaTime * movement.normalized, ForceMode.Impulse); 
        SpeedControl();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInput = context.ReadValue<Vector2>();
            RotationBasedInputs();
        } else if (context.canceled)
        {
            playerInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            player.velocity = new Vector3(player.velocity.x, 0, player.velocity.z);
            player.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }

    public void OnTeleport(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time - timeSinceLastTeleport < teleportCooldown) return;
            timeSinceLastTeleport = Time.time;
            player.velocity = new Vector3(0, 0, 0);
            //if no input is pressed, teleport in the direction the player is facing
            if (playerInput == Vector2.zero)
            {
                player.position += new Vector3(Mathf.Sin(playerRotator.eulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(playerRotator.eulerAngles.y * Mathf.Deg2Rad)) * teleportDistanceMultiplier;
            }
            //if input is pressed, teleport to the direction of the inputs
            else
            {
                player.position += new Vector3(playerInput.x, 0, playerInput.y) * teleportDistanceMultiplier;
            }
            
        }
    }

    //this function makes sure that the player's speed is capped at a certain value
    private void SpeedControl()
    {
        Vector3 horizontalVelocity = new(player.velocity.x, 0, player.velocity.z);
        if (horizontalVelocity.magnitude > (speed / 4))
        {
            Vector3 cappedVelocity = horizontalVelocity.normalized * (speed / 4);
            player.velocity = new Vector3(cappedVelocity.x, player.velocity.y, cappedVelocity.z);
        }
    }

    //this function changes the player's input direction based on the rotation of the camera & player
    private void RotationBasedInputs()
    {
        if (playerInput == Vector2.zero)
        {
            return;
        }
        Vector3 rotatedInput = Quaternion.Euler(0, playerRotator.eulerAngles.y, 0) * new Vector3(playerInput.x, 0, playerInput.y);
        playerInput = new Vector2(rotatedInput.x, rotatedInput.z) * inputDirection;
    }

    //this function checks for ground distance for the player and then applies drag if it does touch the ground
    private void CheckForDrag()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.6f, groundMask);
        if (isGrounded)
        {
            player.drag = groundDrag;
        }
        else
        {
            player.drag = 0;
        }
    }

    public void ReverseInputs()
    {
        inputDirection *= -1;
    }

    public void ReverseCameraRotation()
    {
        playerCameraReverser *= -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        //reverse inputs when an input reverser is touched, then place the wall back behind the boss
        if (other.gameObject.CompareTag("InputReverser"))
        {
            ReverseInputs();
            inputReverserWall.transform.position = boss.GetBossPosition() + new Vector3(0, 0, 3);
        }
        //reverse camera rotation when a camera reverser is touched, then place the wall back behind the boss
        if (other.gameObject.CompareTag("CameraReverser"))
        {
            playerRotator.eulerAngles = new Vector3(playerRotator.eulerAngles.x, playerRotator.eulerAngles.y, playerRotator.eulerAngles.z + 180);
            playerRotator.transform.position = new Vector3(playerRotator.position.x, playerRotator.position.y - playerCameraReverser, playerRotator.position.z);
            ReverseCameraRotation();
            cameraReverserWall.transform.position = boss.GetBossPosition() + new Vector3(0, 0, 3);
        }
    }
}